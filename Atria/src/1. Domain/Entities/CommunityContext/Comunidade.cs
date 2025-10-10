using Atria.Domain.Entities.UserContext;
using System;
using System.Collections.Generic;

namespace Atria.Domain.Entities.CommunityContext;

public class Comunidade
{
    public int IdComunidade { get; set; }
    public string Nome { get; set; } = null!;
    public string Descricao { get; set; } = null!;
    public DateTime DataCriacao { get; set; }
    public bool IsForumGeral { get; set; }
    
    // Foreign key
    public string? FkCriador { get; set; }
    
    // Navigation properties
    public virtual Usuario? Criador { get; set; }
    public virtual ICollection<GrupoDeEstudo> GruposDeEstudo { get; private set; }
    public virtual ICollection<Postagem> Postagens { get; private set; }
    
    public Comunidade()
    {
        DataCriacao = DateTime.UtcNow;
        GruposDeEstudo = new List<GrupoDeEstudo>();
        Postagens = new List<Postagem>();
    }
}