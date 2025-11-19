using System;
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

        [Column("TITULO")]
        [Required(ErrorMessage = "O título da postagem é obrigatório")]
        [StringLength(200, ErrorMessage = "O título não pode ter mais de 200 caracteres")]
        public string Titulo { get; set; } = string.Empty;

        [Column("CONTEUDO")]
        [Required(ErrorMessage = "O conteúdo da postagem é obrigatório")]
        [StringLength(5000, ErrorMessage = "O conteúdo não pode ter mais de 5000 caracteres")]
        public string Conteudo { get; set; } = string.Empty;

        [Column("DATA_POSTAGEM")]
        public DateTime DataPostagem { get; set; } = DateTime.UtcNow;

        [Column("NOFORUMGERAL")]
        public bool NoForumGeral { get; set; } = true;

        [Column("FK_USUARIO")]
        public int FKUsuario { get; set; }
        public ApplicationUser? Usuario { get; set; }

        [Column("FK_COMUNIDADE")]
        public int? FKComunidade { get; set; } = 0;
        public Comunidade? Comunidade { get; set; }

        // Suporte para postagens vinculadas a grupos de estudo
        [Column("FK_GRUPO")]
        public int? FKGrupo { get; set; } = 0;
        public GrupoEstudo? GrupoEstudo { get; set; }

        // Coleção de comentários associados à postagem
        public ICollection<Comentario>? Comentarios { get; set; }

        // Define se a postagem deve ser visível na aba geral.
        // Regras: se ambos FKComunidade e FKGrupo forem nulos, vazios (não aplicável a int) ou zero => verdadeiro.
        // Caso contrário => falso.
        public void SetVisibleOnGeral()
        {
            var comunidadeIsEmpty = !FKComunidade.HasValue || FKComunidade.GetValueOrDefault() == 0;
            var grupoIsEmpty = !FKGrupo.HasValue || FKGrupo.GetValueOrDefault() == 0;

            NoForumGeral = comunidadeIsEmpty && grupoIsEmpty;
        }
    }
}