using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Atria.Domain.Entities.MaterialContext;

namespace Atria.Infrastructure.Persistence.Configurations;

public class ArtigoConfiguration : IEntityTypeConfiguration<Artigo>
{
    public void Configure(EntityTypeBuilder<Artigo> builder)
    {
        builder.Property(a => a.DOI)
            .HasColumnName("DOI")
            .HasMaxLength(100);
    }
}