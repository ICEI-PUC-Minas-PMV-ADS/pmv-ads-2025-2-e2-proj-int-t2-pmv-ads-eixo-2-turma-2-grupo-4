using System;
using System.Collections.Generic;
using Atria.Domain.Entities.UserContext;

namespace Atria.Domain.Entities.CommunityContext;

public class Postagem
{
    public int IdPostagem { get; set; }
    public string Titulo { get; set; } = null!;
    public string Conteudo { get; set; } = null!;
    public DateTime DataCriacao { get; set; }
    public bool NoForumGeral { get; set; }
    
    // Foreign keys
    public string FkAutor { get; set; } = null!;
    public int? FkComunidade { get; set; }
    
    // Navigation properties
    public virtual Usuario Autor { get; set; } = null!;
    public virtual Comunidade? Comunidade { get; set; }
    public virtual ICollection<Comentario> Comentarios { get; private set; }
    
    public Postagem()
    {
        DataCriacao = DateTime.UtcNow;
        Comentarios = new List<Comentario>();
    }
}