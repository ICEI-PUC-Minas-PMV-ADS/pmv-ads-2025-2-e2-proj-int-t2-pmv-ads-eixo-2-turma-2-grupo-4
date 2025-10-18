using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Atria.Models
{
    // User customizado herdando IdentityUser<int> para usar int como PK
    [Table("TB_USUARIO")]
    public class ApplicationUser : IdentityUser<int>
    {
        [Column("NOME")]
        [Required]
        [MaxLength(150)]
        public string Nome { get; set; } = string.Empty;

        [Column("EMAIL")]
        [Required]
        [MaxLength(150)]
        public override string? Email { get => base.Email; set => base.Email = value; }

        // Identity já possui PasswordHash (Senha criptografada)
        [Column("SENHA_HASH")]
        public override string? PasswordHash { get => base.PasswordHash; set => base.PasswordHash = value; }

        [Column("DATA_CADASTRO")]
        public DateTime DataCadastro { get; set; } = DateTime.UtcNow;

        // Tipo do usuário (ex: Comum, Professor, Moderador)
        [Column("TIPO_USUARIO")]
        [MaxLength(50)]
        public string? TipoUsuario { get; set; }

        // Outros campos podem ser adicionados conforme necessidade
    }
}