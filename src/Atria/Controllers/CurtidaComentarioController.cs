using Atria.Data;
using Atria.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Atria.Controllers
{
    [Authorize]
    public class CurtidaComentarioController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CurtidaComentarioController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpPost]
        [IgnoreAntiforgeryToken] // NOVO: Permitir requisições AJAX sem token
        public async Task<IActionResult> Toggle([FromBody] CurtidaRequest request)
        {
            var userIdClaim = User.FindFirst("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier");
            if (userIdClaim == null || !int.TryParse(userIdClaim.Value, out var userId))
            {
                return Json(new { success = false, message = "Usuário não autenticado" });
            }

            try
            {
                // Verifica se o usuário já curtiu este comentário
                var curtidaExistente = await _context.CurtidasComentario
                    .FirstOrDefaultAsync(c => c.FKComentario == request.ComentarioId && c.FKUsuario == userId);

                if (curtidaExistente != null)
                {
                    // Remove a curtida (descurtir)
                    _context.CurtidasComentario.Remove(curtidaExistente);
                    await _context.SaveChangesAsync();

                    // Conta total de curtidas
                    var totalCurtidas = await _context.CurtidasComentario
                        .CountAsync(c => c.FKComentario == request.ComentarioId);

                    return Json(new { success = true, curtido = false, totalCurtidas });
                }
                else
                {
                    // Adiciona nova curtida
                    var novaCurtida = new CurtidaComentario
                    {
                        FKComentario = request.ComentarioId,
                        FKUsuario = userId,
                        DataCurtida = DateTime.UtcNow,
                        Tipo = "LIKE"
                    };

                    _context.CurtidasComentario.Add(novaCurtida);
                    await _context.SaveChangesAsync();

                    // Conta total de curtidas
                    var totalCurtidas = await _context.CurtidasComentario
                        .CountAsync(c => c.FKComentario == request.ComentarioId);

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
        public async Task<IActionResult> GetStatus(int comentarioId)
        {
            var userIdClaim = User.FindFirst("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier");
            var userId = userIdClaim != null && int.TryParse(userIdClaim.Value, out var id) ? id : 0;

            var totalCurtidas = await _context.CurtidasComentario
                .CountAsync(c => c.FKComentario == comentarioId);

            var curtido = userId > 0 && await _context.CurtidasComentario
                .AnyAsync(c => c.FKComentario == comentarioId && c.FKUsuario == userId);

            return Json(new { totalCurtidas, curtido });
        }
    }

    // Classe helper para receber JSON
    public class CurtidaRequest
    {
        public int ComentarioId { get; set; }
    }
}
