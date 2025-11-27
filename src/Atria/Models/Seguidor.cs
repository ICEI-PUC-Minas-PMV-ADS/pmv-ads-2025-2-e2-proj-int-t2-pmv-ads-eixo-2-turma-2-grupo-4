using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Atria.Models
{
    [Table("TB_SEGUIDOR")]
    public class Seguidor
    {
        [Column("FK_SEGUIDOR")]
        public int FKSeguidor { get; set; }
        public ApplicationUser? UsuarioSeguidor { get; set; }

        [Column("FK_SEGUIDO")]
        public int FKSeguido { get; set; }
        public ApplicationUser? UsuarioSeguido { get; set; }

        [Column("DATA_INICIO")]
        public DateTime DataInicio { get; set; } = DateTime.UtcNow;
    }
}
