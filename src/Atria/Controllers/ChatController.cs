using Atria.Data;
using Atria.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Atria.Controllers
{
    [Authorize]
    public class ChatController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public ChatController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: Chat/Privado?userId=5
        [HttpGet]
        public async Task<IActionResult> Privado(int userId)
        {
            // 1. Identifica quem sou eu
            var meuUser = await _userManager.GetUserAsync(User);
            if (meuUser == null) return Challenge();

            // 2. Busca o usuário com quem quero falar
            var outroUser = await _userManager.Users.FirstOrDefaultAsync(u => u.Id == userId);
            if (outroUser == null) return NotFound();

            // 3. Se tentar falar consigo mesmo, redireciona para o perfil
            if (meuUser.Id == userId) return RedirectToAction("Index", "Profile");

            // 4. Busca as mensagens (A conversa completa entre Nós dois)
            // Lógica: (Eu mandei p/ Ele) OU (Ele mandou p/ Mim)
            var mensagens = await _context.Mensagens
                .Where(m => (m.FKRemetente == meuUser.Id && m.FKDestinatario == userId) ||
                            (m.FKRemetente == userId && m.FKDestinatario == meuUser.Id))
                .Include(m => m.Remetente) // Precisamos saber quem mandou cada uma
                .OrderBy(m => m.DataEnvio)
                .ToListAsync();

            // 5. Monta o pacote para a tela
            var model = new ChatPrivadoViewModel
            {
                UsuarioDestino = outroUser,
                Mensagens = mensagens
            };

            return View(model);
        }

        // GET: Chat/Index (Minhas Conversas)
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            // CORREÇÃO 1: Verifica se o ID é nulo antes de converter
            var userIdString = _userManager.GetUserId(User);
            if (string.IsNullOrEmpty(userIdString)) return Challenge();

            var userId = int.Parse(userIdString);

            var mensagens = await _context.Mensagens
                .Where(m => (m.FKRemetente == userId || m.FKDestinatario == userId) && m.FKGrupo == null)
                .Include(m => m.Remetente)
                .Include(m => m.Destinatario)
                .OrderByDescending(m => m.DataEnvio)
                .ToListAsync();

            var conversas = mensagens
                .GroupBy(m => m.FKRemetente == userId ? m.FKDestinatario : m.FKRemetente)
                .Select(g => {
                    var ultimaMsg = g.First();
                    var outroUsuario = ultimaMsg.FKRemetente == userId ? ultimaMsg.Destinatario : ultimaMsg.Remetente;

                    return new InboxItemViewModel
                    {
                        // CORREÇÃO 2: Aceita que pode ser nulo ou usa null-forgiving (!) se tiver certeza
                        Usuario = outroUsuario,
                        UltimaMensagem = ultimaMsg.Conteudo,
                        DataUltimaMensagem = ultimaMsg.DataEnvio
                    };
                })
                .ToList();

            return View(conversas);
        }
    }
}