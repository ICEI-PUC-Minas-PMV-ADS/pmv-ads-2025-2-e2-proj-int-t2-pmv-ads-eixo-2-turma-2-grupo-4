using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Atria.Domain.Entities.InteractionContext;

namespace Atria.Infrastructure.Persistence.Configurations;

public class AvaliacaoConfiguration : IEntityTypeConfiguration<Avaliacao>
{
    public void Configure(EntityTypeBuilder<Avaliacao> builder)
    {
        builder.ToTable("TB_AVALIACAO");

        builder.HasKey(a => a.IdAvaliacao);

        builder.Property(a => a.IdAvaliacao)
            .HasColumnName("ID_AVALIACAO")
            .ValueGeneratedOnAdd();

        builder.Property(a => a.Nota)
            .HasColumnName("NOTA")
            .HasPrecision(3, 1)
            .IsRequired();

        builder.Property(a => a.Resenha)
            .HasColumnName("RESENHA")
            .HasColumnType("text")
            .IsRequired();

        builder.Property(a => a.DataAvaliacao)
            .HasColumnName("DATA_AVALIACAO")
            .HasColumnType("datetime")
            .IsRequired();

        builder.Property(a => a.TipoAvaliacao)
            .HasColumnName("TIPO_AVALIACAO")
            .HasConversion<string>()
            .HasMaxLength(20)
            .IsRequired();

        builder.Property(a => a.TextoEspecialista)
            .HasColumnName("TEXTO_ESPECIALISTA")
            .HasColumnType("text");

        builder.Property(a => a.FkAutor)
            .HasColumnName("FK_AUTOR")
            .HasMaxLength(36)
            .IsRequired();

        builder.Property(a => a.FkMaterial)
            .HasColumnName("FK_MATERIAL")
            .IsRequired();

        // Relacionamentos
        builder.HasOne(a => a.Autor)
            .WithMany(u => u.Avaliacoes)
            .HasForeignKey(a => a.FkAutor)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(a => a.Material)
            .WithMany(m => m.Avaliacoes)
            .HasForeignKey(a => a.FkMaterial)
            .OnDelete(DeleteBehavior.Cascade);

        // Garantir que um usuário só possa avaliar um material uma vez
        builder.HasIndex(a => new { a.FkAutor, a.FkMaterial })
            .IsUnique();
    }
}