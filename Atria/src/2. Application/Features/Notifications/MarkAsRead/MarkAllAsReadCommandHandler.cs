using System.Threading;
using System.Threading.Tasks;
using Atria.Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Atria.Application.Features.Notifications.MarkAsRead;

public class MarkAllAsReadCommandHandler : IRequestHandler<MarkAllAsReadCommand, Unit>
{
    private readonly IApplicationDbContext _context;

    public MarkAllAsReadCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Unit> Handle(MarkAllAsReadCommand request, CancellationToken cancellationToken)
    {
        var items = await _context.Notificacoes.Where(n => n.FkUsuario == request.UserId && !n.Lida).ToListAsync(cancellationToken);
        foreach (var n in items)
        {
            n.Lida = true;
        }

        await _context.SaveChangesAsync(cancellationToken);
        return Unit.Value;
    }
}