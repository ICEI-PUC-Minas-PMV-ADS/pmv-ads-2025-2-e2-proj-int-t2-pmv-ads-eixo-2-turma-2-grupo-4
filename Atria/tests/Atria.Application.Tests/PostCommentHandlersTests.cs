using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Atria.Application.Features.Posts.CreatePost;
using Atria.Application.Features.Comments.CreateComment;
using Atria.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Xunit;
using Atria.Domain.Entities.UserContext;
using Atria.Domain.Entities.CommunityContext;
using Atria.Domain.Enums;

namespace Atria.Application.Tests
{
    public class PostCommentHandlersTests : IDisposable
    {
        private readonly AppDbContext _context;

        public PostCommentHandlersTests()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            _context = new AppDbContext(options);

            // Seed users
            var userMember = new Usuario
            {
                IdUsuario = "member-1",
                Nome = "Member",
                Email = "member@example.com",
                SenhaHash = "hash",
                TipoUsuario = TipoUsuario.Comum,
                DataCadastro = DateTime.UtcNow
            };

            var userNonMember = new Usuario
            {
                IdUsuario = "nonmember-1",
                Nome = "NonMember",
                Email = "nonmember@example.com",
                SenhaHash = "hash",
                TipoUsuario = TipoUsuario.Comum,
                DataCadastro = DateTime.UtcNow
            };

            var professor = new Usuario
            {
                IdUsuario = "prof-1",
                Nome = "Prof",
                Email = "prof@example.com",
                SenhaHash = "hash",
                TipoUsuario = TipoUsuario.Professor,
                DataCadastro = DateTime.UtcNow
            };

            _context.Usuarios.AddRange(userMember, userNonMember, professor);
            _context.SaveChanges();

            // Seed community and membership
            var community = new Comunidade
            {
                IdComunidade = 100,
                Nome = "CommTest",
                Descricao = "Desc",
                FkCriador = professor.IdUsuario,
                DataCriacao = DateTime.UtcNow
            };
            _context.Comunidades.Add(community);
            _context.SaveChanges();

            var membership = new ComunidadeMembro
            {
                UsuarioId = userMember.IdUsuario,
                ComunidadeId = community.IdComunidade,
                IsAdmin = false,
                IsModAdmin = false,
                IsPending = false,
                JoinedAt = DateTime.UtcNow
            };
            _context.ComunidadeMembros.Add(membership);
            _context.SaveChanges();
        }

        [Fact]
        public async Task Member_Can_Create_Post_In_Community()
        {
            var handler = new CreatePostCommandHandler(_context);
            var cmd = new CreatePostCommand
            {
                Title = "Post 1",
                Content = "Content",
                CommunityId = 100,
                NoForumGeral = false,
                RequesterId = "member-1"
            };

            var id = await handler.Handle(cmd, CancellationToken.None);
            Assert.True(id > 0);

            var post = _context.Postagens.Find(id);
            Assert.NotNull(post);
            Assert.Equal(100, post.FkComunidade);
        }

        [Fact]
        public async Task NonMember_Cannot_Create_Post_In_Community()
        {
            var handler = new CreatePostCommandHandler(_context);
            var cmd = new CreatePostCommand
            {
                Title = "Post 2",
                Content = "Content",
                CommunityId = 100,
                NoForumGeral = false,
                RequesterId = "nonmember-1"
            };

            await Assert.ThrowsAsync<ArgumentException>(async () => await handler.Handle(cmd, CancellationToken.None));
        }

        [Fact]
        public async Task Can_Create_Post_In_ForumGeral_When_NoForumGeral_True()
        {
            var handler = new CreatePostCommandHandler(_context);
            var cmd = new CreatePostCommand
            {
                Title = "Post 3",
                Content = "Content",
                CommunityId = null,
                NoForumGeral = true,
                RequesterId = "nonmember-1"
            };

            var id = await handler.Handle(cmd, CancellationToken.None);
            Assert.True(id > 0);

            var post = _context.Postagens.Find(id);
            Assert.NotNull(post);
            Assert.Null(post.FkComunidade);
            Assert.True(post.NoForumGeral);
        }

        [Fact]
        public async Task Member_Can_Comment_On_Community_Post()
        {
            // create a post in community by professor
            var postHandler = new CreatePostCommandHandler(_context);
            var createCmd = new CreatePostCommand
            {
                Title = "Comm Post",
                Content = "c",
                CommunityId = 100,
                NoForumGeral = false,
                RequesterId = "prof-1"
            };
            var postId = await postHandler.Handle(createCmd, CancellationToken.None);

            var commentHandler = new CreateCommentCommandHandler(_context);
            var commentCmd = new CreateCommentCommand
            {
                PostId = postId,
                Content = "Nice",
                RequesterId = "member-1"
            };

            var commentId = await commentHandler.Handle(commentCmd, CancellationToken.None);
            Assert.True(commentId > 0);

            var comment = _context.Comentarios.Find(commentId);
            Assert.NotNull(comment);
            Assert.Equal(postId, comment.FkPostagem);
        }

        [Fact]
        public async Task NonMember_Cannot_Comment_On_Community_Post()
        {
            // create a community post
            var postHandler = new CreatePostCommandHandler(_context);
            var createCmd = new CreatePostCommand
            {
                Title = "Comm Post 2",
                Content = "c",
                CommunityId = 100,
                NoForumGeral = false,
                RequesterId = "prof-1"
            };
            var postId = await postHandler.Handle(createCmd, CancellationToken.None);

            var commentHandler = new CreateCommentCommandHandler(_context);
            var commentCmd = new CreateCommentCommand
            {
                PostId = postId,
                Content = "Bad",
                RequesterId = "nonmember-1"
            };

            await Assert.ThrowsAsync<ArgumentException>(async () => await commentHandler.Handle(commentCmd, CancellationToken.None));
        }

        [Fact]
        public async Task Anyone_Can_Comment_On_ForumGeral_Post()
        {
            // create forum geral post
            var postHandler = new CreatePostCommandHandler(_context);
            var createCmd = new CreatePostCommand
            {
                Title = "Forum Post",
                Content = "c",
                CommunityId = null,
                NoForumGeral = true,
                RequesterId = "prof-1"
            };
            var postId = await postHandler.Handle(createCmd, CancellationToken.None);

            var commentHandler = new CreateCommentCommandHandler(_context);
            var commentCmd = new CreateCommentCommand
            {
                PostId = postId,
                Content = "Public",
                RequesterId = "nonmember-1"
            };

            var commentId = await commentHandler.Handle(commentCmd, CancellationToken.None);
            Assert.True(commentId > 0);
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
