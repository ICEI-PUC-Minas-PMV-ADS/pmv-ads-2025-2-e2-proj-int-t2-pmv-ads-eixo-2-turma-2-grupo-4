using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Atria.Domain.Entities.MaterialContext;

namespace Atria.Infrastructure.Persistence.Configurations;

public class ListaDeLeituraConfiguration : IEntityTypeConfiguration<ListaDeLeitura>
{
    public void Configure(EntityTypeBuilder<ListaDeLeitura> builder)
    {
        builder.ToTable("TB_LISTA_LEITURA");

        builder.HasKey(l => l.IdListaLeitura);

        builder.Property(l => l.IdListaLeitura)
            .HasColumnName("ID_LISTA_LEITURA")
            .ValueGeneratedOnAdd();

        builder.Property(l => l.Nome)
            .HasColumnName("NOME")
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(l => l.FkCriador)
            .HasColumnName("FK_CRIADOR")
            .HasMaxLength(36)
            .IsRequired();

        // Relacionamento com Usuario
        builder.HasOne(l => l.Criador)
            .WithMany(u => u.ListasDeLeitura)
            .HasForeignKey(l => l.FkCriador)
            .OnDelete(DeleteBehavior.Cascade);

        // Relacionamento N:N com Material
        builder.HasMany(l => l.Materiais)
            .WithMany(m => m.ListasDeLeitura)
            .UsingEntity<Dictionary<string, object>>(
                "TB_LISTA_TEM_MATERIAL",
                j => j
                    .HasOne<Material>()
                    .WithMany()
                    .HasForeignKey("FK_MATERIAL")
                    .OnDelete(DeleteBehavior.Cascade),
                j => j
                    .HasOne<ListaDeLeitura>()
                    .WithMany()
                    .HasForeignKey("FK_LISTA_LEITURA")
                    .OnDelete(DeleteBehavior.Cascade),
                j =>
                {
                    j.HasKey("FK_LISTA_LEITURA", "FK_MATERIAL");
                    j.Property<DateTime>("DATA_ADICAO")
                        .HasColumnName("DATA_ADICAO")
                        .HasColumnType("datetime")
                        .ValueGeneratedOnAdd();
                });
    }
}