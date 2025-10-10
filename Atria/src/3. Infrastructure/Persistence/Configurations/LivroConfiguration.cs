using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Atria.Domain.Entities.MaterialContext;

namespace Atria.Infrastructure.Persistence.Configurations;

public class LivroConfiguration : IEntityTypeConfiguration<Livro>
{
    public void Configure(EntityTypeBuilder<Livro> builder)
    {
        builder.Property(l => l.ISBN)
            .HasColumnName("ISBN")
            .HasMaxLength(20);

        builder.Property(l => l.Editora)
            .HasColumnName("EDITORA")
            .HasMaxLength(100);
    }
}