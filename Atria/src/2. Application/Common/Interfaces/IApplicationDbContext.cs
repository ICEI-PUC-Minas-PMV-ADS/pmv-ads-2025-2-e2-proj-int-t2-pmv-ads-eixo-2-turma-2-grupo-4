using Atria.Domain.Entities.UserContext;
using Atria.Domain.Entities.InteractionContext;
using Atria.Domain.Entities.CommunityContext;
using Atria.Domain.Entities.MaterialContext;
using Microsoft.EntityFrameworkCore;

namespace Atria.Application.Common.Interfaces;

public interface IApplicationDbContext
{
    DbSet<Usuario> Usuarios { get; }
    DbSet<MensagemPrivada> MensagensPrivadas { get; }
    DbSet<Comunidade> Comunidades { get; }
    DbSet<ComunidadeMembro> ComunidadeMembros { get; }
    DbSet<Material> Materiais { get; }
    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}