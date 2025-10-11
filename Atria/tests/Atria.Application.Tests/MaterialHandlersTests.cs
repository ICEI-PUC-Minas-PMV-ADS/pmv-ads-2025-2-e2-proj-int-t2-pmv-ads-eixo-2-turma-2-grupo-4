using System;
using System.Threading;
using System.Threading.Tasks;
using Atria.Application.Features.Material.CreateMaterial;
using Atria.Application.Features.Material.ApproveMaterial;
using Atria.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Xunit;
using Atria.Domain.Entities.UserContext;
using Atria.Domain.Entities.CommunityContext;
using Atria.Domain.Entities.MaterialContext;
using Atria.Domain.Enums;

namespace Atria.Application.Tests
{
    public class MaterialHandlersTests : IDisposable
    {
        private readonly AppDbContext _context;

        public MaterialHandlersTests()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            _context = new AppDbContext(options);

            // Seed users
            var professor = new Usuario
            {
                IdUsuario = "prof-1",
                Nome = "Prof One",
                Email = "prof1@example.com",
                SenhaHash = "hash",
                TipoUsuario = TipoUsuario.Professor,
                DataCadastro = DateTime.UtcNow
            };

            var aluno = new Usuario
            {
                IdUsuario = "user-1",
                Nome = "User One",
                Email = "user1@example.com",
                SenhaHash = "hash",
                TipoUsuario = TipoUsuario.Comum,
                DataCadastro = DateTime.UtcNow
            };

            var moderator = new Usuario
            {
                IdUsuario = "mod-1",
                Nome = "Mod One",
                Email = "mod1@example.com",
                SenhaHash = "hash",
                TipoUsuario = TipoUsuario.Moderador,
                DataCadastro = DateTime.UtcNow
            };

            _context.Usuarios.AddRange(professor, aluno, moderator);
            _context.SaveChanges();

            // Seed community where professor is creator
            var community = new Comunidade
            {
                IdComunidade = 1,
                Nome = "Com1",
                Descricao = "Desc",
                FkCriador = professor.IdUsuario,
                DataCriacao = DateTime.UtcNow
            };

            _context.Comunidades.Add(community);
            _context.SaveChanges();

            // Seed membership: aluno is member, and make him modadmin for test
            var member = new ComunidadeMembro
            {
                UsuarioId = "user-1",
                ComunidadeId = community.IdComunidade,
                IsAdmin = false,
                IsModAdmin = true,
                IsPending = false,
                JoinedAt = DateTime.UtcNow
            };

            _context.ComunidadeMembros.Add(member);
            _context.SaveChanges();
        }

        [Fact]
        public async Task Professor_Can_Create_Material_Without_Community()
        {
            var handler = new CreateMaterialCommandHandler(_context);
            var cmd = new CreateMaterialCommand
            {
                Title = "Livro A",
                Author = "Autor",
                Year = 2020,
                TipoMaterial = "Livro",
                RequesterId = "prof-1"
            };

            var id = await handler.Handle(cmd, CancellationToken.None);
            Assert.True(id > 0);

            var created = await _context.Materiais.FindAsync(id);
            Assert.NotNull(created);
            Assert.Equal("Livro A", created.Titulo);
            Assert.Null(created.FkComunidade);
        }

        [Fact]
        public async Task NonProfessor_Cannot_Create_Material()
        {
            var handler = new CreateMaterialCommandHandler(_context);
            var cmd = new CreateMaterialCommand
            {
                Title = "Artigo X",
                Author = "Autor",
                Year = 2021,
                TipoMaterial = "Artigo",
                RequesterId = "user-1" // not a professor
            };

            await Assert.ThrowsAsync<ArgumentException>(async () => await handler.Handle(cmd, CancellationToken.None));
        }

