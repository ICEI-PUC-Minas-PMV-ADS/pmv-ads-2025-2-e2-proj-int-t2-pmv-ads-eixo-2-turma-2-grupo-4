using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Atria.Models
{
    [Table("TB_COMENTARIO")]
    public class Comentario
    {
        [Key]
      [Column("ID_COMENTARIO")]
        public int Id { get; set; }

  [Column("CONTEUDO")]
        [Required(ErrorMessage = "O conteúdo do comentário é obrigatório")]
    [MaxLength(1000, ErrorMessage = "O comentário não pode ter mais de 1000 caracteres")]
        public string Conteudo { get; set; } = string.Empty;

 [Column("DATA_COMENTARIO")]
    public DateTime DataComentario { get; set; } = DateTime.UtcNow;

        [Column("FK_USUARIO")]
        public int FKUsuario { get; set; } = 0;
        public ApplicationUser? Usuario { get; set; }

        [Column("FK_POSTAGEM")] 
        public int FKPostagem { get; set; } = 0;
        public Postagem? Postagem { get; set; }
    }
}
