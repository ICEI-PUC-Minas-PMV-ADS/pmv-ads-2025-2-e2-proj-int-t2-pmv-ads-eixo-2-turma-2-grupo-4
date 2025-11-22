using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Atria.Models
{
    [Table("TB_MENSAGEM")]
    public class Mensagem
    {
        [Key]
        [Column("ID_MENSAGEM")]
        public int Id { get; set; }

        [Column("CONTEUDO")]
        [Required]
        [MaxLength(1000)]
        public string Conteudo { get; set; } = string.Empty;

        [Column("DATA_ENVIO")]
        public DateTime DataEnvio { get; set; } = DateTime.UtcNow;

        // QUEM ENVIOU (Sempre obrigatório)
        [Column("FK_REMETENTE")]
        public int FKRemetente { get; set; }
        public virtual ApplicationUser? Remetente { get; set; }

        // DESTINO 1: GRUPO (Opcional - Só preenche se for mensagem de grupo)
        [Column("FK_GRUPO")]
        public int? FKGrupo { get; set; }
        public virtual GrupoEstudo? GrupoEstudo { get; set; }

        // DESTINO 2: USUÁRIO (Opcional - Só preenche se for Direct/DM)
        [Column("FK_DESTINATARIO")]
        public int? FKDestinatario { get; set; }
        public virtual ApplicationUser? Destinatario { get; set; }
    }
}