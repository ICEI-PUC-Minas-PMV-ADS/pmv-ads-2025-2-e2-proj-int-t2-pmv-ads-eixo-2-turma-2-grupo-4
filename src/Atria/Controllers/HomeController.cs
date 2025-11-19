using Microsoft.AspNetCore.Mvc;
using Atria.Models;
using System.Diagnostics;
using System.Linq;
using System.Collections.Generic; // Necessário para List<>

namespace Atria.Controllers
{
    // ================================================================
    // CLASSE DE AJUDA PARA OS COMENTÁRIOS FICTÍCIOS
    // ================================================================
    public class ComentarioFicticio
    {
        public string Autor { get; set; } = string.Empty;
        public string Icone { get; set; } = string.Empty; // ex: "fas fa-user-check"
        public string Cor { get; set; } = string.Empty;   // ex: "text-success"
        public string Data { get; set; } = string.Empty;
        public string Texto { get; set; } = string.Empty;
    }

    public class HomeController : Controller
    {
        // O método Index() continua igual
        public IActionResult Index()
        {
            string lorem = "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Nullam in dui mauris. Vivamus hendrerit arcu sed erat molestie, non rutrum eni... (Este é um texto fictício)";

            var postagensFicticias = new List<Postagem>
            {
                new Postagem { Id = 1, Titulo = "⭐ [FIXADO] Regras da Comunidade e Como Usar o Atria", Conteudo = "Bem-vindo ao Atria! Por favor, leia as regras antes de postar...", DataPostagem = DateTime.Now.AddDays(-3), Usuario = new ApplicationUser { UserName = "Moderador" }},
                new Postagem { Id = 2, Titulo = "🏆 [Mais Votado] A lista definitiva de livros para quem está começando em C#", Conteudo = "Depois de 10 anos programando, essa é a minha lista pessoal...", DataPostagem = DateTime.Now.AddHours(-5), Usuario = new ApplicationUser { UserName = "DevSenior" }},
                new Postagem { Id = 4, Titulo = "O que vocês acharam de 'O Idiota' de Dostoiévski?", Conteudo = lorem, DataPostagem = DateTime.Now.AddMinutes(-45), Usuario = new ApplicationUser { UserName = "LeitorClassico" }},
                new Postagem { Id = 6, Titulo = "Como manter a rotina de academia e estudos?", Conteudo = lorem, DataPostagem = DateTime.Now.AddMinutes(-10), Usuario = new ApplicationUser { UserName = "MarombeiroDev" }}
            };

            return View(postagensFicticias.OrderByDescending(p => p.DataPostagem));
        }

        // ================================================================
        // MÉTODO DISCUSSÃO FICTÍCIA (ATUALIZADO)
        // ================================================================
        public IActionResult DiscussaoFicticia(int id)
        {
            // 1. Recriamos a lista de postagens falsas
            string lorem = "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Nullam in dui mauris. Vivamus hendrerit arcu sed erat molestie, non rutrum eni... (Este é um texto fictício)";
            var postagensFicticias = new List<Postagem>
            {
                new Postagem { Id = 1, Titulo = "⭐ [FIXADO] Regras da Comunidade e Como Usar o Atria", Conteudo = "Bem-vindo ao Atria! Por favor, leia as regras antes de postar...", DataPostagem = DateTime.Now.AddDays(-3), Usuario = new ApplicationUser { UserName = "Moderador" }},
                new Postagem { Id = 2, Titulo = "🏆 [Mais Votado] A lista definitiva de livros para quem está começando em C#", Conteudo = "Depois de 10 anos programando, essa é a minha lista pessoal...", DataPostagem = DateTime.Now.AddHours(-5), Usuario = new ApplicationUser { UserName = "DevSenior" }},
                new Postagem { Id = 4, Titulo = "O que vocês acharam de 'O Idiota' de Dostoiévski?", Conteudo = lorem, DataPostagem = DateTime.Now.AddMinutes(-45), Usuario = new ApplicationUser { UserName = "LeitorClassico" }},
                new Postagem { Id = 6, Titulo = "Como manter a rotina de academia e estudos?", Conteudo = lorem, DataPostagem = DateTime.Now.AddMinutes(-10), Usuario = new ApplicationUser { UserName = "MarombeiroDev" }}
            };

            // 2. Encontramos o post específico
            var post = postagensFicticias.FirstOrDefault(p => p.Id == id);
            if (post == null)
            {
                return RedirectToAction("Index");
            }

            // 3. (NOVO) Criamos a lista de comentários
            var comentarios = new List<ComentarioFicticio>();

            // 4. (NOVO) Preenchemos os comentários baseado no ID do post
            switch (post.Id)
            {
                case 1: // Regras
                    comentarios.Add(new ComentarioFicticio { Autor = "NovoMembro", Icone = "fas fa-user-plus", Cor = "text-primary", Data = "1d atrás", Texto = "Obrigado pelas regras! Ansioso para participar." });
                    break;

                case 2: // Livros C#
                    comentarios.Add(new ComentarioFicticio { Autor = "TechGuru", Icone = "fas fa-user-check", Cor = "text-success", Data = "2h atrás", Texto = "Ótimo ponto! Também adicionaria o livro 'Clean Code' a essa lista, essencial para qualquer dev." });
                    comentarios.Add(new ComentarioFicticio { Autor = "NovoDev", Icone = "fas fa-user-graduate", Cor = "text-info", Data = "1h atrás", Texto = "Valeu pela dica! Vou começar por esse do C#." });
                    break;

                case 4: // Dostoiévski
                    comentarios.Add(new ComentarioFicticio { Autor = "LeitoraVoraz", Icone = "fas fa-book-reader", Cor = "text-warning", Data = "30m atrás", Texto = "Ainda estou na metade, mas a profundidade psicológica é incrível. O Príncipe Míchkin é um personagem fascinante." });
                    break;

                case 6: // Academia e Estudos
                    comentarios.Add(new ComentarioFicticio { Autor = "FocoNoApp", Icone = "fas fa-laptop-code", Cor = "text-primary", Data = "15m atrás", Texto = "Eu uso a técnica Pomodoro para estudar e encaixo o treino logo após o almoço, quando a energia está mais baixa para codar." });
                    comentarios.Add(new ComentarioFicticio { Autor = "GymBro", Icone = "fas fa-dumbbell", Cor = "text-danger", Data = "5m atrás", Texto = "O segredo é não pular o dia de perna. O resto vem." });
                    break;
            }

            // 5. (NOVO) Enviamos os comentários para a View
            ViewBag.Comentarios = comentarios;

            // 6. Enviamos o post para a nova View
            return View(post);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
