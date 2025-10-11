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
    public virtual ICollection<MensagemPrivada> MensagensEnviadas { get; private set; }
    public virtual ICollection<MensagemPrivada> MensagensRecebidas { get; private set; }
    public virtual ICollection<ComunidadeMembro> ComunidadeMembros { get; private set; }

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
        MensagensEnviadas = new List<MensagemPrivada>();
        MensagensRecebidas = new List<MensagemPrivada>();
        ComunidadeMembros = new List<ComunidadeMembro>();
    }

    /// <summary>
    /// Publica uma nova postagem. N�o persiste no banco; apenas cria a entidade em mem�ria.
    /// </summary>
    public Postagem PublicarPostagem(string titulo, string conteudo, Comunidade? comunidade = null)
    {
        if (string.IsNullOrWhiteSpace(titulo)) throw new ArgumentException("T�tulo � obrigat�rio", nameof(titulo));
        if (string.IsNullOrWhiteSpace(conteudo)) throw new ArgumentException("Conte�do � obrigat�rio", nameof(conteudo));

        var postagem = new Postagem
        {
            Titulo = titulo,
            Conteudo = conteudo,
            FkAutor = this.IdUsuario,
            Autor = this,
            FkComunidade = comunidade?.IdComunidade
        };

        Postagens.Add(postagem);
        return postagem;
    }

    /// <summary>
    /// Cria uma nova comunidade e a adiciona � lista de comunidades criadas pelo usu�rio.
    /// </summary>
    public Comunidade CriarComunidade(string nome, string descricao, bool isForumGeral = false)
    {
        if (string.IsNullOrWhiteSpace(nome)) throw new ArgumentException("Nome da comunidade � obrigat�rio", nameof(nome));

        var comunidade = new Comunidade
        {
            Nome = nome,
            Descricao = descricao ?? string.Empty,
            IsForumGeral = isForumGeral,
            FkCriador = this.IdUsuario,
            Criador = this
        };

        ComunidadesCriadas.Add(comunidade);
        return comunidade;
    }

    /// <summary>
    /// Cria uma nova lista de leitura associada ao usu�rio.
    /// </summary>
    public ListaDeLeitura CriarListaDeLeitura(string nome)
    {
        if (string.IsNullOrWhiteSpace(nome)) throw new ArgumentException("Nome da lista � obrigat�rio", nameof(nome));

        var lista = new ListaDeLeitura
        {
            Nome = nome,
            FkCriador = this.IdUsuario,
            Criador = this
        };

        ListasDeLeitura.Add(lista);
        return lista;
    }

    /// <summary>
    /// Envia uma mensagem privada para outro usu�rio (entidade MensagemPrivada).
    /// </summary>
    public MensagemPrivada EnviarMensagem(Usuario destinatario, string conteudo)
    {
        if (destinatario == null) throw new ArgumentNullException(nameof(destinatario));
        if (string.IsNullOrWhiteSpace(conteudo)) throw new ArgumentException("Conte�do da mensagem � obrigat�rio", nameof(conteudo));

        var mensagem = new MensagemPrivada
        {
            IdMensagem = Guid.NewGuid().ToString(),
            Conteudo = conteudo,
            DataEnvio = DateTime.UtcNow,
            Lida = false,
            FkRemetente = this.IdUsuario,
            FkDestinatario = destinatario.IdUsuario,
            Remetente = this,
            Destinatario = destinatario
        };

        MensagensEnviadas.Add(mensagem);
        destinatario.MensagensRecebidas.Add(mensagem);

        return mensagem;
    }

    /// <summary>
    /// Segue outro usu�rio (apenas adiciona l�gica em mem�ria).
    /// </summary>
    public void SeguirUsuario(Usuario usuario)
    {
        if (usuario == null) throw new ArgumentNullException(nameof(usuario));
        // Comportamento de seguimento simples: poderia ser implementado com entidade de relacionamento N:M.
        // Aqui apenas gera uma notifica��o ao usu�rio seguido.
        var notificacao = new Notificacao
        {
            Conteudo = $"{this.Nome} come�ou a seguir voc�.",
            FkUsuario = usuario.IdUsuario,
            Usuario = usuario,
            DataCriacao = DateTime.UtcNow
        };

        usuario.Notificacoes.Add(notificacao);
        Notificacoes.Add(notificacao);
    }

    /// <summary>
    /// Segue uma comunidade (apenas adiciona o usu�rio � lista de participantes da comunidade em mem�ria).
    /// </summary>
    public void SeguirComunidade(Comunidade comunidade)
    {
        if (comunidade == null) throw new ArgumentNullException(nameof(comunidade));
        // O relacionamento N:M de usu�rios e comunidades n�o foi modelado como entidade aqui; essa a��o � conceitual.
        // Implementa��o concreta deve ser feita via reposit�rio/servi�o.
    }
}