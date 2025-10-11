using System;
using System.Threading;
using System.Threading.Tasks;
using Atria.Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Atria.Application.Features.Material.ApproveMaterial;

public class ApproveMaterialCommandHandler : IRequestHandler<ApproveMaterialCommand, Unit>
{
    private readonly IApplicationDbContext _context;

    public ApproveMaterialCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Unit> Handle(ApproveMaterialCommand request, CancellationToken cancellationToken)
    {
        var material = await _context.Materiais.FirstOrDefaultAsync(m => m.IdMaterial == request.MaterialId, cancellationToken);
        if (material == null) throw new ArgumentException("Material not found", nameof(request.MaterialId));

        var requester = await _context.Usuarios.FirstOrDefaultAsync(u => u.IdUsuario == request.RequesterId, cancellationToken);
        if (requester == null) throw new ArgumentException("Requester not found", nameof(request.RequesterId));

        // System moderator can approve
        var isSystemModerator = requester.TipoUsuario == Atria.Domain.Enums.TipoUsuario.Moderador;
        if (isSystemModerator)
        {
            material.Status = request.Approve ? "Aprovado" : "Rejeitado";
            await _context.SaveChangesAsync(cancellationToken);
            return Unit.Value;
        }

        // If material is linked to a community, allow community ModAdmin or Admin to approve
        if (material.FkComunidade.HasValue)
        {
            var membership = await _context.ComunidadeMembros
                .FirstOrDefaultAsync(cm => cm.ComunidadeId == material.FkComunidade.Value && cm.UsuarioId == requester.IdUsuario, cancellationToken);

            if (membership != null && (membership.IsModAdmin || membership.IsAdmin))
            {
                material.Status = request.Approve ? "Aprovado" : "Rejeitado";
                await _context.SaveChangesAsync(cancellationToken);
                return Unit.Value;
            }
        }

        throw new ArgumentException("Only system moderators or community mod-admins/admins can approve this material");
    }
}