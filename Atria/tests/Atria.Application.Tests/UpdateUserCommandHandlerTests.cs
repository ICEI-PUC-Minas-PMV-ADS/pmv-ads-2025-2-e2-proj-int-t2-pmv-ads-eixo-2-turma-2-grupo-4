using System;
using System.Threading;
using System.Threading.Tasks;
using Atria.Application.Features.Users.UpdateUser;
using Atria.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Xunit;
using Atria.Domain.Entities.UserContext;
using Atria.Domain.Enums;

namespace Atria.Application.Tests;

public class UpdateUserCommandHandlerTests : IDisposable
{
    private readonly AppDbContext _context;

    public UpdateUserCommandHandlerTests()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        _context = new AppDbContext(options);

        // Seed a user
        var user = new Usuario
        {
            IdUsuario = "user-1",
            Nome = "Original",
            Email = "original@example.com",
            SenhaHash = "hash",
            TipoUsuario = TipoUsuario.Comum,
            DataCadastro = DateTime.UtcNow
        };

        _context.Usuarios.Add(user);
        _context.SaveChanges();
    }

    [Fact]
    public async Task OwnerCanUpdateTheirProfile()
    {
        var handler = new UpdateUserCommandHandler(_context);

        var cmd = new UpdateUserCommand
        {
            Id = "user-1",
            Nome = "Updated",
            RequesterId = "user-1",
            RequesterRole = null
        };

        var result = await handler.Handle(cmd, CancellationToken.None);

        Assert.Equal("Updated", result.Nome);
    }

    [Fact]
    public async Task ModeratorCanUpdateOtherProfile()
    {
        var handler = new UpdateUserCommandHandler(_context);

        var cmd = new UpdateUserCommand
        {
            Id = "user-1",
            Nome = "ModeratorUpdate",
            RequesterId = "other",
            RequesterRole = "Moderador"
        };

        var result = await handler.Handle(cmd, CancellationToken.None);

        Assert.Equal("ModeratorUpdate", result.Nome);
    }

    [Fact]
    public async Task NonOwnerNonModeratorCannotUpdate()
    {
        var handler = new UpdateUserCommandHandler(_context);

        var cmd = new UpdateUserCommand
        {
            Id = "user-1",
            Nome = "BadUpdate",
            RequesterId = "other",
            RequesterRole = "Comum"
        };

        await Assert.ThrowsAsync<ArgumentException>(async () => await handler.Handle(cmd, CancellationToken.None));
    }

    public void Dispose()
    {
        _context.Dispose();
    }
}