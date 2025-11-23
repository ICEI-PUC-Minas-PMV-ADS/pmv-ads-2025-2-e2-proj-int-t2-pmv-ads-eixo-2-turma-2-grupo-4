using Atria.Data;
using Atria.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Atria.Controllers
{
    [Authorize]
    public class ComentarioAvaliacaoController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ComentarioAvaliacaoController(ApplicationDbContext context)
        {
    _context = context;
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Conteudo,FKAvaliacao,FKComentarioPai")] ComentarioAvaliacao comentario)
        {
  // Remove validação de modelo para debug
  ModelState.Remove("Usuario");
   ModelState.Remove("Avaliacao");
   ModelState.Remove("ComentarioPai");
      ModelState.Remove("Respostas");
            
     if (!ModelState.IsValid)
     {
     TempData["ErrorMessage"] = "Há erros no formulário. Verifique os campos.";
    
        // Buscar material da avaliação para redirecionar corretamente
       var avaliacao = await _context.Avaliacoes.FindAsync(comentario.FKAvaliacao);
     if (avaliacao != null)
   {
     return RedirectToAction("Details", "Materiais", new { id = avaliacao.FKMaterial });
     }
                return RedirectToAction("Index", "Materiais");
   }

 var userIdClaim = User.FindFirst("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier");
      if (userIdClaim == null)
  {
   TempData["ErrorMessage"] = "Você precisa estar logado para comentar.";
  
     var avaliacao = await _context.Avaliacoes.FindAsync(comentario.FKAvaliacao);
              if (avaliacao != null)
     {
          return RedirectToAction("Details", "Materiais", new { id = avaliacao.FKMaterial });
     }
       return RedirectToAction("Index", "Materiais");
 }

            if (!int.TryParse(userIdClaim.Value, out var userId))
            {
      TempData["ErrorMessage"] = "Erro ao identificar usuário.";
    
        var avaliacao = await _context.Avaliacoes.FindAsync(comentario.FKAvaliacao);
             if (avaliacao != null)
      {
            return RedirectToAction("Details", "Materiais", new { id = avaliacao.FKMaterial });
       }
       return RedirectToAction("Index", "Materiais");
          }

            try
            {
        comentario.FKUsuario = userId;
 comentario.DataComentario = DateTime.Now;

           _context.Add(comentario);
         await _context.SaveChangesAsync();

   TempData["SuccessMessage"] = comentario.FKComentarioPai.HasValue 
               ? "Resposta adicionada com sucesso!" 
         : "Comentário adicionado com sucesso!";
     
                // Redirecionar para a página do material
       var avaliacaoFinal = await _context.Avaliacoes.FindAsync(comentario.FKAvaliacao);
                if (avaliacaoFinal != null)
        {
         return RedirectToAction("Details", "Materiais", new { id = avaliacaoFinal.FKMaterial });
            }
    
         return RedirectToAction("Index", "Materiais");
        }
catch (Exception ex)
        {
      TempData["ErrorMessage"] = $"Erro ao salvar comentário: {ex.Message}";
     Console.WriteLine($"ERRO DETALHADO: {ex.ToString()}");
        
      var avaliacao = await _context.Avaliacoes.FindAsync(comentario.FKAvaliacao);
   if (avaliacao != null)
        {
   return RedirectToAction("Details", "Materiais", new { id = avaliacao.FKMaterial });
           }
       return RedirectToAction("Index", "Materiais");
}
        }

  [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
  try
            {
   var comentario = await _context.ComentariosAvaliacao
    .Include(c => c.Avaliacao)
         .Include(c => c.Respostas)
        .FirstOrDefaultAsync(c => c.Id == id);
                
       if (comentario == null)
      {
         TempData["ErrorMessage"] = "Comentário não encontrado.";
    return RedirectToAction("Index", "Materiais");
 }

    // Verificar se o usuário é o autor do comentário
     var userIdClaim = User.FindFirst("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier");
     if (userIdClaim == null || !int.TryParse(userIdClaim.Value, out var userId) || comentario.FKUsuario != userId)
                {
         TempData["ErrorMessage"] = "Você não tem permissão para excluir este comentário.";
       return RedirectToAction("Details", "Materiais", new { id = comentario.Avaliacao!.FKMaterial });
    }

     var materialId = comentario.Avaliacao!.FKMaterial;
        
    // Remove todas as respostas primeiro
     if (comentario.Respostas != null && comentario.Respostas.Any())
                {
        _context.ComentariosAvaliacao.RemoveRange(comentario.Respostas);
      }
  
      _context.ComentariosAvaliacao.Remove(comentario);
 await _context.SaveChangesAsync();

             TempData["SuccessMessage"] = "Comentário removido com sucesso!";
      return RedirectToAction("Details", "Materiais", new { id = materialId });
  }
         catch (Exception ex)
         {
   TempData["ErrorMessage"] = $"Erro ao remover comentário: {ex.Message}";
     return RedirectToAction("Index", "Materiais");
  }
 }
    }
}
