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
                if (grupoId.HasValue) return RedirectToAction("Details", "GruposEstudo", new { id = grupoId });
                if (destinatarioId.HasValue) return RedirectToAction("Privado", "Chat", new { userId = destinatarioId });
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
                FKGrupo = grupoId,
                FKDestinatario = destinatarioId
            };

            _context.Mensagens.Add(mensagem);
            await _context.SaveChangesAsync();

            // 4. REDIRECIONAMENTO INTELIGENTE (A CORREÇÃO ESTÁ AQUI)

            // Se for mensagem de Grupo -> Volta para o Grupo
            if (grupoId.HasValue)
            {
                return RedirectToAction("Details", "GruposEstudo", new { id = grupoId });
            }

            // Se for Direct -> Volta para o Chat Privado (ESTAVA FALTANDO ISSO)
            if (destinatarioId.HasValue)
            {
                return RedirectToAction("Privado", "Chat", new { userId = destinatarioId });
            }

            return RedirectToAction("Index", "Home");
        }
    }
}