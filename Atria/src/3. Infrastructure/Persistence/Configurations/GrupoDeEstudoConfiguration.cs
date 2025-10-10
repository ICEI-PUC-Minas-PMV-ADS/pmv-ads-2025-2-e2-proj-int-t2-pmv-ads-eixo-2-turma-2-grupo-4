using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Atria.Domain.Entities.CommunityContext;

namespace Atria.Infrastructure.Persistence.Configurations;

public class GrupoDeEstudoConfiguration : IEntityTypeConfiguration<GrupoDeEstudo>
{
    public void Configure(EntityTypeBuilder<GrupoDeEstudo> builder)
    {
        builder.ToTable("TB_GRUPO_ESTUDO");

        builder.HasKey(g => g.IdGrupoEstudo);

        builder.Property(g => g.IdGrupoEstudo)
            .HasColumnName("ID_GRUPO_ESTUDO")
            .ValueGeneratedOnAdd();

        builder.Property(g => g.Nome)
            .HasColumnName("NOME")
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(g => g.DataCriacao)
            .HasColumnName("DATA_CRIACAO")
            .HasColumnType("datetime")
            .IsRequired();

        builder.Property(g => g.FkComunidade)
            .HasColumnName("FK_COMUNIDADE")
            .IsRequired();

        // Relacionamento
        builder.HasOne(g => g.Comunidade)
            .WithMany(c => c.GruposDeEstudo)
            .HasForeignKey(g => g.FkComunidade)
            .OnDelete(DeleteBehavior.Cascade);
    }
}