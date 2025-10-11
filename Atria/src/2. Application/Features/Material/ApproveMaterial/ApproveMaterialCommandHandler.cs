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

        var isSystemModerator = requester.TipoUsuario == Atria.Domain.Enums.TipoUsuario.Moderador;
        if (!isSystemModerator)
        {
            // check if requester is modadmin of the professor's community or something similar (skip advanced logic for now)
            throw new ArgumentException("Only system moderators can approve materials currently");
        }

        material.Status = request.Approve ? "Aprovado" : "Rejeitado";
        await _context.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}