using Atria.Domain.Entities.UserContext;
using Microsoft.EntityFrameworkCore;

namespace Atria.Application.Common.Interfaces;

public interface IApplicationDbContext
{
    DbSet<Usuario> Usuarios { get; }
    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}