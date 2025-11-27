using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Atria.Models
{
    [Table("TB_VISUALIZACAO_POSTAGEM")]
    public class VisualizacaoPostagem
    {
        [Key]
        [Column("ID_VISUALIZACAO")]
        public int Id { get; set; }

        [Column("FK_POSTAGEM")]
        public int FKPostagem { get; set; }
        public Postagem? Postagem { get; set; }

        [Column("FK_USUARIO")]
        public int? FKUsuario { get; set; }
        public ApplicationUser? Usuario { get; set; }

        [Column("DATA_VISUALIZACAO")]
        public DateTime DataVisualizacao { get; set; } = DateTime.UtcNow;

        [Column("IP_ADDRESS")]
        [MaxLength(45)]
        public string? IpAddress { get; set; }
    }
}
