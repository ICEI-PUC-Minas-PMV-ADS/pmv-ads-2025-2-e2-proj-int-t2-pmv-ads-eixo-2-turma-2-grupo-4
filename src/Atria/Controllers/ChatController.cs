using Atria.Data;
using Atria.Models;
using Atria.Services;
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
        private readonly INotificacaoService _notificacaoService;

        public ChatController(ApplicationDbContext context, UserManager<ApplicationUser> userManager, INotificacaoService notificacaoService)
        {
            _context = context;
            _userManager = userManager;
            _notificacaoService = notificacaoService;
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
            // Verifica se o ID é nulo antes de converter
            var userIdString = _userManager.GetUserId(User);
            if (string.IsNullOrEmpty(userIdString)) return Challenge();

            var userId = int.Parse(userIdString);

            // Buscar apenas mensagens privadas (onde FKDestinatario não é null e FKGrupo é null)
            var mensagens = await _context.Mensagens
                .Where(m => (m.FKRemetente == userId || m.FKDestinatario == userId) 
                       && m.FKGrupo == null 
                       && m.FKDestinatario != null)
                .Include(m => m.Remetente)
                .Include(m => m.Destinatario)
                .OrderByDescending(m => m.DataEnvio)
                .ToListAsync();

            var conversas = mensagens
                .GroupBy(m => m.FKRemetente == userId ? m.FKDestinatario : m.FKRemetente)
                .Where(g => g.Key.HasValue) // Garantir que o outro usuário existe
                .Select(g => {
                    var ultimaMsg = g.First();
                    var outroUsuario = ultimaMsg.FKRemetente == userId ? ultimaMsg.Destinatario : ultimaMsg.Remetente;

                    return new InboxItemViewModel
                    {
                        Usuario = outroUsuario,
                        UltimaMensagem = ultimaMsg.Conteudo,
                        DataUltimaMensagem = ultimaMsg.DataEnvio
                    };
                })
                .Where(c => c.Usuario != null) // Filtrar conversas sem usuário
                .OrderByDescending(c => c.DataUltimaMensagem)
                .ToList();

            return View(conversas);
        }

        // POST: Chat/EnviarMensagem
        [HttpPost]
        [IgnoreAntiforgeryToken]
        public async Task<IActionResult> EnviarMensagem([FromBody] EnviarMensagemRequest request)
        {
            var meuUser = await _userManager.GetUserAsync(User);
            if (meuUser == null)
            {
                return Json(new { success = false, message = "Usuário não autenticado" });
            }

            if (string.IsNullOrWhiteSpace(request.Conteudo))
            {
                return Json(new { success = false, message = "Mensagem vazia" });
            }

            try
            {
                var mensagem = new Mensagem
                {
                    Conteudo = request.Conteudo.Trim(),
                    FKRemetente = meuUser.Id,
                    FKDestinatario = request.UsuarioId,
                    DataEnvio = DateTime.UtcNow
                };

                _context.Mensagens.Add(mensagem);
                await _context.SaveChangesAsync();

                // Criar notificação para o destinatário
                await _notificacaoService.CriarNotificacaoMensagemPrivada(meuUser.Id, request.UsuarioId, mensagem.Id);

                return Json(new { success = true, mensagem = new {
                    mensagem.Id,
                    mensagem.Conteudo,
                    mensagem.DataEnvio,
                    Remetente = new { meuUser.Id, meuUser.Nome }
                }});
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = $"Erro ao enviar mensagem: {ex.Message}" });
            }
        }
    }

    public class EnviarMensagemRequest
    {
        public int UsuarioId { get; set; }
        public string Conteudo { get; set; } = string.Empty;
    }
}