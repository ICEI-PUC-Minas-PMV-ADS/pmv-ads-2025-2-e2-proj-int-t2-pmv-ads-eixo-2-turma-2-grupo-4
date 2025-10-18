using System.ComponentModel.DataAnnotations.Schema;

namespace Atria.Models
{
    [Table("TB_LISTA_TEM_MATERIAL")]
    public class ListaTemMaterial
    {
        [Column("FK_LISTA")]
        public int FKLista { get; set; }
        public ListaLeitura? ListaLeitura { get; set; }

        [Column("FK_MATERIAL")]
        public int FKMaterial { get; set; }
        public Material? Material { get; set; }

        [Column("ORDEM")]
        public int? Ordem { get; set; }
    }
}