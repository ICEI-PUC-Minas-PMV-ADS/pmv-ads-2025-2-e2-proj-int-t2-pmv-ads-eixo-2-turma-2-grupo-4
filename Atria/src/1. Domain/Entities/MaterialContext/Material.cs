using Atria.Domain.Entities.UserContext;
using Atria.Domain.Entities.InteractionContext;
using Atria.Domain.Entities.CommunityContext;
using System.Collections.Generic;

namespace Atria.Domain.Entities.MaterialContext;

public abstract class Material
{
    public int IdMaterial { get; set; }
    public string Titulo { get; set; } = null!;
    public string Autor { get; set; } = null!;
    public int AnoPublicacao { get; set; }
    public string Status { get; set; } = null!;
    public TipoMaterial TipoMaterial { get; set; }
    
    // Foreign key - permitir nulo para compatibilidade com ON DELETE SET NULL
    public string? FkProfessorCadastro { get; set; }

    // Optional: link material to a community
    public int? FkComunidade { get; set; }

    // Navigation property
    public virtual Usuario? ProfessorCadastro { get; set; }
    public virtual Comunidade? Comunidade { get; set; }
    public virtual ICollection<Avaliacao> Avaliacoes { get; private set; }
    public virtual ICollection<ListaDeLeitura> ListasDeLeitura { get; private set; }

    protected Material()
    {
        Status = "Pendente"; // Default status
        Avaliacoes = new List<Avaliacao>();
        ListasDeLeitura = new List<ListaDeLeitura>();
    }
}

public enum TipoMaterial
{
    Livro,
    Artigo
}