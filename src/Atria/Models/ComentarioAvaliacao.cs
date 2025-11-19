using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Atria.Models
{
    [Table("TB_COMENTARIO_AVALIACAO")]
    public class ComentarioAvaliacao
    {
  [Key]
     [Column("ID_COMENTARIO_AVALIACAO")]
  public int Id { get; set; }

 [Column("CONTEUDO")]
   [Required(ErrorMessage = "O conteúdo do comentário é obrigatório")]
    [MaxLength(500, ErrorMessage = "O comentário não pode ter mais de 500 caracteres")]
        public string Conteudo { get; set; } = string.Empty;

     [Column("DATA_COMENTARIO")]
        public DateTime DataComentario { get; set; } = DateTime.UtcNow;

    [Column("FK_USUARIO")]
        public int FKUsuario { get; set; } = 0;
  public ApplicationUser? Usuario { get; set; }

        [Column("FK_AVALIACAO")]
        public int FKAvaliacao { get; set; } = 0;
        public Avaliacao? Avaliacao { get; set; }
    }
}
