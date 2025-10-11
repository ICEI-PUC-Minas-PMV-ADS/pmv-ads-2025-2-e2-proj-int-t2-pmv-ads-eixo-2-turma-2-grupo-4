using Atria.Domain.Entities.UserContext;
using System;

namespace Atria.Domain.Entities.CommunityContext;

public class ComunidadeMembro
{
    public int Id { get; set; }
    public string UsuarioId { get; set; } = null!;
    public int ComunidadeId { get; set; }

    // Roles within the community
    public bool IsAdmin { get; set; }
    public bool IsModAdmin { get; set; }

    // Invitation / pending state
    public bool IsPending { get; set; }
    public string? InvitedBy { get; set; }

    public virtual Usuario Usuario { get; set; } = null!;
    public virtual Comunidade Comunidade { get; set; } = null!;

    public DateTime JoinedAt { get; set; }

    public ComunidadeMembro()
    {
        JoinedAt = DateTime.UtcNow;
        IsPending = false;
    }
}