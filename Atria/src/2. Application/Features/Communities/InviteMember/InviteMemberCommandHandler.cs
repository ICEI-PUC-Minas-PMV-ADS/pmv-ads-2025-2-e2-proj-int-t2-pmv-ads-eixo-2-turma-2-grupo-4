using System;
using System.Threading;
using System.Threading.Tasks;
using Atria.Application.Common.Interfaces;
using Atria.Domain.Entities.CommunityContext;
using Atria.Domain.Entities.InteractionContext;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Atria.Application.Features.Communities.InviteMember;

public class InviteMemberCommandHandler : IRequestHandler<InviteMemberCommand, Unit>
{
    private readonly IApplicationDbContext _context;

    public InviteMemberCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Unit> Handle(InviteMemberCommand request, CancellationToken cancellationToken)
    {
        var community = await _context.Comunidades.FirstOrDefaultAsync(c => c.IdComunidade == request.CommunityId, cancellationToken);
        if (community == null) throw new ArgumentException("Community not found", nameof(request.CommunityId));

        var requester = await _context.Usuarios.FirstOrDefaultAsync(u => u.IdUsuario == request.RequesterId, cancellationToken);
        if (requester == null) throw new ArgumentException("Requester not found", nameof(request.RequesterId));

        // Only creator, community admin/modadmin or system moderator can invite
        var isSystemModerator = requester.TipoUsuario == Atria.Domain.Enums.TipoUsuario.Moderador;
        var isCreator = community.FkCriador == requester.IdUsuario;
        var requesterMembership = await _context.ComunidadeMembros.FirstOrDefaultAsync(cm => cm.UsuarioId == request.RequesterId && cm.ComunidadeId == request.CommunityId, cancellationToken);
        var isAdmin = requesterMembership != null && requesterMembership.IsAdmin;

        if (!isSystemModerator && !isCreator && !isAdmin)
            throw new ArgumentException("Requester not allowed to invite users");

        // Ensure target user exists
        var targetUser = await _context.Usuarios.FirstOrDefaultAsync(u => u.IdUsuario == request.TargetUserId, cancellationToken);
        if (targetUser == null) throw new ArgumentException("Target user not found", nameof(request.TargetUserId));

        // If already a membership, reject
        var existing = await _context.ComunidadeMembros.FirstOrDefaultAsync(cm => cm.UsuarioId == request.TargetUserId && cm.ComunidadeId == request.CommunityId, cancellationToken);
        if (existing != null) throw new ArgumentException("User already member or invited");

        var membership = new ComunidadeMembro
        {
            UsuarioId = request.TargetUserId,
            ComunidadeId = request.CommunityId,
            IsAdmin = false,
            IsModAdmin = false,
            IsPending = true,
            InvitedBy = request.RequesterId,
            JoinedAt = DateTime.UtcNow
        };

        _context.ComunidadeMembros.Add(membership);

        // Create notification for invited user
        var notification = new Notificacao
        {
            Conteudo = $"Você foi convidado para a comunidade {community.Nome}",
            FkUsuario = request.TargetUserId,
            Usuario = targetUser,
            DataCriacao = DateTime.UtcNow
        };

        _context.Notificacoes.Add(notification);

        await _context.SaveChangesAsync(cancellationToken);
        return Unit.Value;
    }
}