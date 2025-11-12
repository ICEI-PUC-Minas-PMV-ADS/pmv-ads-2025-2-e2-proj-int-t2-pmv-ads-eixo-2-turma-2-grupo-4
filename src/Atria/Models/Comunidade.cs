using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Atria.Models
{
    [Table("TB_COMUNIDADE")]
    public class Comunidade
    {
        [Key]
        [Column("ID_COMUNIDADE")]
        public int Id { get; set; }

        [Column("NOME")]
        [Required]
        [MaxLength(150)]
        public string Nome { get; set; } = string.Empty;

        [Column("DESCRICAO")]
        public string? Descricao { get; set; }

        [Column("DATA_CRIACAO")]
        public DateTime DataCriacao { get; set; } = DateTime.UtcNow;

        public ICollection<Postagem>? Postagens { get; set; } = new List<Postagem>();
        public ICollection<UsuarioComunidade> Usuarios { get; set; } = new List<UsuarioComunidade>();
        public ICollection<GrupoEstudo>? GruposEstudo { get; set; } = new List<GrupoEstudo>();
    }
}