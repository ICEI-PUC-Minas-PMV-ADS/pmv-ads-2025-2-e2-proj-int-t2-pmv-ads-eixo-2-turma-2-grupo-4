using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Atria.Models
{
    [Table("TB_NOTIFICACAO")]
    public class Notificacao
    {
        [Key]
        [Column("ID_NOTIFICACAO")]
        public int Id { get; set; }

        [Column("FK_USUARIO")]
        public int FKUsuario { get; set; }
        public ApplicationUser? Usuario { get; set; }

        [Column("TITULO")]
        [Required]
        [MaxLength(200)]
        public string Titulo { get; set; } = string.Empty;

        [Column("MENSAGEM")]
        [Required]
        [MaxLength(500)]
        public string Mensagem { get; set; } = string.Empty;

        [Column("TIPO")]
        [MaxLength(50)]
        public string Tipo { get; set; } = "INFO"; // INFO, COMENTARIO, CURTIDA, RESPOSTA

        [Column("LINK")]
        [MaxLength(500)]
        public string? Link { get; set; }

        [Column("ICONE")]
        [MaxLength(50)]
        public string Icone { get; set; } = "bi-bell";

        [Column("COR")]
        [MaxLength(50)]
        public string Cor { get; set; } = "text-primary";

        [Column("LIDA")]
        public bool Lida { get; set; } = false;

        [Column("DATA_CRIACAO")]
        public DateTime DataCriacao { get; set; } = DateTime.UtcNow;
    }
}
