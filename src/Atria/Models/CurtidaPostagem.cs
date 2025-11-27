using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Atria.Models
{
    [Table("TB_CURTIDA_POSTAGEM")]
    public class CurtidaPostagem
    {
        [Key]
        [Column("ID_CURTIDA")]
        public int Id { get; set; }

        [Column("FK_POSTAGEM")]
        public int FKPostagem { get; set; }
        public Postagem? Postagem { get; set; }

        [Column("FK_USUARIO")]
        public int FKUsuario { get; set; }
        public ApplicationUser? Usuario { get; set; }

        [Column("DATA_CURTIDA")]
        public DateTime DataCurtida { get; set; } = DateTime.UtcNow;

        [Column("TIPO")]
        [MaxLength(10)]
        public string Tipo { get; set; } = "LIKE";
    }
}
