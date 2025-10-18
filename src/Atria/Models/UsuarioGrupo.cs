using System.ComponentModel.DataAnnotations.Schema;

namespace Atria.Models
{
    [Table("TB_USUARIO_GRUPO")]
    public class UsuarioGrupo
    {
        [Column("FK_USUARIO")]
        public int FKUsuario { get; set; }
        public ApplicationUser? Usuario { get; set; }

        [Column("FK_GRUPO")]
        public int FKGrupo { get; set; }
        public GrupoEstudo? GrupoEstudo { get; set; }

        [Column("DATA_ENTRADA")]
        public DateTime DataEntrada { get; set; } = DateTime.UtcNow;
    }
}