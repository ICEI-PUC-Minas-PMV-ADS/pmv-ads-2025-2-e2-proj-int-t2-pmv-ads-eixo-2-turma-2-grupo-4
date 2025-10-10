using System;
using System.Collections.Generic;
using Atria.Domain.Entities.CommunityContext;
using Atria.Domain.Entities.InteractionContext;
using Atria.Domain.Entities.MaterialContext;
using Atria.Domain.Enums;

namespace Atria.Domain.Entities.UserContext;

public class Usuario
{
    public string IdUsuario { get; set; } = null!;
    public string Nome { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string SenhaHash { get; set; } = null!;
    public TipoUsuario TipoUsuario { get; set; }
    public DateTime DataCadastro { get; set; }
    public string? Matricula { get; set; }
    public string? AreaAtuacao { get; set; }

    // Navigation properties
    public virtual ICollection<Comunidade> ComunidadesCriadas { get; private set; }
    public virtual ICollection<Postagem> Postagens { get; private set; }
    public virtual ICollection<Comentario> Comentarios { get; private set; }
    public virtual ICollection<Avaliacao> Avaliacoes { get; private set; }
    public virtual ICollection<ListaDeLeitura> ListasDeLeitura { get; private set; }
    public virtual ICollection<Material> MateriaisCadastrados { get; private set; }
    public virtual ICollection<Notificacao> Notificacoes { get; private set; }
    
    public Usuario()
    {
        IdUsuario = Guid.NewGuid().ToString();
        DataCadastro = DateTime.UtcNow;
        ComunidadesCriadas = new List<Comunidade>();
        Postagens = new List<Postagem>();
        Comentarios = new List<Comentario>();
        Avaliacoes = new List<Avaliacao>();
        ListasDeLeitura = new List<ListaDeLeitura>();
        MateriaisCadastrados = new List<Material>();
        Notificacoes = new List<Notificacao>();
    }
}