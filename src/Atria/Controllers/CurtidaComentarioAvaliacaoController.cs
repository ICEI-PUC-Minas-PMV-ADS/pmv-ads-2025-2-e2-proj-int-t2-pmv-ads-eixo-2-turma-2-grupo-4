using Atria.Data;
using Atria.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Atria.Controllers
{
    [Authorize]
    public class CurtidaComentarioAvaliacaoController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CurtidaComentarioAvaliacaoController(ApplicationDbContext context)
     {
    _context = context;
  }

  [HttpPost]
        public async Task<IActionResult> Toggle(int comentarioAvaliacaoId)
        {
 var userIdClaim = User.FindFirst("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier");
    if (userIdClaim == null || !int.TryParse(userIdClaim.Value, out var userId))
    {
    return Json(new { success = false, message = "Usuário não autenticado" });
            }

    try
         {
      // Verifica se o usuário já curtiu este comentário
   var curtidaExistente = await _context.CurtidasComentarioAvaliacao
        .FirstOrDefaultAsync(c => c.FKComentarioAvaliacao == comentarioAvaliacaoId && c.FKUsuario == userId);

          if (curtidaExistente != null)
         {
   // Remove a curtida (descurtir)
        _context.CurtidasComentarioAvaliacao.Remove(curtidaExistente);
      await _context.SaveChangesAsync();

     // Conta total de curtidas
         var totalCurtidas = await _context.CurtidasComentarioAvaliacao
   .CountAsync(c => c.FKComentarioAvaliacao == comentarioAvaliacaoId);

    return Json(new { success = true, curtido = false, totalCurtidas });
 }
                else
       {
    // Adiciona nova curtida
      var novaCurtida = new CurtidaComentarioAvaliacao
 {
     FKComentarioAvaliacao = comentarioAvaliacaoId,
FKUsuario = userId,
   DataCurtida = DateTime.UtcNow,
  Tipo = "LIKE"
};

_context.CurtidasComentarioAvaliacao.Add(novaCurtida);
      await _context.SaveChangesAsync();

          // Conta total de curtidas
         var totalCurtidas = await _context.CurtidasComentarioAvaliacao
    .CountAsync(c => c.FKComentarioAvaliacao == comentarioAvaliacaoId);

    return Json(new { success = true, curtido = true, totalCurtidas });
         }
  }
   catch (Exception ex)
   {
     return Json(new { success = false, message = $"Erro: {ex.Message}" });
   }
        }

        // API para obter status de curtida e total
      [HttpGet]
        public async Task<IActionResult> GetStatus(int comentarioAvaliacaoId)
        {
   var userIdClaim = User.FindFirst("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier");
   var userId = userIdClaim != null && int.TryParse(userIdClaim.Value, out var id) ? id : 0;

          var totalCurtidas = await _context.CurtidasComentarioAvaliacao
        .CountAsync(c => c.FKComentarioAvaliacao == comentarioAvaliacaoId);

         var curtido = userId > 0 && await _context.CurtidasComentarioAvaliacao
        .AnyAsync(c => c.FKComentarioAvaliacao == comentarioAvaliacaoId && c.FKUsuario == userId);

       return Json(new { totalCurtidas, curtido });
        }
    }
}
