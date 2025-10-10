using System;
using System.Collections.Generic;

namespace Atria.Domain.Entities.CommunityContext;

public class GrupoDeEstudo
{
    public int IdGrupoEstudo { get; set; }
    public string Nome { get; set; } = null!;
    public DateTime DataCriacao { get; set; }
    
    // Foreign key
    public int FkComunidade { get; set; }
    
    // Navigation property
    public virtual Comunidade Comunidade { get; set; } = null!;

    public GrupoDeEstudo()
    {
        DataCriacao = DateTime.UtcNow;
    }
}