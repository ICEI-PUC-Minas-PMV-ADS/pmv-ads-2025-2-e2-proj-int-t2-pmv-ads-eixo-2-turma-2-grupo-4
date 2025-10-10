using Atria.Domain.Entities.UserContext;

namespace Atria.Domain.Entities.MaterialContext;

public class ListaDeLeitura
{
    public int IdListaLeitura { get; set; }
    public string Nome { get; set; } = null!;
    
    // Foreign key
    public string FkCriador { get; set; } = null!;
    
    // Navigation property
    public virtual Usuario Criador { get; set; } = null!;
    public virtual ICollection<Material> Materiais { get; private set; }
    
    public ListaDeLeitura()
    {
        Materiais = new List<Material>();
    }
}