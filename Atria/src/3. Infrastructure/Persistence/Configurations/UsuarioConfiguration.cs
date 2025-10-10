using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Atria.Domain.Entities.UserContext;

namespace Atria.Infrastructure.Persistence.Configurations;

public class UsuarioConfiguration : IEntityTypeConfiguration<Usuario>
{
    public void Configure(EntityTypeBuilder<Usuario> builder)
    {
        builder.ToTable("TB_USUARIO");

        builder.HasKey(u => u.IdUsuario);

        builder.Property(u => u.IdUsuario)
            .HasColumnName("ID_USUARIO")
            .HasMaxLength(36)
            .IsRequired();

        builder.Property(u => u.Nome)
            .HasColumnName("NOME")
            .HasMaxLength(255)
            .IsRequired();

        builder.Property(u => u.Email)
            .HasColumnName("EMAIL")
            .HasMaxLength(150)
            .IsRequired();

        builder.HasIndex(u => u.Email)
            .IsUnique();

        builder.Property(u => u.SenhaHash)
            .HasColumnName("SENHA_HASH")
            .HasMaxLength(255)
            .IsRequired();

        builder.Property(u => u.TipoUsuario)
            .HasColumnName("TIPO_USUARIO")
            .HasConversion<string>()
            .HasMaxLength(20)
            .IsRequired();

        builder.Property(u => u.DataCadastro)
            .HasColumnName("DATA_CADASTRO")
            .HasColumnType("datetime")
            .IsRequired();

        builder.Property(u => u.Matricula)
            .HasColumnName("MATRICULA")
            .HasMaxLength(50);

        builder.HasIndex(u => u.Matricula)
            .IsUnique()
            .HasFilter("MATRICULA IS NOT NULL");

        builder.Property(u => u.AreaAtuacao)
            .HasColumnName("AREA_ATUACAO")
            .HasMaxLength(100);
    }
}