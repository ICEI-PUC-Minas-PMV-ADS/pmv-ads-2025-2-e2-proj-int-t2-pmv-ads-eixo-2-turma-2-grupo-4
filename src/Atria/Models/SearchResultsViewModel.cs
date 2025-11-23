using Atria.Models;

namespace Atria.Models
{
    public class SearchResultsViewModel
    {
        public string Termo { get; set; } = string.Empty;
        public string Filtro { get; set; } = "todos"; // todos, usuarios, comunidades, posts

        // Listas de resultados
        public List<ApplicationUser> Usuarios { get; set; } = new List<ApplicationUser>();
        public List<Comunidade> Comunidades { get; set; } = new List<Comunidade>();
        public List<GrupoEstudo> Grupos { get; set; } = new List<GrupoEstudo>();
        public List<Postagem> Postagens { get; set; } = new List<Postagem>();

        // Propriedade auxiliar para saber se achou algo
        public bool TemResultados => Usuarios.Any() || Comunidades.Any() || Grupos.Any() || Postagens.Any();
    }
}