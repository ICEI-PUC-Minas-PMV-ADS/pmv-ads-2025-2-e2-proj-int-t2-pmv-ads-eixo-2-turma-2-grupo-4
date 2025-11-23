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

        // NOVO: Suporte para comentários aninhados em avaliações (estilo Reddit)
        [Column("FK_COMENTARIO_PAI")]
 public int? FKComentarioPai { get; set; }
        
        [ForeignKey("FKComentarioPai")]
        public ComentarioAvaliacao? ComentarioPai { get; set; }
        
        public ICollection<ComentarioAvaliacao> Respostas { get; set; } = new List<ComentarioAvaliacao>();

        // NOVO: Sistema de curtidas
  public ICollection<CurtidaComentarioAvaliacao> Curtidas { get; set; } = new List<CurtidaComentarioAvaliacao>();

        // Propriedade calculada para total de curtidas
   [NotMapped]
   public int TotalCurtidas => Curtidas?.Count ?? 0;
    }
}
