using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Atria.Models
{
    [Table("TB_USUARIO_COMUNIDADE")]
    public class UsuarioComunidade
    {
        [Column("FK_USUARIO")]
        public int FKUsuario { get; set; }
        public ApplicationUser? Usuario { get; set; }

        [Column("FK_COMUNIDADE")]
        public int FKComunidade { get; set; }
        public Comunidade? Comunidade { get; set; }

        [Column("DATA_ENTRADA")]
        public DateTime DataEntrada { get; set; } = DateTime.UtcNow;
    }
}