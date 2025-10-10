using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Atria.Domain.Entities.CommunityContext;

namespace Atria.Infrastructure.Persistence.Configurations;

public class ComentarioConfiguration : IEntityTypeConfiguration<Comentario>
{
    public void Configure(EntityTypeBuilder<Comentario> builder)
    {
        builder.ToTable("TB_COMENTARIO");

        builder.HasKey(c => c.IdComentario);

        builder.Property(c => c.IdComentario)
            .HasColumnName("ID_COMENTARIO")
            .ValueGeneratedOnAdd();

        builder.Property(c => c.Conteudo)
            .HasColumnName("CONTEUDO")
            .HasColumnType("text")
            .IsRequired();

        builder.Property(c => c.DataCriacao)
            .HasColumnName("DATA_CRIACAO")
            .HasColumnType("datetime")
            .IsRequired();

        builder.Property(c => c.FkAutor)
            .HasColumnName("FK_AUTOR")
            .HasMaxLength(36)
            .IsRequired();

        builder.Property(c => c.FkPostagem)
            .HasColumnName("FK_POSTAGEM")
            .IsRequired();

        // Relacionamentos
        builder.HasOne(c => c.Autor)
            .WithMany(u => u.Comentarios)
            .HasForeignKey(c => c.FkAutor)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(c => c.Postagem)
            .WithMany(p => p.Comentarios)
            .HasForeignKey(c => c.FkPostagem)
            .OnDelete(DeleteBehavior.Cascade);
    }
}