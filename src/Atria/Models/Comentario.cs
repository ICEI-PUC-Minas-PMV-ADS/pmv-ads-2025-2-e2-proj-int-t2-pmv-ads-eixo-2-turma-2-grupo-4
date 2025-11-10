using System;
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
        [Required]
        public string Conteudo { get; set; } = string.Empty;

        [Column("DATA_COMENTARIO")]
        public DateTime DataComentario { get; set; } = DateTime.UtcNow;

        // FK para o post ao qual o comentário pertence
        [Column("FK_POSTAGEM")]
        public int FKPostagem { get; set; }
        public Postagem? Postagem { get; set; }

        [Column("FK_USUARIO")]
        public int FKUsuario { get; set; }
        public ApplicationUser? Usuario { get; set; }
    }
}