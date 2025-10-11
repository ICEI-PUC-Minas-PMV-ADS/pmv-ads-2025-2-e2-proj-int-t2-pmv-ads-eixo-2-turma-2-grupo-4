using System;
using System.Threading;
using System.Threading.Tasks;
using Atria.Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Atria.Application.Features.Communities.AcceptInvite;

public class AcceptInviteCommandHandler : IRequestHandler<AcceptInviteCommand, Unit>
{
    private readonly IApplicationDbContext _context;

    public AcceptInviteCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Unit> Handle(AcceptInviteCommand request, CancellationToken cancellationToken)
    {
        var membership = await _context.ComunidadeMembros.FirstOrDefaultAsync(cm => cm.ComunidadeId == request.CommunityId && cm.UsuarioId == request.UserId, cancellationToken);
        if (membership == null) throw new ArgumentException("No invitation found");
        if (!membership.IsPending) throw new ArgumentException("Membership is not pending");

        membership.IsPending = false;
        membership.JoinedAt = DateTime.UtcNow;

        // Notify inviter
        if (!string.IsNullOrEmpty(membership.InvitedBy))
        {
            var inviter = await _context.Usuarios.FirstOrDefaultAsync(u => u.IdUsuario == membership.InvitedBy, cancellationToken);
            if (inviter != null)
            {
                var notif = new Atria.Domain.Entities.InteractionContext.Notificacao
                {
                    Conteudo = $"{membership.UsuarioId} aceitou seu convite para a comunidade {membership.ComunidadeId}",
                    FkUsuario = inviter.IdUsuario,
                    Usuario = inviter,
                    DataCriacao = DateTime.UtcNow
                };

                _context.Notificacoes.Add(notif);
            }
        }

        await _context.SaveChangesAsync(cancellationToken);
        return Unit.Value;
    }
}