using System;
using System.Threading;
using System.Threading.Tasks;
using Atria.Application.Common.Interfaces;
using Atria.Domain.Entities.InteractionContext;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Atria.Application.Features.Messages.SendMessage;

public class SendMessageCommandHandler : IRequestHandler<SendMessageCommand, string>
{
    private readonly IApplicationDbContext _context;
    private readonly ILogger<SendMessageCommandHandler> _logger;

    public SendMessageCommandHandler(IApplicationDbContext context, ILogger<SendMessageCommandHandler> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<string> Handle(SendMessageCommand request, CancellationToken cancellationToken)
    {
        // Basic validations
        if (string.IsNullOrWhiteSpace(request.SenderId)) throw new ArgumentException("SenderId is required");
        if (string.IsNullOrWhiteSpace(request.RecipientId)) throw new ArgumentException("RecipientId is required");
        if (string.IsNullOrWhiteSpace(request.Content)) throw new ArgumentException("Content is required");

        // Check users exist
        var sender = await _context.Usuarios.FindAsync(new object[] { request.SenderId }, cancellationToken);
        var recipient = await _context.Usuarios.FindAsync(new object[] { request.RecipientId }, cancellationToken);

        if (sender == null) throw new ArgumentException("Sender not found", nameof(request.SenderId));
        if (recipient == null) throw new ArgumentException("Recipient not found", nameof(request.RecipientId));

        var mensagem = new MensagemPrivada
        {
            IdMensagem = Guid.NewGuid().ToString(),
            Conteudo = request.Content,
            DataEnvio = DateTime.UtcNow,
            Lida = false,
            FkRemetente = sender.IdUsuario,
            FkDestinatario = recipient.IdUsuario,
            Remetente = sender,
            Destinatario = recipient
        };

        _context.MensagensPrivadas.Add(mensagem);
        await _context.SaveChangesAsync(cancellationToken);

        _logger.LogInformation("Mensagem {MessageId} enviada de {Sender} para {Recipient}", mensagem.IdMensagem, sender.IdUsuario, recipient.IdUsuario);

        return mensagem.IdMensagem;
    }
}