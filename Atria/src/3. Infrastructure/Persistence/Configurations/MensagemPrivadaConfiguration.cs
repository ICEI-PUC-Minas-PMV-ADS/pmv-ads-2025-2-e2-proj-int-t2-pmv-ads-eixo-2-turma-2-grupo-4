using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Atria.Domain.Entities.InteractionContext;

namespace Atria.Infrastructure.Persistence.Configurations;

public class MensagemPrivadaConfiguration : IEntityTypeConfiguration<MensagemPrivada>
{
    public void Configure(EntityTypeBuilder<MensagemPrivada> builder)
    {
        builder.ToTable("TB_MENSAGEM_PRIVADA");

        builder.HasKey(m => m.IdMensagem);

        builder.Property(m => m.IdMensagem)
            .HasColumnName("ID_MENSAGEM")
            .HasMaxLength(36)
            .IsRequired();

        builder.Property(m => m.Conteudo)
            .HasColumnName("CONTEUDO")
            .HasColumnType("text")
            .IsRequired();

        builder.Property(m => m.DataEnvio)
            .HasColumnName("DATA_ENVIO")
            .HasColumnType("datetime")
            .IsRequired();

        builder.Property(m => m.Lida)
            .HasColumnName("LIDA")
            .IsRequired();

        builder.Property(m => m.FkRemetente)
            .HasColumnName("FK_REMETENTE")
            .HasMaxLength(36)
            .IsRequired();

        builder.Property(m => m.FkDestinatario)
            .HasColumnName("FK_DESTINATARIO")
            .HasMaxLength(36)
            .IsRequired();

        builder.HasOne(m => m.Remetente)
            .WithMany(u => u.MensagensEnviadas)
            .HasForeignKey(m => m.FkRemetente)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(m => m.Destinatario)
            .WithMany(u => u.MensagensRecebidas)
            .HasForeignKey(m => m.FkDestinatario)
            .OnDelete(DeleteBehavior.Cascade);
    }
}