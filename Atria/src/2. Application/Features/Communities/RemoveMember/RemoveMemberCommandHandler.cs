using System;
using System.Threading;
using System.Threading.Tasks;
using Atria.Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Atria.Application.Features.Communities.RemoveMember;

public class RemoveMemberCommandHandler : IRequestHandler<RemoveMemberCommand, Unit>
{
    private readonly IApplicationDbContext _context;

    public RemoveMemberCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Unit> Handle(RemoveMemberCommand request, CancellationToken cancellationToken)
    {
        var community = await _context.Comunidades.FirstOrDefaultAsync(c => c.IdComunidade == request.CommunityId, cancellationToken);
        if (community == null) throw new ArgumentException("Community not found", nameof(request.CommunityId));

        var requester = await _context.Usuarios.FirstOrDefaultAsync(u => u.IdUsuario == request.RequesterId, cancellationToken);
        if (requester == null) throw new ArgumentException("Requester not found", nameof(request.RequesterId));

        var isSystemModerator = requester.TipoUsuario == Atria.Domain.Enums.TipoUsuario.Moderador;
        var isCreator = community.FkCriador == request.RequesterId;
        var requesterMembership = await _context.ComunidadeMembros.FirstOrDefaultAsync(m => m.UsuarioId == request.RequesterId && m.ComunidadeId == request.CommunityId, cancellationToken);
        var isAdmin = requesterMembership != null && requesterMembership.IsAdmin;

        if (!isSystemModerator && !isCreator && !isAdmin)
            throw new ArgumentException("Requester is not allowed to remove members from this community");

        var membership = await _context.ComunidadeMembros.FirstOrDefaultAsync(m => m.UsuarioId == request.UserId && m.ComunidadeId == request.CommunityId, cancellationToken);
        if (membership == null) throw new ArgumentException("Target user is not a member of the community");

        _context.ComunidadeMembros.Remove(membership);
        await _context.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}