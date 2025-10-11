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
    public virtual ICollection<ComunidadeMembro> Membros { get; private set; }
    
    public Comunidade()
    {
        DataCriacao = DateTime.UtcNow;
        GruposDeEstudo = new List<GrupoDeEstudo>();
        Postagens = new List<Postagem>();
        Membros = new List<ComunidadeMembro>();
    }

    public ComunidadeMembro AddMember(Usuario usuario, bool isAdmin = false, bool isModAdmin = false)
    {
        var member = new ComunidadeMembro
        {
            UsuarioId = usuario.IdUsuario,
            ComunidadeId = this.IdComunidade,
            Usuario = usuario,
            Comunidade = this,
            IsAdmin = isAdmin,
            IsModAdmin = isModAdmin
        };

        Membros.Add(member);
        return member;
    }
}