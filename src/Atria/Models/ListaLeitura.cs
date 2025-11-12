using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Atria.Models
{
    [Table("TB_LISTA_LEITURA")]
    public class ListaLeitura
    {
        [Key]
        [Column("ID_LISTA")]
        public int Id { get; set; }

        [Column("NOME")]
        [Required]
        [MaxLength(150)]
        public string Nome { get; set; } = string.Empty;

        [Column("DESCRICAO")]
        public string? Descricao { get; set; }

        [Column("FK_USUARIO")]
        public int FKUsuario { get; set; } = 0;
        public ApplicationUser? Usuario { get; set; }

        public ICollection<ListaTemMaterial> ListaTemMateriais { get; set; } = new List<ListaTemMaterial>();
    }
}