using System;
using System.Threading;
using System.Threading.Tasks;
using Atria.Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Atria.Application.Features.Comments.CreateComment;

public class CreateCommentCommandHandler : IRequestHandler<CreateCommentCommand, int>
{
    private readonly IApplicationDbContext _context;

    public CreateCommentCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<int> Handle(CreateCommentCommand request, CancellationToken cancellationToken)
    {
        var requester = await _context.Usuarios.FirstOrDefaultAsync(u => u.IdUsuario == request.RequesterId, cancellationToken);
        if (requester == null) throw new ArgumentException("Requester not found");

        var post = await _context.Postagens.FirstOrDefaultAsync(p => p.IdPostagem == request.PostId, cancellationToken);
        if (post == null) throw new ArgumentException("Post not found", nameof(request.PostId));

        // If post is tied to a community, commenter must be member or post is in forum geral
        if (post.FkComunidade.HasValue)
        {
            var membership = await _context.ComunidadeMembros.FirstOrDefaultAsync(m => m.ComunidadeId == post.FkComunidade.Value && m.UsuarioId == requester.IdUsuario, cancellationToken);
            if (membership == null) throw new ArgumentException("Requester must be a community member to comment on this post");
        }
        else
        {
            if (!post.NoForumGeral) throw new ArgumentException("Post not in forum geral and requester not allowed");
        }

        var comment = new Atria.Domain.Entities.CommunityContext.Comentario
        {
            Conteudo = request.Content,
            FkAutor = requester.IdUsuario,
            FkPostagem = request.PostId
        };

        _context.Comentarios.Add(comment);
        await _context.SaveChangesAsync(cancellationToken);

        return comment.IdComentario;
    }
}