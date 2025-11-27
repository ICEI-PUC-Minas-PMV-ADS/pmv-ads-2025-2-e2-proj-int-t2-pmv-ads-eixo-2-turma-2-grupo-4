using Atria.Data;
using Atria.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Atria.Controllers
{
    [Authorize]
    public class CurtidaPostagemController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CurtidaPostagemController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpPost]
        [IgnoreAntiforgeryToken]
        public async Task<IActionResult> Toggle([FromBody] CurtidaPostagemRequest request)
        {
            var userIdClaim = User.FindFirst("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier");
            if (userIdClaim == null || !int.TryParse(userIdClaim.Value, out var userId))
            {
                return Json(new { success = false, message = "Usuário não autenticado" });
            }

            try
            {
                // Verifica se o usuário já curtiu esta postagem
                var curtidaExistente = await _context.CurtidasPostagem
                    .FirstOrDefaultAsync(c => c.FKPostagem == request.PostagemId && c.FKUsuario == userId);

                if (curtidaExistente != null)
                {
                    // Remove a curtida (descurtir)
                    _context.CurtidasPostagem.Remove(curtidaExistente);
                    await _context.SaveChangesAsync();

                    // Conta total de curtidas
                    var totalCurtidas = await _context.CurtidasPostagem
                        .CountAsync(c => c.FKPostagem == request.PostagemId);

                    return Json(new { success = true, curtido = false, totalCurtidas });
                }
                else
                {
                    // Adiciona nova curtida
                    var novaCurtida = new CurtidaPostagem
                    {
                        FKPostagem = request.PostagemId,
                        FKUsuario = userId,
                        DataCurtida = DateTime.UtcNow,
                        Tipo = "LIKE"
                    };

                    _context.CurtidasPostagem.Add(novaCurtida);
                    await _context.SaveChangesAsync();

                    // Criar notificação para o autor da postagem
                    var postagem = await _context.Postagens
                        .Include(p => p.Usuario)
                        .FirstOrDefaultAsync(p => p.Id == request.PostagemId);

                    if (postagem != null && postagem.FKUsuario != userId)
                    {
                        var usuario = await _context.Users.FindAsync(userId);
                        var notificacao = new Notificacao
                        {
                            FKUsuario = postagem.FKUsuario,
                            Titulo = "Nova curtida",
                            Mensagem = $"{usuario?.Nome ?? "Alguém"} curtiu sua postagem \"{postagem.Titulo.Substring(0, Math.Min(50, postagem.Titulo.Length))}...\"",
                            Tipo = "CURTIDA",
                            Link = $"/Postagens/Details/{postagem.Id}",
                            Icone = "bi-heart-fill",
                            Cor = "text-danger",
                            Lida = false,
                            DataCriacao = DateTime.UtcNow
                        };

                        _context.Notificacoes.Add(notificacao);
                        await _context.SaveChangesAsync();
                    }

                    // Conta total de curtidas
                    var totalCurtidas = await _context.CurtidasPostagem
                        .CountAsync(c => c.FKPostagem == request.PostagemId);

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
        public async Task<IActionResult> GetStatus(int postagemId)
        {
            var userIdClaim = User.FindFirst("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier");
            var userId = userIdClaim != null && int.TryParse(userIdClaim.Value, out var id) ? id : 0;

            var totalCurtidas = await _context.CurtidasPostagem
                .CountAsync(c => c.FKPostagem == postagemId);

            var curtido = userId > 0 && await _context.CurtidasPostagem
                .AnyAsync(c => c.FKPostagem == postagemId && c.FKUsuario == userId);

            return Json(new { totalCurtidas, curtido });
        }
    }

    // Classe helper para receber JSON
    public class CurtidaPostagemRequest
    {
        public int PostagemId { get; set; }
    }
}
