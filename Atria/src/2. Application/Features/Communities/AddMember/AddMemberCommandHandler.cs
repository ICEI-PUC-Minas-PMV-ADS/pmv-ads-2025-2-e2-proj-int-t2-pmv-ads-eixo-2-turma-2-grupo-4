using System;
using System.Threading;
using System.Threading.Tasks;
using Atria.Application.Common.Interfaces;
using Atria.Domain.Entities.CommunityContext;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Atria.Application.Features.Communities.AddMember;

public class AddMemberCommandHandler : IRequestHandler<AddMemberCommand, Unit>
{
    private readonly IApplicationDbContext _context;

    public AddMemberCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Unit> Handle(AddMemberCommand request, CancellationToken cancellationToken)
    {
        // Validate community exists
        var community = await _context.Comunidades.FirstOrDefaultAsync(c => c.IdComunidade == request.CommunityId, cancellationToken);
        if (community == null) throw new ArgumentException("Community not found", nameof(request.CommunityId));

        // Validate requester is allowed (creator or admin of community or system moderator)
        var requester = await _context.Usuarios.FirstOrDefaultAsync(u => u.IdUsuario == request.RequesterId, cancellationToken);
        if (requester == null) throw new ArgumentException("Requester not found", nameof(request.RequesterId));

        var isSystemModerator = requester.TipoUsuario == Atria.Domain.Enums.TipoUsuario.Moderador;
        var isCreator = community.FkCriador == request.RequesterId;
        var requesterMembership = await _context.ComunidadeMembros.FirstOrDefaultAsync(m => m.UsuarioId == request.RequesterId && m.ComunidadeId == request.CommunityId, cancellationToken);
        var isAdmin = requesterMembership != null && requesterMembership.IsAdmin;

        if (!isSystemModerator && !isCreator && !isAdmin)
            throw new ArgumentException("Requester is not allowed to add members to this community");

        // Validate target user exists
        var user = await _context.Usuarios.FirstOrDefaultAsync(u => u.IdUsuario == request.UserId, cancellationToken);
        if (user == null) throw new ArgumentException("User not found", nameof(request.UserId));

        // Check not already a member
        var existing = await _context.ComunidadeMembros.FirstOrDefaultAsync(m => m.UsuarioId == request.UserId && m.ComunidadeId == request.CommunityId, cancellationToken);
        if (existing != null) throw new ArgumentException("User already member of the community");

        var membership = new ComunidadeMembro
        {
            UsuarioId = request.UserId,
            ComunidadeId = request.CommunityId,
            IsAdmin = request.IsAdmin,
            IsModAdmin = request.IsModAdmin,
            JoinedAt = DateTime.UtcNow
        };

        _context.ComunidadeMembros.Add(membership);
        await _context.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}