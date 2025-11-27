using Atria.Data;
using Atria.Models;
using Atria.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Atria.Controllers
{
    [Authorize]
    public class SeguidoresController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly INotificacaoService _notificacaoService;

        public SeguidoresController(ApplicationDbContext context, INotificacaoService notificacaoService)
        {
            _context = context;
            _notificacaoService = notificacaoService;
        }

        // API para seguir um usuário
        [HttpPost]
        [IgnoreAntiforgeryToken]
        public async Task<IActionResult> Seguir([FromBody] SeguirRequest request)
        {
            var userIdClaim = User.FindFirst("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier");
            if (userIdClaim == null || !int.TryParse(userIdClaim.Value, out var userId))
            {
                return Json(new { success = false, message = "Usuário não autenticado" });
            }

            if (userId == request.UsuarioId)
            {
                return Json(new { success = false, message = "Você não pode seguir a si mesmo" });
            }

            try
            {
                // Verifica se já está seguindo
                var jaSeguindo = await _context.Seguidores
                    .AnyAsync(s => s.FKSeguidor == userId && s.FKSeguido == request.UsuarioId);

                if (jaSeguindo)
                {
                    return Json(new { success = false, message = "Você já está seguindo este usuário" });
                }

                // Cria o relacionamento
                var seguidor = new Seguidor
                {
                    FKSeguidor = userId,
                    FKSeguido = request.UsuarioId,
                    DataInicio = DateTime.UtcNow
                };

                _context.Seguidores.Add(seguidor);
                await _context.SaveChangesAsync();

                // Cria notificação
                await _notificacaoService.CriarNotificacaoNovoSeguidor(userId, request.UsuarioId);

                return Json(new { success = true, message = "Você está seguindo este usuário" });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = $"Erro: {ex.Message}" });
            }
        }

        // API para deixar de seguir um usuário
        [HttpPost]
        [IgnoreAntiforgeryToken]
        public async Task<IActionResult> DeixarDeSeguir([FromBody] SeguirRequest request)
        {
            var userIdClaim = User.FindFirst("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier");
            if (userIdClaim == null || !int.TryParse(userIdClaim.Value, out var userId))
            {
                return Json(new { success = false, message = "Usuário não autenticado" });
            }

            try
            {
                var seguidor = await _context.Seguidores
                    .FirstOrDefaultAsync(s => s.FKSeguidor == userId && s.FKSeguido == request.UsuarioId);

                if (seguidor == null)
                {
                    return Json(new { success = false, message = "Você não está seguindo este usuário" });
                }

                _context.Seguidores.Remove(seguidor);
                await _context.SaveChangesAsync();

                return Json(new { success = true, message = "Você deixou de seguir este usuário" });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = $"Erro: {ex.Message}" });
            }
        }

        // API para verificar se está seguindo
        [HttpGet]
        public async Task<IActionResult> EstaSeguindo(int usuarioId)
        {
            var userIdClaim = User.FindFirst("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier");
            if (userIdClaim == null || !int.TryParse(userIdClaim.Value, out var userId))
            {
                return Json(new { success = false, seguindo = false });
            }

            try
            {
                var seguindo = await _context.Seguidores
                    .AnyAsync(s => s.FKSeguidor == userId && s.FKSeguido == usuarioId);

                return Json(new { success = true, seguindo });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, seguindo = false, message = $"Erro: {ex.Message}" });
            }
        }

        // API para contar seguidores
        [HttpGet]
        public async Task<IActionResult> ContarSeguidores(int usuarioId)
        {
            try
            {
                var count = await _context.Seguidores
                    .CountAsync(s => s.FKSeguido == usuarioId);

                return Json(new { success = true, count });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, count = 0, message = $"Erro: {ex.Message}" });
            }
        }

        // API para contar quantos o usuário está seguindo
        [HttpGet]
        public async Task<IActionResult> ContarSeguindo(int usuarioId)
        {
            try
            {
                var count = await _context.Seguidores
                    .CountAsync(s => s.FKSeguidor == usuarioId);

                return Json(new { success = true, count });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, count = 0, message = $"Erro: {ex.Message}" });
            }
        }
    }

    public class SeguirRequest
    {
        public int UsuarioId { get; set; }
    }
}
