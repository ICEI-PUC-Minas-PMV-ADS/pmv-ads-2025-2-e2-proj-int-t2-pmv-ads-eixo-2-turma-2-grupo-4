using Atria.Data;
using Atria.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Atria.Controllers
{
    [Authorize]
    public class NotificacaoController : Controller
    {
        private readonly ApplicationDbContext _context;

        public NotificacaoController(ApplicationDbContext context)
        {
            _context = context;
        }

        // API para obter notificações do usuário logado
        [HttpGet]
        public async Task<IActionResult> GetNotificacoes()
        {
            var userIdClaim = User.FindFirst("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier");
            if (userIdClaim == null || !int.TryParse(userIdClaim.Value, out var userId))
            {
                return Json(new { success = false, message = "Usuário não autenticado" });
            }

            try
            {
                var notificacoes = await _context.Notificacoes
                    .Where(n => n.FKUsuario == userId)
                    .OrderByDescending(n => n.DataCriacao)
                    .Take(50)
                    .Select(n => new
                    {
                        n.Id,
                        n.Titulo,
                        n.Mensagem,
                        n.Tipo,
                        n.Link,
                        n.Icone,
                        n.Cor,
                        n.Lida,
                        Data = n.DataCriacao.ToString("o")
                    })
                    .ToListAsync();

                return Json(new { success = true, notificacoes });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = $"Erro: {ex.Message}" });
            }
        }

        // API para marcar notificação como lida
        [HttpPost]
        [IgnoreAntiforgeryToken]
        public async Task<IActionResult> MarcarComoLida([FromBody] MarcarLidaRequest request)
        {
            var userIdClaim = User.FindFirst("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier");
            if (userIdClaim == null || !int.TryParse(userIdClaim.Value, out var userId))
            {
                return Json(new { success = false, message = "Usuário não autenticado" });
            }

            try
            {
                var notificacao = await _context.Notificacoes
                    .FirstOrDefaultAsync(n => n.Id == request.NotificacaoId && n.FKUsuario == userId);

                if (notificacao != null)
                {
                    notificacao.Lida = true;
                    await _context.SaveChangesAsync();
                    return Json(new { success = true });
                }

                return Json(new { success = false, message = "Notificação não encontrada" });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = $"Erro: {ex.Message}" });
            }
        }

        // API para limpar todas as notificações
        [HttpPost]
        [IgnoreAntiforgeryToken]
        public async Task<IActionResult> LimparTodas()
        {
            var userIdClaim = User.FindFirst("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier");
            if (userIdClaim == null || !int.TryParse(userIdClaim.Value, out var userId))
            {
                return Json(new { success = false, message = "Usuário não autenticado" });
            }

            try
            {
                var notificacoes = await _context.Notificacoes
                    .Where(n => n.FKUsuario == userId)
                    .ToListAsync();

                _context.Notificacoes.RemoveRange(notificacoes);
                await _context.SaveChangesAsync();

                return Json(new { success = true });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = $"Erro: {ex.Message}" });
            }
        }

        // API para contar notificações não lidas
        [HttpGet]
        public async Task<IActionResult> ContarNaoLidas()
        {
            var userIdClaim = User.FindFirst("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier");
            if (userIdClaim == null || !int.TryParse(userIdClaim.Value, out var userId))
            {
                return Json(new { success = false, count = 0 });
            }

            try
            {
                var count = await _context.Notificacoes
                    .CountAsync(n => n.FKUsuario == userId && !n.Lida);

                return Json(new { success = true, count });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, count = 0, message = $"Erro: {ex.Message}" });
            }
        }
    }

    public class MarcarLidaRequest
    {
        public int NotificacaoId { get; set; }
    }
}
