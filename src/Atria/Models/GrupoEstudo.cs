using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Atria.Models
{
    [Table("TB_GRUPO_ESTUDO")]
    public class GrupoEstudo
    {
        [Key]
        [Column("ID_GRUPO")]
        public int Id { get; set; }

        [Column("NOME")]
        [Required]
        [MaxLength(150)]
        public string Nome { get; set; } = string.Empty;

        [Column("DESCRICAO")]
        public string? Descricao { get; set; }

        [Column("FK_COMUNIDADE")]
        public int FKComunidade { get; set; }
        public Comunidade? Comunidade { get; set; }

        [Column("DATA_CRIACAO")]
        public DateTime DataCriacao { get; set; } = DateTime.UtcNow;

        public ICollection<UsuarioGrupo> Usuarios { get; set; } = new List<UsuarioGrupo>();
        public ICollection<Postagem>? Postagens { get; set; } = new List<Postagem>();
    }
}