        [Fact]
        public async Task Professor_Cannot_Create_Material_In_NonMember_Community()
        {
            // Create a professor who is not member of community
            var prof2 = new Usuario
            {
                IdUsuario = "prof-2",
                Nome = "Prof Two",
                Email = "prof2@example.com",
                SenhaHash = "hash",
                TipoUsuario = TipoUsuario.Professor,
                DataCadastro = DateTime.UtcNow
            };
            _context.Usuarios.Add(prof2);
            _context.SaveChanges();

            var handler = new CreateMaterialCommandHandler(_context);
            var cmd = new CreateMaterialCommand
            {
                Title = "Livro B",
                Author = "Autor",
                Year = 2019,
                TipoMaterial = "Livro",
                RequesterId = "prof-2",
                CommunityId = 1 // community exists but prof-2 is not member
            };

            await Assert.ThrowsAsync<ArgumentException>(async () => await handler.Handle(cmd, CancellationToken.None));
        }

        [Fact]
        public async Task Professor_Can_Create_Material_In_Community_As_Creator()
        {
            var handler = new CreateMaterialCommandHandler(_context);
            var cmd = new CreateMaterialCommand
            {
                Title = "Livro C",
                Author = "Autor",
                Year = 2018,
                TipoMaterial = "Livro",
                RequesterId = "prof-1",
                CommunityId = 1
            };

            var id = await handler.Handle(cmd, CancellationToken.None);
            Assert.True(id > 0);

            var created = await _context.Materiais.FindAsync(id);
            Assert.NotNull(created);
            Assert.Equal(1, created.FkComunidade);
        }

        [Fact]
        public async Task SystemModerator_Can_Approve_Material()
        {
            // Create a material by professor
            var createHandler = new CreateMaterialCommandHandler(_context);
            var createCmd = new CreateMaterialCommand
            {
                Title = "Livro D",
                Author = "Autor",
                Year = 2022,
                TipoMaterial = "Livro",
                RequesterId = "prof-1"
            };

            var id = await createHandler.Handle(createCmd, CancellationToken.None);

            var approveHandler = new ApproveMaterialCommandHandler(_context);
            var approveCmd = new ApproveMaterialCommand
            {
                MaterialId = id,
                RequesterId = "mod-1",
                Approve = true
            };

            await approveHandler.Handle(approveCmd, CancellationToken.None);

            var mat = await _context.Materiais.FindAsync(id);
            Assert.Equal("Aprovado", mat.Status);
        }

        [Fact]
        public async Task CommunityModAdmin_Can_Approve_Material_Linked_To_Community()
        {
            // Create material linked to community 1 by creator professor
            var createHandler = new CreateMaterialCommandHandler(_context);
            var createCmd = new CreateMaterialCommand
            {
                Title = "Livro E",
                Author = "Autor",
                Year = 2023,
                TipoMaterial = "Livro",
                RequesterId = "prof-1",
                CommunityId = 1
            };

            var id = await createHandler.Handle(createCmd, CancellationToken.None);

            // user-1 is modadmin in community
            var approveHandler = new ApproveMaterialCommandHandler(_context);
            var approveCmd = new ApproveMaterialCommand
            {
                MaterialId = id,
                RequesterId = "user-1",
                Approve = true
            };

            await approveHandler.Handle(approveCmd, CancellationToken.None);

            var mat = await _context.Materiais.FindAsync(id);
            Assert.Equal("Aprovado", mat.Status);
        }

        [Fact]
        public async Task NonAuthorized_Cannot_Approve_Material()
        {
            var createHandler = new CreateMaterialCommandHandler(_context);
            var createCmd = new CreateMaterialCommand
            {
                Title = "Livro F",
                Author = "Autor",
                Year = 2024,
                TipoMaterial = "Livro",
                RequesterId = "prof-1"
            };

            var id = await createHandler.Handle(createCmd, CancellationToken.None);

            var approveHandler = new ApproveMaterialCommandHandler(_context);
            var approveCmd = new ApproveMaterialCommand
            {
                MaterialId = id,
                RequesterId = "user-1", // user-1 is not modadmin for this material (material not linked to community)
                Approve = true
            };

            await Assert.ThrowsAsync<ArgumentException>(async () => await approveHandler.Handle(approveCmd, CancellationToken.None));
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
