using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Atria.Models
{
    [Table("TB_CURTIDA_COMENTARIO_AVALIACAO")]
    public class CurtidaComentarioAvaliacao
    {
     [Key]
 [Column("ID_CURTIDA")]
  public int Id { get; set; }

   [Column("FK_COMENTARIO_AVALIACAO")]
        public int FKComentarioAvaliacao { get; set; }
public ComentarioAvaliacao? ComentarioAvaliacao { get; set; }

     [Column("FK_USUARIO")]
 public int FKUsuario { get; set; }
        public ApplicationUser? Usuario { get; set; }

     [Column("DATA_CURTIDA")]
    public DateTime DataCurtida { get; set; } = DateTime.UtcNow;

  [Column("TIPO")]
   [MaxLength(10)]
        public string Tipo { get; set; } = "LIKE"; // LIKE ou DISLIKE (para futuro)
    }
}
