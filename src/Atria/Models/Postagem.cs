using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Atria.Models
{
    [Table("TB_POSTAGEM")]
    public class Postagem
    {
        [Key]
        [Column("ID_POSTAGEM")]
        public int Id { get; set; }

        [Column("CONTEUDO")]
        [Required]
        public string Conteudo { get; set; } = string.Empty;

        [Column("DATA_POSTAGEM")]
        public DateTime DataPostagem { get; set; } = DateTime.UtcNow;

        [Column("NOFORUMGERAL")]
        public bool NoForumGeral { get; set; } = true;

        [Column("FK_USUARIO")]
        public int FKUsuario { get; set; }
        public ApplicationUser? Usuario { get; set; }

        [Column("FK_COMUNIDADE")]
        public int? FKComunidade { get; set; }
        public Comunidade? Comunidade { get; set; }
    }
}