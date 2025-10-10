using System.Reflection;
using Atria.Application.Common.Interfaces;
using Atria.Domain.Entities.UserContext;
using Atria.Domain.Entities.CommunityContext;
using Atria.Domain.Entities.MaterialContext;
using Atria.Domain.Entities.InteractionContext;
using Microsoft.EntityFrameworkCore;

namespace Atria.Infrastructure.Persistence;

public class AppDbContext : DbContext, IApplicationDbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    public DbSet<Usuario> Usuarios => Set<Usuario>();
    public DbSet<Comunidade> Comunidades => Set<Comunidade>();
    public DbSet<GrupoDeEstudo> GruposDeEstudo => Set<GrupoDeEstudo>();
    public DbSet<Postagem> Postagens => Set<Postagem>();
    public DbSet<Comentario> Comentarios => Set<Comentario>();
    public DbSet<Material> Materiais => Set<Material>();
    public DbSet<Livro> Livros => Set<Livro>();
    public DbSet<Artigo> Artigos => Set<Artigo>();
    public DbSet<ListaDeLeitura> ListasDeLeitura => Set<ListaDeLeitura>();
    public DbSet<Avaliacao> Avaliacoes => Set<Avaliacao>();
    public DbSet<Notificacao> Notificacoes => Set<Notificacao>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        
        // Define o charset padrão para todas as colunas string
        foreach (var entity in modelBuilder.Model.GetEntityTypes())
        {
            foreach (var property in entity.GetProperties())
            {
                if (property.ClrType == typeof(string))
                {
                    property.SetColumnType("varchar(255)");
                }
            }
        }

        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

        // Configura a herança TPH para Material
        modelBuilder.Entity<Material>()
            .HasDiscriminator(m => m.TipoMaterial)
            .HasValue<Livro>(TipoMaterial.Livro)
            .HasValue<Artigo>(TipoMaterial.Artigo);
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            throw new InvalidOperationException(
                "DbContext não está configurado. Certifique-se de que a string de conexão está definida.");
        }

        optionsBuilder.EnableSensitiveDataLogging();
        base.OnConfiguring(optionsBuilder);
    }
}