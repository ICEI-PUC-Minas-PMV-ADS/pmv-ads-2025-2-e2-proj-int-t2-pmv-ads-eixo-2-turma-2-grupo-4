using Atria.Domain.Entities.UserContext;
using System;

namespace Atria.Domain.Entities.InteractionContext;

public class MensagemPrivada
{
    public string IdMensagem { get; set; } = null!;
    public string Conteudo { get; set; } = null!;
    public DateTime DataEnvio { get; set; }
    public bool Lida { get; set; }

    public string FkRemetente { get; set; } = null!;
    public string FkDestinatario { get; set; } = null!;

    public virtual Usuario Remetente { get; set; } = null!;
    public virtual Usuario Destinatario { get; set; } = null!;

    public MensagemPrivada()
    {
        IdMensagem = Guid.NewGuid().ToString();
        DataEnvio = DateTime.UtcNow;
        Lida = false;
    }
}