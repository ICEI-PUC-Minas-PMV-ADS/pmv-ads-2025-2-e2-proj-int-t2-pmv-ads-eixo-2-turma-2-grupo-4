using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Atria.Domain.Entities.InteractionContext;

namespace Atria.Infrastructure.Persistence.Configurations;

public class NotificacaoConfiguration : IEntityTypeConfiguration<Notificacao>
{
    public void Configure(EntityTypeBuilder<Notificacao> builder)
    {
        builder.ToTable("TB_NOTIFICACAO");

        builder.HasKey(n => n.IdNotificacao);

        builder.Property(n => n.IdNotificacao)
            .HasColumnName("ID_NOTIFICACAO")
            .ValueGeneratedOnAdd();

        builder.Property(n => n.Conteudo)
            .HasColumnName("CONTEUDO")
            .HasMaxLength(255)
            .IsRequired();

        builder.Property(n => n.DataCriacao)
            .HasColumnName("DATA_CRIACAO")
            .HasColumnType("datetime")
            .IsRequired();

        builder.Property(n => n.FkUsuario)
            .HasColumnName("FK_USUARIO")
            .HasMaxLength(36)
            .IsRequired();

        // Relacionamento
        builder.HasOne(n => n.Usuario)
            .WithMany(u => u.Notificacoes)
            .HasForeignKey(n => n.FkUsuario)
            .OnDelete(DeleteBehavior.Cascade);
    }
}