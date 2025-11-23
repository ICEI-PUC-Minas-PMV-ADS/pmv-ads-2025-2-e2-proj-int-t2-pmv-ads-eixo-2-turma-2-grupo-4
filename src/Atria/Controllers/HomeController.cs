using Microsoft.AspNetCore.Mvc;
using Atria.Models;
using Atria.Data; // Importante para o DbContext
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace Atria.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _context;

        // Injeção de dependência do Banco de Dados
        public HomeController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            // Busca postagens reais do banco
            // Lógica: NoForumGeral == true OU (Sem Comunidade E Sem Grupo)
            var postagensGerais = await _context.Postagens
                .Include(p => p.Usuario)
                .Include(p => p.Comunidade)    // <--- NECESSÁRIO PARA A TAG DE COMUNIDADE
                .Include(p => p.GrupoEstudo)
                .Include(p => p.Comentarios!)             // <--- MUDANÇA AQUI
                .ThenInclude(c => c.Usuario)          // <--- ADICIONE ISSO (Para ver quem comentou)
                .Where(p => p.NoForumGeral == true || (p.FKComunidade == 0 && p.FKGrupo == 0))
                .OrderByDescending(p => p.DataPostagem)
                .ToListAsync();

            return View(postagensGerais);
        }

        // O método DiscussaoFicticia foi REMOVIDO pois usaremos Postagens/Details real

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}