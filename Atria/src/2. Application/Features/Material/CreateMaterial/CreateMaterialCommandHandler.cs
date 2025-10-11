using System;
using System.Threading;
using System.Threading.Tasks;
using Atria.Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Atria.Application.Features.Material.CreateMaterial;

public class CreateMaterialCommandHandler : IRequestHandler<CreateMaterialCommand, int>
{
    private readonly IApplicationDbContext _context;

    public CreateMaterialCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<int> Handle(CreateMaterialCommand request, CancellationToken cancellationToken)
    {
        // Only professors can create materials
        var requester = await _context.Usuarios.FirstOrDefaultAsync(u => u.IdUsuario == request.RequesterId, cancellationToken);
        if (requester == null) throw new ArgumentException("Requester not found");
        if (requester.TipoUsuario != Atria.Domain.Enums.TipoUsuario.Professor)
            throw new ArgumentException("Only professors can create materials");

        // If CommunityId provided, validate community and membership/creator
        if (request.CommunityId.HasValue)
        {
            var community = await _context.Comunidades.FirstOrDefaultAsync(c => c.IdComunidade == request.CommunityId.Value, cancellationToken);
            if (community == null) throw new ArgumentException("Community not found", nameof(request.CommunityId));

            // Check membership or creator
            var isCreator = community.FkCriador == requester.IdUsuario;
            var membership = await _context.ComunidadeMembros
                .FirstOrDefaultAsync(cm => cm.ComunidadeId == request.CommunityId.Value && cm.UsuarioId == requester.IdUsuario, cancellationToken);

            var isMember = membership != null;
            if (!isCreator && !isMember)
                throw new ArgumentException("Requester must be a member or the creator of the community to add material to it");
        }

        global::Atria.Domain.Entities.MaterialContext.Material material;
        if (string.Equals(request.TipoMaterial, "Livro", StringComparison.OrdinalIgnoreCase))
        {
            material = new global::Atria.Domain.Entities.MaterialContext.Livro
            {
                Titulo = request.Title,
                Autor = request.Author,
                AnoPublicacao = request.Year,
                Status = request.Status,
                FkProfessorCadastro = requester.IdUsuario,
                ISBN = request.ISBN ?? string.Empty,
                Editora = request.Editora ?? string.Empty,
                FkComunidade = request.CommunityId
            };
        }
        else
        {
            material = new global::Atria.Domain.Entities.MaterialContext.Artigo
            {
                Titulo = request.Title,
                Autor = request.Author,
                AnoPublicacao = request.Year,
                Status = request.Status,
                FkProfessorCadastro = requester.IdUsuario,
                DOI = request.DOI ?? string.Empty,
                FkComunidade = request.CommunityId
            };
        }

        _context.Materiais.Add(material);
        await _context.SaveChangesAsync(cancellationToken);

        return material.IdMaterial;
    }
}