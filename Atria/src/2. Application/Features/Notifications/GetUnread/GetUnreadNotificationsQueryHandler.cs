using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Atria.Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Atria.Application.Features.Notifications.GetNotifications;

namespace Atria.Application.Features.Notifications.GetUnread;

public class GetUnreadNotificationsQueryHandler : IRequestHandler<GetUnreadNotificationsQuery, IEnumerable<NotificationDto>>
{
    private readonly IApplicationDbContext _context;

    public GetUnreadNotificationsQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<NotificationDto>> Handle(GetUnreadNotificationsQuery request, CancellationToken cancellationToken)
    {
        var notifications = await _context.Notificacoes
            .AsNoTracking()
            .Where(n => n.FkUsuario == request.UserId && !n.Lida)
            .OrderByDescending(n => n.DataCriacao)
            .ToListAsync(cancellationToken);

        return notifications.Select(n => new NotificationDto
        {
            Id = n.IdNotificacao,
            Conteudo = n.Conteudo,
            DataCriacao = n.DataCriacao,
            Lida = n.Lida
        }).ToList();
    }
}