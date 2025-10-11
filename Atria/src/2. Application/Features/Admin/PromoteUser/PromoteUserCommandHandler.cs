using System;
using System.Threading;
using System.Threading.Tasks;
using Atria.Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Atria.Application.Features.Admin.PromoteUser;

public class PromoteUserCommandHandler : IRequestHandler<PromoteUserCommand, Unit>
{
    private readonly IApplicationDbContext _context;

    public PromoteUserCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Unit> Handle(PromoteUserCommand request, CancellationToken cancellationToken)
    {
        // Validate requester has rights: must be moderator (system) or admin of the community
        var requester = await _context.Usuarios.FirstOrDefaultAsync(u => u.IdUsuario == request.RequesterId, cancellationToken);
        if (requester == null) throw new ArgumentException("Requester not found", nameof(request.RequesterId));

        var isSystemModerator = requester.TipoUsuario == Atria.Domain.Enums.TipoUsuario.Moderador;

        if (!isSystemModerator && request.MakeAdminInCommunity && request.CommunityId.HasValue)
        {
            // check requester is admin in that community
            var membership = await _context.ComunidadeMembros.FirstOrDefaultAsync(cm => cm.UsuarioId == request.RequesterId && cm.ComunidadeId == request.CommunityId.Value, cancellationToken);
            if (membership == null || !membership.IsAdmin)
            {
                throw new ArgumentException("Requester is not admin of the specified community");
            }
        }

        if (request.MakeAdminInCommunity && request.CommunityId.HasValue)
        {
            var targetMembership = await _context.ComunidadeMembros.FirstOrDefaultAsync(cm => cm.UsuarioId == request.TargetUserId && cm.ComunidadeId == request.CommunityId.Value, cancellationToken);
            if (targetMembership == null) throw new ArgumentException("Target user is not a member of the community");

            targetMembership.IsAdmin = request.MakeAdminInCommunity;
            if (request.MakeModAdmin)
                targetMembership.IsModAdmin = true;

            await _context.SaveChangesAsync(cancellationToken);
            return Unit.Value;
        }

        throw new ArgumentException("Invalid promote request");
    }
}