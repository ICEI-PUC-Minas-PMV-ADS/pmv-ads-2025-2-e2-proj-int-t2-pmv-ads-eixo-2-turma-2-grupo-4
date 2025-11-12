using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Atria.Models
{
    [Table("TB_AVALIACAO")]
    public class Avaliacao
    {
        [Key]
        [Column("ID_AVALIACAO")]
        public int Id { get; set; }

        [Column("NOTA")]
        [Required]
        public int Nota { get; set; }

        [Column("TIPO_AVALIACAO")]
        [MaxLength(50)]
        public string? TipoAvaliacao { get; set; }

        [Column("FK_USUARIO")]
        public int FKUsuario { get; set; } = 0;
        public ApplicationUser? Usuario { get; set; }

        [Column("FK_MATERIAL")]
        public int FKMaterial { get; set; } = 0;
        public Material? Material { get; set; }

        [Column("RESENHA")]
        public string? Resenha { get; set; }
    }
}