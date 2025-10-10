using Atria.Domain.Entities.UserContext;
using System;

namespace Atria.Domain.Entities.InteractionContext;

public class Notificacao
{
    public int IdNotificacao { get; set; }
    public string Conteudo { get; set; } = null!;
    public DateTime DataCriacao { get; set; }
    
    // Foreign key
    public string FkUsuario { get; set; } = null!;
    
    // Navigation property
    public virtual Usuario Usuario { get; set; } = null!;

    public Notificacao()
    {
        DataCriacao = DateTime.UtcNow;
    }
}