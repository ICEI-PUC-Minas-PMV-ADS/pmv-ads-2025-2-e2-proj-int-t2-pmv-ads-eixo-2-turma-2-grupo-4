using System;
using Atria.Domain.Entities.UserContext;

namespace Atria.Domain.Entities.CommunityContext;

public class Comentario
{
    public int IdComentario { get; set; } = default!;
    public string Conteudo { get; set; } = null!;
    public DateTime DataCriacao { get; set; }
    
    // Foreign keys
    public string FkAutor { get; set; } = null!;
    public int FkPostagem { get; set; }
    
    // Navigation properties
    public virtual Usuario Autor { get; set; } = null!;
    public virtual Postagem Postagem { get; set; } = null!;

    public Comentario()
    {
        DataCriacao = DateTime.UtcNow;
    }
}