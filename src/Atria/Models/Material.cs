using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Atria.Models
{
    [Table("TB_MATERIAL")]
    public class Material
    {
        [Key]
        [Column("ID_MATERIAL")]
        public int Id { get; set; }

        [Column("TITULO")]
        [Required]
        [MaxLength(250)]
        public string Titulo { get; set; } = string.Empty;

        [Column("DESCRICAO")]
        public string? Descricao { get; set; }

        [Column("TIPO")]
        [MaxLength(50)]
        public string? Tipo { get; set; }

        [Column("CAMINHOIMG")]
        [MaxLength(250)]
        public string? CAMINHOIMG { get; set; }


        [Column("FK_USUARIO_CRIADOR")]
        public int FKUsuarioCriador { get; set; }
        public ApplicationUser? Criador { get; set; }


        [Column("DATA_CRIACAO")]
        public DateTime DataCriacao { get; set; } = DateTime.UtcNow;

        [Column("STATUS")]
        public string? Status { get; set; }

        // Inicializar coleções para evitar nulidade
        public ICollection<Avaliacao> Avaliacoes { get; set; } = new List<Avaliacao>();
        public ICollection<ListaTemMaterial> ListaTemMateriais { get; set; } = new List<ListaTemMaterial>();
    }
}

