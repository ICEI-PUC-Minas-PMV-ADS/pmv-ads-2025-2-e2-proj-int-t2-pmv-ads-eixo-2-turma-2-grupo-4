using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Atria.Application.Features.Communities.InviteMember;
using Atria.Application.Features.Communities.AcceptInvite;
using Atria.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Xunit;
using Atria.Domain.Entities.UserContext;
using Atria.Domain.Entities.CommunityContext;
using Atria.Domain.Entities.InteractionContext;
using Atria.Domain.Enums;

namespace Atria.Application.Tests
{
    public class InviteHandlersTests : IDisposable
    {
        private readonly AppDbContext _context;

        public InviteHandlersTests()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            _context = new AppDbContext(options);

            // Seed users
            var creator = new Usuario
            {
                IdUsuario = "creator-1",
                Nome = "Creator",
                Email = "creator@example.com",
                SenhaHash = "hash",
                TipoUsuario = TipoUsuario.Comum,
                DataCadastro = DateTime.UtcNow
            };

            var adminUser = new Usuario
            {
                IdUsuario = "admin-1",
                Nome = "Admin",
                Email = "admin@example.com",
                SenhaHash = "hash",
                TipoUsuario = TipoUsuario.Comum,
                DataCadastro = DateTime.UtcNow
            };

            var normalUser = new Usuario
            {
                IdUsuario = "user-1",
                Nome = "User",
                Email = "user@example.com",
                SenhaHash = "hash",
                TipoUsuario = TipoUsuario.Comum,
                DataCadastro = DateTime.UtcNow
            };

            var moderator = new Usuario
            {
                IdUsuario = "mod-1",
                Nome = "Mod",
                Email = "mod@example.com",
                SenhaHash = "hash",
                TipoUsuario = TipoUsuario.Moderador,
                DataCadastro = DateTime.UtcNow
            };

            _context.Usuarios.AddRange(creator, adminUser, normalUser, moderator);
            _context.SaveChanges();

            // Seed community
            var community = new Comunidade
            {
                IdComunidade = 10,
                Nome = "CommunityX",
                Descricao = "Desc",
                FkCriador = creator.IdUsuario,
                DataCriacao = DateTime.UtcNow
            };
            _context.Comunidades.Add(community);
            _context.SaveChanges();

            // Add adminUser as admin member
            var adminMembership = new ComunidadeMembro
            {
                UsuarioId = adminUser.IdUsuario,
                ComunidadeId = community.IdComunidade,
                IsAdmin = true,
                IsModAdmin = false,
                IsPending = false,
                JoinedAt = DateTime.UtcNow
            };
            _context.ComunidadeMembros.Add(adminMembership);
            _context.SaveChanges();
        }

        [Fact]
        public async Task Creator_Can_Invite_User()
        {
            var handler = new InviteMemberCommandHandler(_context);
            var cmd = new InviteMemberCommand
            {
                CommunityId = 10,
                TargetUserId = "user-1",
                RequesterId = "creator-1"
            };

            await handler.Handle(cmd, CancellationToken.None);

            var membership = _context.ComunidadeMembros.FirstOrDefault(m => m.UsuarioId == "user-1" && m.ComunidadeId == 10);
            Assert.NotNull(membership);
            Assert.True(membership.IsPending);

            var notif = _context.Notificacoes.FirstOrDefault(n => n.FkUsuario == "user-1" && n.Conteudo.Contains("convidado"));
            Assert.NotNull(notif);
        }

        [Fact]
        public async Task Admin_Can_Invite_User()
        {
            var handler = new InviteMemberCommandHandler(_context);
            var cmd = new InviteMemberCommand
            {
                CommunityId = 10,
                TargetUserId = "user-1",
                RequesterId = "admin-1"
            };

            await handler.Handle(cmd, CancellationToken.None);

            var membership = _context.ComunidadeMembros.FirstOrDefault(m => m.UsuarioId == "user-1" && m.ComunidadeId == 10);
            Assert.NotNull(membership);
            Assert.True(membership.IsPending);
        }

        [Fact]
        public async Task SystemModerator_Can_Invite_User()
        {
            var handler = new InviteMemberCommandHandler(_context);
            var cmd = new InviteMemberCommand
            {
                CommunityId = 10,
                TargetUserId = "user-1",
                RequesterId = "mod-1"
            };

            await handler.Handle(cmd, CancellationToken.None);

            var membership = _context.ComunidadeMembros.FirstOrDefault(m => m.UsuarioId == "user-1" && m.ComunidadeId == 10);
            Assert.NotNull(membership);
            Assert.True(membership.IsPending);
        }

        [Fact]
        public async Task NonAuthorized_Cannot_Invite()
        {
            var handler = new InviteMemberCommandHandler(_context);
            var cmd = new InviteMemberCommand
            {
                CommunityId = 10,
                TargetUserId = "user-1",
                RequesterId = "user-1" // normal user trying to invite
            };

            await Assert.ThrowsAsync<ArgumentException>(async () => await handler.Handle(cmd, CancellationToken.None));
        }

        [Fact]
        public async Task User_Can_Accept_Invite()
        {
            // First invite
            var inviteHandler = new InviteMemberCommandHandler(_context);
            var inviteCmd = new InviteMemberCommand
            {
                CommunityId = 10,
                TargetUserId = "user-1",
                RequesterId = "creator-1"
            };
            await inviteHandler.Handle(inviteCmd, CancellationToken.None);

            // Then accept
            var acceptHandler = new AcceptInviteCommandHandler(_context);
            var acceptCmd = new AcceptInviteCommand
            {
                CommunityId = 10,
                UserId = "user-1"
            };

            await acceptHandler.Handle(acceptCmd, CancellationToken.None);

            var membership = _context.ComunidadeMembros.FirstOrDefault(m => m.UsuarioId == "user-1" && m.ComunidadeId == 10);
            Assert.NotNull(membership);
            Assert.False(membership.IsPending);

            var inviterNotif = _context.Notificacoes.FirstOrDefault(n => n.FkUsuario == "creator-1" && n.Conteudo.Contains("aceitou"));
            Assert.NotNull(inviterNotif);
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
