using Atria.Data;
using Atria.Models;
using Microsoft.EntityFrameworkCore;

namespace Atria.Services
{
    public interface INotificacaoService
    {
        Task CriarNotificacaoMensagemPrivada(int remetenteId, int destinatarioId, int mensagemId);
        Task CriarNotificacaoMaterialAvaliado(int materialId, string novoStatus);
        Task CriarNotificacaoNovoSeguidor(int seguidorId, int seguidoId);
    }

    public class NotificacaoService : INotificacaoService
    {
        private readonly ApplicationDbContext _context;

        public NotificacaoService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task CriarNotificacaoMensagemPrivada(int remetenteId, int destinatarioId, int mensagemId)
        {
            try
            {
                var remetente = await _context.Users.FindAsync(remetenteId);
                if (remetente == null) return;

                var notificacao = new Notificacao
                {
                    FKUsuario = destinatarioId,
                    Titulo = "Nova Mensagem",
                    Mensagem = $"{remetente.Nome} enviou uma mensagem para você",
                    Tipo = "MENSAGEM",
                    Link = $"/Chat/Privado?userId={remetenteId}",
                    Icone = "bi-chat-dots-fill",
                    Cor = "text-info",
                    Lida = false,
                    DataCriacao = DateTime.UtcNow
                };

                _context.Notificacoes.Add(notificacao);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                // Log do erro (você pode usar ILogger aqui)
                Console.WriteLine($"Erro ao criar notificação de mensagem: {ex.Message}");
            }
        }

        public async Task CriarNotificacaoMaterialAvaliado(int materialId, string novoStatus)
        {
            try
            {
                var material = await _context.Materiais
                    .Include(m => m.Criador)
                    .FirstOrDefaultAsync(m => m.Id == materialId);

                if (material == null || material.Criador == null) return;

                string titulo = "";
                string mensagem = "";
                string icone = "";
                string cor = "";

                if (novoStatus == "Aprovado")
                {
                    titulo = "Material Aprovado!";
                    mensagem = $"Seu material '{material.Titulo}' foi aprovado e está disponível na plataforma";
                    icone = "bi-check-circle-fill";
                    cor = "text-success";
                }
                else if (novoStatus == "Rejeitado")
                {
                    titulo = "Material Rejeitado";
                    mensagem = $"Seu material '{material.Titulo}' não foi aprovado. Verifique os critérios da plataforma";
                    icone = "bi-x-circle-fill";
                    cor = "text-danger";
                }
                else
                {
                    return; // Não notifica para outros status
                }

                var notificacao = new Notificacao
                {
                    FKUsuario = material.FKUsuarioCriador,
                    Titulo = titulo,
                    Mensagem = mensagem,
                    Tipo = "MATERIAL_AVALIADO",
                    Link = $"/Materiais/Details/{materialId}",
                    Icone = icone,
                    Cor = cor,
                    Lida = false,
                    DataCriacao = DateTime.UtcNow
                };

                _context.Notificacoes.Add(notificacao);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao criar notificação de material: {ex.Message}");
            }
        }

        public async Task CriarNotificacaoNovoSeguidor(int seguidorId, int seguidoId)
        {
            try
            {
                var seguidor = await _context.Users.FindAsync(seguidorId);
                if (seguidor == null) return;

                var notificacao = new Notificacao
                {
                    FKUsuario = seguidoId,
                    Titulo = "Novo Seguidor",
                    Mensagem = $"{seguidor.Nome} começou a seguir você",
                    Tipo = "SEGUIDOR",
                    Link = $"/Profile/Index/{seguidorId}",
                    Icone = "bi-person-plus-fill",
                    Cor = "text-primary",
                    Lida = false,
                    DataCriacao = DateTime.UtcNow
                };

                _context.Notificacoes.Add(notificacao);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao criar notificação de seguidor: {ex.Message}");
            }
        }
    }
}
