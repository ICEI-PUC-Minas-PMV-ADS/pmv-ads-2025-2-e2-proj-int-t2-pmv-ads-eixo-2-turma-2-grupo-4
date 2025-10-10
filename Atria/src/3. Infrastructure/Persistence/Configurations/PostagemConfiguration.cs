using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Atria.Domain.Entities.CommunityContext;

namespace Atria.Infrastructure.Persistence.Configurations;

public class PostagemConfiguration : IEntityTypeConfiguration<Postagem>
{
    public void Configure(EntityTypeBuilder<Postagem> builder)
    {
        builder.ToTable("TB_POSTAGEM");

        builder.HasKey(p => p.IdPostagem);

        builder.Property(p => p.IdPostagem)
            .HasColumnName("ID_POSTAGEM")
            .ValueGeneratedOnAdd();

        builder.Property(p => p.Titulo)
            .HasColumnName("TITULO")
            .HasMaxLength(255)
            .IsRequired();

        builder.Property(p => p.Conteudo)
            .HasColumnName("CONTEUDO")
            .HasColumnType("text")
            .IsRequired();

        builder.Property(p => p.DataCriacao)
            .HasColumnName("DATA_CRIACAO")
            .HasColumnType("datetime")
            .IsRequired();

        builder.Property(p => p.NoForumGeral)
            .HasColumnName("NO_FORUM_GERAL")
            .IsRequired();

        builder.Property(p => p.FkAutor)
            .HasColumnName("FK_AUTOR")
            .HasMaxLength(36)
            .IsRequired();

        builder.Property(p => p.FkComunidade)
            .HasColumnName("FK_COMUNIDADE");

        // Relacionamentos
        builder.HasOne(p => p.Autor)
            .WithMany(u => u.Postagens)
            .HasForeignKey(p => p.FkAutor)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(p => p.Comunidade)
            .WithMany(c => c.Postagens)
            .HasForeignKey(p => p.FkComunidade)
            .OnDelete(DeleteBehavior.Cascade);

        // Check constraint para garantir que postagens no fórum geral não tenham comunidade
        builder.ToTable(t => t.HasCheckConstraint(
            "CHK_POSTAGEM_LOCAL",
            "(FK_COMUNIDADE IS NULL AND NO_FORUM_GERAL = 1) OR (FK_COMUNIDADE IS NOT NULL AND NO_FORUM_GERAL = 0)"));
    }
}