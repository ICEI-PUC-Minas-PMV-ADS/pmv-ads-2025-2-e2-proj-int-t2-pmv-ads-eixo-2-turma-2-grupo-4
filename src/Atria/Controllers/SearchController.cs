using Atria.Data;
using Atria.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Atria.Controllers
{
    [AllowAnonymous]
    public class SearchController : Controller
    {
        private readonly ApplicationDbContext _context;

        public SearchController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Search/Index?q=termo&filter=todos
        public async Task<IActionResult> Index(string q, string filter = "todos")
        {
            var model = new SearchResultsViewModel
            {
                Termo = q,
                Filtro = filter
            };

            if (string.IsNullOrWhiteSpace(q))
            {
                return View(model);
            }

            q = q.Trim();

            // 1. Buscar USUÁRIOS
            if (filter == "todos" || filter == "usuarios")
            {
                model.Usuarios = await _context.Users
                    // O '!' avisa: "Confia em mim, Nome e Email são obrigatórios no banco"
                    .Where(u => u.Nome!.Contains(q) || u.Email!.Contains(q))
                    .OrderBy(u => u.Nome)
                    .Take(10)
                    .ToListAsync();
            }

            // 2. Buscar COMUNIDADES
            if (filter == "todos" || filter == "comunidades")
            {
                model.Comunidades = await _context.Comunidades
                    // Descricao pode ser nula, então verificamos 'c.Descricao != null' antes
                    .Where(c => c.Nome!.Contains(q) || (c.Descricao != null && c.Descricao.Contains(q)))
                    .OrderBy(c => c.Nome)
                    .Take(10)
                    .ToListAsync();
            }

            // 3. Buscar GRUPOS DE ESTUDO
            if (filter == "todos" || filter == "grupos")
            {
                model.Grupos = await _context.GruposEstudo
                    // Mesma coisa: verifica se Descricao existe antes de buscar
                    .Where(g => g.Nome!.Contains(q) || (g.Descricao != null && g.Descricao.Contains(q)))
                    .OrderBy(g => g.Nome)
                    .Take(10)
                    .ToListAsync();
            }

            // 4. Buscar POSTAGENS
            if (filter == "todos" || filter == "posts")
            {
                model.Postagens = await _context.Postagens
                    .Include(p => p.Usuario)
                    // Titulo e Conteudo são obrigatórios, usamos '!'
                    .Where(p => p.Titulo!.Contains(q) || p.Conteudo!.Contains(q))
                    .OrderByDescending(p => p.DataPostagem)
                    .Take(10)
                    .ToListAsync();
            }

            return View(model);
        }
    }
}