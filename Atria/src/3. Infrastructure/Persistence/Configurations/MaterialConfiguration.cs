using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Atria.Domain.Entities.MaterialContext;

namespace Atria.Infrastructure.Persistence.Configurations;

public class MaterialConfiguration : IEntityTypeConfiguration<Material>
{
    public void Configure(EntityTypeBuilder<Material> builder)
    {
        builder.ToTable("TB_MATERIAL");

        builder.HasKey(m => m.IdMaterial);

        builder.Property(m => m.IdMaterial)
            .HasColumnName("ID_MATERIAL")
            .ValueGeneratedOnAdd();

        builder.Property(m => m.Titulo)
            .HasColumnName("TITULO")
            .HasMaxLength(255)
            .IsRequired();

        builder.Property(m => m.Autor)
            .HasColumnName("AUTOR")
            .HasMaxLength(255)
            .IsRequired();

        builder.Property(m => m.AnoPublicacao)
            .HasColumnName("ANO_PUBLICACAO");

        builder.Property(m => m.Status)
            .HasColumnName("STATUS")
            .HasMaxLength(50);

        builder.Property(m => m.TipoMaterial)
            .HasColumnName("TIPO_MATERIAL")
            .HasConversion<string>()
            .HasMaxLength(20)
            .IsRequired();

        builder.Property(m => m.FkProfessorCadastro)
            .HasColumnName("FK_PROFESSOR_CADASTRO")
            .HasMaxLength(36)
            .IsRequired(false); // Permitir nulo para compatibilidade com ON DELETE SET NULL

        builder.HasOne(m => m.ProfessorCadastro)
            .WithMany(p => p.MateriaisCadastrados)
            .HasForeignKey(m => m.FkProfessorCadastro)
            .OnDelete(DeleteBehavior.SetNull);

        // TPH (Table-per-Hierarchy) configuration
        builder.HasDiscriminator(m => m.TipoMaterial)
            .HasValue<Livro>(TipoMaterial.Livro)
            .HasValue<Artigo>(TipoMaterial.Artigo);
    }
}