using Atria.Data;
using Atria.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Atria.Controllers
{
    [Authorize]
    public class MensagensController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public MensagensController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Enviar(string conteudo, int? grupoId, int? destinatarioId)
        {
            // 1. Validação Básica
            if (string.IsNullOrWhiteSpace(conteudo))
            {
                // Se vazio, volta para de onde veio
                if (grupoId.HasValue) return RedirectToAction("Details", "GruposEstudo", new { id = grupoId });
                // if (destinatarioId.HasValue) return RedirectToAction("Chat", "Direct", new { userId = destinatarioId });
                return RedirectToAction("Index", "Home");
            }

            // 2. Pegar usuário logado
            var user = await _userManager.GetUserAsync(User);
            if (user == null) return Challenge();

            // 3. Criar a mensagem
            var mensagem = new Mensagem
            {
                Conteudo = conteudo,
                DataEnvio = DateTime.UtcNow,
                FKRemetente = user.Id,
                FKGrupo = grupoId,          // Preenchido se for Grupo
                FKDestinatario = destinatarioId // Preenchido se for Direct
            };

            _context.Mensagens.Add(mensagem);
            await _context.SaveChangesAsync();

            // 4. Redirecionar de volta
            if (grupoId.HasValue)
            {
                return RedirectToAction("Details", "GruposEstudo", new { id = grupoId });
            }

            // Futuramente você adicionará o redirecionamento do Direct aqui
            return RedirectToAction("Index", "Home");
        }
    }
}