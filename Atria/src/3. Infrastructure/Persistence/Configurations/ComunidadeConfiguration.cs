using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Atria.Domain.Entities.CommunityContext;

namespace Atria.Infrastructure.Persistence.Configurations;

public class ComunidadeConfiguration : IEntityTypeConfiguration<Comunidade>
{
    public void Configure(EntityTypeBuilder<Comunidade> builder)
    {
        builder.ToTable("TB_COMUNIDADE");

        builder.HasKey(c => c.IdComunidade);

        builder.Property(c => c.IdComunidade)
            .HasColumnName("ID_COMUNIDADE")
            .ValueGeneratedOnAdd();

        builder.Property(c => c.Nome)
            .HasColumnName("NOME")
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(c => c.Descricao)
            .HasColumnName("DESCRICAO")
            .HasColumnType("text");

        builder.Property(c => c.DataCriacao)
            .HasColumnName("DATA_CRIACAO")
            .HasColumnType("datetime")
            .IsRequired();

        builder.Property(c => c.IsForumGeral)
            .HasColumnName("IS_FORUM_GERAL")
            .HasDefaultValue(false)
            .IsRequired();

        builder.Property(c => c.FkCriador)
            .HasColumnName("FK_CRIADOR")
            .HasMaxLength(36);

        // Relacionamento
        builder.HasOne(c => c.Criador)
            .WithMany(u => u.ComunidadesCriadas)
            .HasForeignKey(c => c.FkCriador)
            .OnDelete(DeleteBehavior.SetNull);

        // Relacionamento para membro
        builder.HasMany(c => c.Membros)
            .WithOne(cm => cm.Comunidade)
            .HasForeignKey(cm => cm.ComunidadeId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}