using Atria.Domain.Entities.UserContext;
using Atria.Domain.Entities.MaterialContext;
using System;

namespace Atria.Domain.Entities.InteractionContext;

public class Avaliacao
{
    public int IdAvaliacao { get; set; }
    public decimal Nota { get; set; }
    public string Resenha { get; set; } = null!;
    public DateTime DataAvaliacao { get; set; }
    public TipoAvaliacao TipoAvaliacao { get; set; }
    public string? TextoEspecialista { get; set; }
    
    // Foreign keys
    public string FkAutor { get; set; } = null!;
    public int FkMaterial { get; set; }
    
    // Navigation properties
    public virtual Usuario Autor { get; set; } = null!;
    public virtual Material Material { get; set; } = null!;

    public Avaliacao()
    {
        DataAvaliacao = DateTime.UtcNow;
    }
}

public enum TipoAvaliacao
{
    Publica,
    Especialista
}