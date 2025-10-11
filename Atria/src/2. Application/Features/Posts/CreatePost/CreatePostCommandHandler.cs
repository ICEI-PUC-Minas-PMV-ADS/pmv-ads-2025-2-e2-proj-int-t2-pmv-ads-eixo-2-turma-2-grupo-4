using System;
using System.Threading;
using System.Threading.Tasks;
using Atria.Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Atria.Application.Features.Posts.CreatePost;

public class CreatePostCommandHandler : IRequestHandler<CreatePostCommand, int>
{
    private readonly IApplicationDbContext _context;

    public CreatePostCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<int> Handle(CreatePostCommand request, CancellationToken cancellationToken)
    {
        var requester = await _context.Usuarios.FirstOrDefaultAsync(u => u.IdUsuario == request.RequesterId, cancellationToken);
        if (requester == null) throw new ArgumentException("Requester not found");

        // If posting to a community
        if (request.CommunityId.HasValue)
        {
            var community = await _context.Comunidades.FirstOrDefaultAsync(c => c.IdComunidade == request.CommunityId.Value, cancellationToken);
            if (community == null) throw new ArgumentException("Community not found", nameof(request.CommunityId));

            // requester must be a member of the community or the creator
            var isCreator = community.FkCriador == requester.IdUsuario;
            var membership = await _context.ComunidadeMembros.FirstOrDefaultAsync(m => m.ComunidadeId == request.CommunityId.Value && m.UsuarioId == requester.IdUsuario, cancellationToken);
            if (membership == null && !isCreator) throw new ArgumentException("Requester must be a community member to post here");
        }
        else
        {
            // Posting to forum geral => NoForumGeral must be true
            if (!request.NoForumGeral) throw new ArgumentException("To post without community, NoForumGeral must be true");
        }

        var post = new Atria.Domain.Entities.CommunityContext.Postagem
        {
            Titulo = request.Title,
            Conteudo = request.Content,
            FkAutor = requester.IdUsuario,
            FkComunidade = request.CommunityId,
            NoForumGeral = request.NoForumGeral
        };

        _context.Postagens.Add(post);
        await _context.SaveChangesAsync(cancellationToken);

        return post.IdPostagem;
    }
}