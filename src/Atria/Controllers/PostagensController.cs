using Atria.Data;
using Atria.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace Atria.Controllers
{
    [Authorize]
    public class PostagensController : Controller
    {
      private readonly ApplicationDbContext _context;

        public PostagensController(ApplicationDbContext context)
        {
  _context = context;
        }

  [AllowAnonymous]
        public async Task<IActionResult> Index()
        {
            try
   {
         var postagens = await _context.Postagens
   .Include(p => p.Usuario)
   .Include(p => p.Comunidade)
            .Include(p => p.GrupoEstudo)
           .Where(p => p.Titulo != null && p.Conteudo != null) // Filtrar postagens válidas
            .ToListAsync();
    
            // ? NOVO: Carregar comunidades para o filtro
   var comunidades = await _context.Comunidades
             .OrderBy(c => c.Nome)
                .ToListAsync();

            ViewBag.Comunidades = comunidades;
      
      return View(postagens);
            }
     catch (Exception ex)
 {
         // Log do erro (você pode adicionar um logger aqui)
            ViewBag.ErrorMessage = "Erro ao carregar postagens: " + ex.Message;
    ViewBag.Comunidades = new List<Comunidade>(); // Lista vazia em caso de erro
      return View(new List<Postagem>());
     }
   }

        [AllowAnonymous]
        public async Task<IActionResult> Details(int? id)
     {
        if (id == null) return NotFound();
  
            var postagem = await _context.Postagens
       .Include(p => p.Usuario)
      .Include(p => p.Comunidade)
   .Include(p => p.GrupoEstudo)
   .Include(p => p.Comentarios!)
    .ThenInclude(c => c.Usuario)
    .FirstOrDefaultAsync(p => p.Id == id);
    
 if (postagem == null) return NotFound();
   
        return View(postagem);
        }

        // Accept optional comunidadeId and grupoId so the same Create view can be used from multiple screens
        public async Task<IActionResult> Create(int? comunidadeId, int? grupoId)
     {
      ViewBag.ComunidadeId = comunidadeId;
    ViewBag.GrupoId = grupoId;
    
     // Carregar listas para dropdowns
     var comunidades = await _context.Comunidades
     .OrderBy(c => c.Nome)
    .ToListAsync();
        
 var grupos = await _context.GruposEstudo
          .OrderBy(g => g.Nome)
     .ToListAsync();
            
     ViewBag.Comunidades = new SelectList(comunidades, "Id", "Nome", comunidadeId);
            ViewBag.Grupos = new SelectList(grupos, "Id", "Nome", grupoId);
    
    return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
 public async Task<IActionResult> Create([Bind("Titulo,Conteudo,FKComunidade,FKGrupo,NoForumGeral")] Postagem postagem)
  {
 if (!ModelState.IsValid)
            {
// Debug: mostrar erros de validação
       var errors = ModelState.Values.SelectMany(v => v.Errors);
  ViewBag.ErrorMessage = "Há erros no formulário. Verifique os campos.";
       
         // Recarregar listas
       var comunidades = await _context.Comunidades.OrderBy(c => c.Nome).ToListAsync();
   var grupos = await _context.GruposEstudo.OrderBy(g => g.Nome).ToListAsync();
      ViewBag.Comunidades = new SelectList(comunidades, "Id", "Nome", postagem.FKComunidade);
    ViewBag.Grupos = new SelectList(grupos, "Id", "Nome", postagem.FKGrupo);
       
       return View(postagem);
      }
     
          var userIdClaim = User.FindFirst("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier");
            if (userIdClaim == null) 
    {
    ViewBag.ErrorMessage = "Você precisa estar logado para criar postagens.";
  return Challenge();
}
            
   if (!int.TryParse(userIdClaim.Value, out var userId)) 
    {
          ViewBag.ErrorMessage = "Erro ao identificar usuário.";
         return BadRequest();
}

  try
    {
     postagem.FKUsuario = userId;
        postagem.DataPostagem = DateTime.UtcNow;
                
     // Garantir valores padrão para FKs opcionais
   if (!postagem.FKComunidade.HasValue || postagem.FKComunidade == 0)
        postagem.FKComunidade = null;
       
     if (!postagem.FKGrupo.HasValue || postagem.FKGrupo == 0)
  postagem.FKGrupo = null;
        
     _context.Add(postagem);
     await _context.SaveChangesAsync();
 
      TempData["SuccessMessage"] = "Postagem criada com sucesso!";
  return RedirectToAction(nameof(Index));
            }
         catch (Exception ex)
       {
            ViewBag.ErrorMessage = $"Erro ao salvar postagem: {ex.Message}";
  
            // Recarregar listas
var comunidades = await _context.Comunidades.OrderBy(c => c.Nome).ToListAsync();
   var grupos = await _context.GruposEstudo.OrderBy(g => g.Nome).ToListAsync();
       ViewBag.Comunidades = new SelectList(comunidades, "Id", "Nome", postagem.FKComunidade);
      ViewBag.Grupos = new SelectList(grupos, "Id", "Nome", postagem.FKGrupo);
    
    return View(postagem);
   }
        }

        public async Task<IActionResult> Edit(int? id)
        {
        if (id == null) return NotFound();
       var postagem = await _context.Postagens.FindAsync(id);
    if (postagem == null) return NotFound();
    
     // Carregar listas para edição
      var comunidades = await _context.Comunidades.OrderBy(c => c.Nome).ToListAsync();
            var grupos = await _context.GruposEstudo.OrderBy(g => g.Nome).ToListAsync();
            ViewBag.Comunidades = new SelectList(comunidades, "Id", "Nome", postagem.FKComunidade);
            ViewBag.Grupos = new SelectList(grupos, "Id", "Nome", postagem.FKGrupo);
    
   return View(postagem);
        }

        [HttpPost]
   [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, [Bind("Id,Titulo,Conteudo,FKComunidade,FKGrupo,NoForumGeral")] Postagem postagemEditada)
      {
     if (id != postagemEditada.Id) return NotFound();
 
      if (ModelState.IsValid)
      {
          try
  {
           // Buscar postagem original do banco para preservar FK_USUARIO e DATA_POSTAGEM
  var postagemOriginal = await _context.Postagens.FindAsync(id);
     if (postagemOriginal == null) return NotFound();
         
   // Atualizar APENAS os campos editáveis
postagemOriginal.Titulo = postagemEditada.Titulo;
          postagemOriginal.Conteudo = postagemEditada.Conteudo;
           postagemOriginal.FKComunidade = postagemEditada.FKComunidade;
        postagemOriginal.FKGrupo = postagemEditada.FKGrupo;
          postagemOriginal.NoForumGeral = postagemEditada.NoForumGeral;
        
       // FK_USUARIO e DATA_POSTAGEM são preservados automaticamente!
            
          _context.Update(postagemOriginal);
        await _context.SaveChangesAsync();
        
TempData["SuccessMessage"] = "Postagem atualizada com sucesso!";
        return RedirectToAction(nameof(Index));
       }
   catch (DbUpdateConcurrencyException)
      {
        if (!_context.Postagens.Any(e => e.Id == postagemEditada.Id)) 
return NotFound();
   else 
        throw;
   }
      }
      
      // Recarregar listas em caso de erro
  var comunidades = await _context.Comunidades.OrderBy(c => c.Nome).ToListAsync();
      var grupos = await _context.GruposEstudo.OrderBy(g => g.Nome).ToListAsync();
      ViewBag.Comunidades = new SelectList(comunidades, "Id", "Nome", postagemEditada.FKComunidade);
  ViewBag.Grupos = new SelectList(grupos, "Id", "Nome", postagemEditada.FKGrupo);
      
  return View(postagemEditada);
        }

     public async Task<IActionResult> Delete(int? id)
        {
   if (id == null) return NotFound();
var postagem = await _context.Postagens
     .Include(p => p.Usuario)
   .Include(p => p.Comunidade)
  .Include(p => p.GrupoEstudo)
    .FirstOrDefaultAsync(p => p.Id == id);
            if (postagem == null) return NotFound();
    return View(postagem);
    }

    [HttpPost, ActionName("Delete")]
  [ValidateAntiForgeryToken]
public async Task<IActionResult> DeleteConfirmed(int id)
    {
     var postagem = await _context.Postagens.FindAsync(id);
  if (postagem != null)
            {
_context.Postagens.Remove(postagem);
     await _context.SaveChangesAsync();
     TempData["SuccessMessage"] = "Postagem removida com sucesso!";
            }
    return RedirectToAction(nameof(Index));
        }
    }
}