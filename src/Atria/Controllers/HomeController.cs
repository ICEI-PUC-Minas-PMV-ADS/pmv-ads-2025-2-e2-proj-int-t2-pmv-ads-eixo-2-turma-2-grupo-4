using Microsoft.AspNetCore.Mvc;
using Atria.Models;
using System.Diagnostics;

namespace Atria.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            string lorem = "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Nullam in dui mauris. Vivamus hendrerit arcu sed erat molestie, non rutrum eni... (Este é um texto fictício)";

            var postagensFicticias = new List<Postagem>
            {
                new Postagem
                {
                    Id = 1,
                    Titulo = "⭐ [FIXADO] Regras da Comunidade e Como Usar o Atria",
                    Conteudo = "Bem-vindo ao Atria! Por favor, leia as regras antes de postar...",
                    DataPostagem = DateTime.Now.AddDays(-3),
                    Usuario = new ApplicationUser { UserName = "Moderador" }
                },
                new Postagem
                {
                    Id = 2,
                    Titulo = "🏆 [Mais Votado] A lista definitiva de livros para quem está começando em C#",
                    Conteudo = "Depois de 10 anos programando, essa é a minha lista pessoal...",
                    DataPostagem = DateTime.Now.AddHours(-5),
                    Usuario = new ApplicationUser { UserName = "DevSenior" }
                },
                new Postagem
                {
                    Id = 4,
                    Titulo = "O que vocês acharam de 'O Idiota' de Dostoiévski?",
                    Conteudo = lorem,
                    DataPostagem = DateTime.Now.AddMinutes(-45),
                    Usuario = new ApplicationUser { UserName = "LeitorClassico" }
                },
                new Postagem
                {
                    Id = 6,
                    Titulo = "Como manter a rotina de academia e estudos?",
                    Conteudo = lorem,
                    DataPostagem = DateTime.Now.AddMinutes(-10),
                    Usuario = new ApplicationUser { UserName = "MarombeiroDev" }
                }
            };

            // A View vai receber a lista e decidir o que fazer com ela
            return View(postagensFicticias.OrderByDescending(p => p.DataPostagem));
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}