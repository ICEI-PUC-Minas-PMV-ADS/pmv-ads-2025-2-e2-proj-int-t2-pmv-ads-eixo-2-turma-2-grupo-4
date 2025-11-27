using Atria.Data;
using Atria.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Atria.Controllers
{
    [Authorize]
    public class ComentarioController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ComentarioController(ApplicationDbContext context)
        {
            _context = context;
        }

        [AllowAnonymous]
        public async Task<IActionResult> Index()
        {
            var comentarios = await _context.Comentarios
         .Include(c => c.Usuario)
       .Include(c => c.Postagem)
     .Include(c => c.Curtidas)
              .OrderByDescending(c => c.DataComentario)
       .ToListAsync();

       return View(comentarios);
        }

        [AllowAnonymous]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();
            var comentario = await _context.Comentarios
                .Include(c => c.Usuario)
                .Include(c => c.Postagem)
                .Include(c => c.Curtidas)
                .AsSplitQuery() // NOVO: Otimização
                .FirstOrDefaultAsync(c => c.Id == id);
            if (comentario == null) return NotFound();
            return View(comentario);
        }

        // GET: Comentario/Create?postagemId=1
        public IActionResult Create(int? postagemId)
        {
            ViewBag.PostagemId = postagemId;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Conteudo,FKPostagem,FKComentarioPai")] Comentario comentario)
        {
            ModelState.Remove("Usuario");
            ModelState.Remove("Postagem");
            ModelState.Remove("ComentarioPai");
            ModelState.Remove("Respostas");
            ModelState.Remove("Curtidas"); // NOVO: Remover validação de curtidas

            if (!ModelState.IsValid)
            {
                TempData["ErrorMessage"] = "Há erros no formulário. Verifique os campos.";
                return RedirectToAction("Details", "Postagens", new { id = comentario.FKPostagem });
            }

            var userIdClaim = User.FindFirst("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier");
            if (userIdClaim == null)
            {
                TempData["ErrorMessage"] = "Você precisa estar logado para comentar.";
                return RedirectToAction("Details", "Postagens", new { id = comentario.FKPostagem });
            }

            if (!int.TryParse(userIdClaim.Value, out var userId))
            {
                TempData["ErrorMessage"] = "Erro ao identificar usuário.";
                return RedirectToAction("Details", "Postagens", new { id = comentario.FKPostagem });
            }

            try
            {
                comentario.FKUsuario = userId;
                comentario.DataComentario = DateTime.UtcNow;

                _context.Add(comentario);
                await _context.SaveChangesAsync();

                // NOVO: Criar notificação
                var postagem = await _context.Postagens
                    .Include(p => p.Usuario)
                    .FirstOrDefaultAsync(p => p.Id == comentario.FKPostagem);

                if (postagem != null)
                {
                    var usuario = await _context.Users.FindAsync(userId);

                    // Se for resposta a um comentário
                    if (comentario.FKComentarioPai.HasValue)
                    {
                        var comentarioPai = await _context.Comentarios
                            .Include(c => c.Usuario)
                            .FirstOrDefaultAsync(c => c.Id == comentario.FKComentarioPai);

                        if (comentarioPai != null && comentarioPai.FKUsuario != userId)
                        {
                            var notificacao = new Notificacao
                            {
                                FKUsuario = comentarioPai.FKUsuario,
                                Titulo = "Nova resposta",
                                Mensagem = $"{usuario?.Nome ?? "Alguém"} respondeu ao seu comentário",
                                Tipo = "RESPOSTA",
                                Link = $"/Postagens/Details/{postagem.Id}",
                                Icone = "bi-reply-fill",
                                Cor = "text-success",
                                Lida = false,
                                DataCriacao = DateTime.UtcNow
                            };

                            _context.Notificacoes.Add(notificacao);
                        }
                    }
                    // Se for comentário direto na postagem
                    else if (postagem.FKUsuario != userId)
                    {
                        var notificacao = new Notificacao
                        {
                            FKUsuario = postagem.FKUsuario,
                            Titulo = "Novo comentário",
                            Mensagem = $"{usuario?.Nome ?? "Alguém"} comentou em sua postagem \"{postagem.Titulo.Substring(0, Math.Min(50, postagem.Titulo.Length))}...\"",
                            Tipo = "COMENTARIO",
                            Link = $"/Postagens/Details/{postagem.Id}",
                            Icone = "bi-chat-dots-fill",
                            Cor = "text-primary",
                            Lida = false,
                            DataCriacao = DateTime.UtcNow
                        };

                        _context.Notificacoes.Add(notificacao);
                    }

                    await _context.SaveChangesAsync();
                }

                TempData["SuccessMessage"] = comentario.FKComentarioPai.HasValue 
                    ? "Resposta adicionada com sucesso!" 
                    : "Comentário adicionado com sucesso!";
    
                return RedirectToAction("Details", "Postagens", new { id = comentario.FKPostagem });
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Erro ao salvar comentário: {ex.Message}";
                return RedirectToAction("Details", "Postagens", new { id = comentario.FKPostagem });
            }
        }

      public async Task<IActionResult> Edit(int? id)
      {
      if (id == null) return NotFound();
    var comentario = await _context.Comentarios.FindAsync(id);
     if (comentario == null) return NotFound();
            return View(comentario);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Conteudo,FKPostagem")] Comentario comentario)
        {
            if (id != comentario.Id) return NotFound();
         if (ModelState.IsValid)
            {
      try
          {
         _context.Update(comentario);
     await _context.SaveChangesAsync();
     }
     catch (DbUpdateConcurrencyException)
  {
        if (!_context.Comentarios.Any(e => e.Id == comentario.Id)) return NotFound();
   else throw;
     }
   return RedirectToAction("Details", "Postagens", new { id = comentario.FKPostagem });
            }
            return View(comentario);
   }

        public async Task<IActionResult> Delete(int? id)
        {
     if (id == null) return NotFound();
            var comentario = await _context.Comentarios
    .Include(c => c.Usuario)
     .Include(c => c.Postagem)
       .FirstOrDefaultAsync(c => c.Id == id);
 if (comentario == null) return NotFound();
      return View(comentario);
   }

    [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
  {
     var comentario = await _context.Comentarios
         .Include(c => c.Respostas) // Incluir respostas diretas
     .ThenInclude(r => r.Respostas) // Incluir respostas das respostas (nível 2)
            .Include(c => c.Curtidas) // NOVO: Incluir curtidas para deletar em cascata
     .FirstOrDefaultAsync(c => c.Id == id);
        
      if (comentario != null)
        {
  var postagemId = comentario.FKPostagem;
      
   // NOVO: Método recursivo para remover todos os comentários aninhados
      await RemoverComentarioRecursivamente(comentario);
  
            // NOVO: Salvar todas as mudanças de uma vez
   await _context.SaveChangesAsync();

     TempData["SuccessMessage"] = "Comentário excluído com sucesso!";
        return RedirectToAction("Details", "Postagens", new { id = postagemId });
  }
     return RedirectToAction("Index");
        }

        // NOVO: Método auxiliar para remover recursivamente
 private async Task RemoverComentarioRecursivamente(Comentario comentario)
        {
  // Carregar todas as respostas aninhadas
       var respostas = await _context.Comentarios
   .Where(c => c.FKComentarioPai == comentario.Id)
         .Include(c => c.Curtidas)
      .ToListAsync();

        // Remover recursivamente cada resposta
   foreach (var resposta in respostas)
   {
    await RemoverComentarioRecursivamente(resposta);
      }

   // Remover curtidas do comentário atual
       if (comentario.Curtidas != null && comentario.Curtidas.Any())
      {
    _context.CurtidasComentario.RemoveRange(comentario.Curtidas);
      }

   // Remover o comentário atual
  _context.Comentarios.Remove(comentario);
        }
    }
}
