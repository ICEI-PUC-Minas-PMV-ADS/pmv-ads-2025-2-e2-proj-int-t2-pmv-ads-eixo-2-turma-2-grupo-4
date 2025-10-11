using System;
using System.Threading;
using System.Threading.Tasks;
using Atria.Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Atria.Application.Features.Notifications.MarkAsRead;

public class MarkNotificationAsReadCommandHandler : IRequestHandler<MarkNotificationAsReadCommand, Unit>
{
    private readonly IApplicationDbContext _context;

    public MarkNotificationAsReadCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Unit> Handle(MarkNotificationAsReadCommand request, CancellationToken cancellationToken)
    {
        var notif = await _context.Notificacoes.FirstOrDefaultAsync(n => n.IdNotificacao == request.NotificationId, cancellationToken);
        if (notif == null) throw new ArgumentException("Notification not found", nameof(request.NotificationId));
        if (notif.FkUsuario != request.RequesterId) throw new ArgumentException("Not authorized to mark this notification");

        notif.Lida = true;
        await _context.SaveChangesAsync(cancellationToken);
        return Unit.Value;
    }
}