using System;
using System.Threading;
using System.Threading.Tasks;
using Atria.Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Atria.Application.Features.Messages.MarkAsRead;

public class MarkAsReadCommandHandler : IRequestHandler<MarkAsReadCommand, Unit>
{
    private readonly IApplicationDbContext _context;

    public MarkAsReadCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Unit> Handle(MarkAsReadCommand request, CancellationToken cancellationToken)
    {
        var message = await _context.MensagensPrivadas.FirstOrDefaultAsync(m => m.IdMensagem == request.MessageId, cancellationToken);
        if (message == null) throw new ArgumentException("Message not found", nameof(request.MessageId));

        if (message.FkDestinatario != request.RequesterId) throw new ArgumentException("Only recipient can mark as read");

        message.Lida = true;
        await _context.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}