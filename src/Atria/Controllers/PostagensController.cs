using Atria.Data;
using Atria.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace Atria.Controllers
{
    [Authorize]
    public class PostagensController : Controller
    {
        private readonly ApplicationDbContext _context;

        public PostagensController(ApplicationDbContext context)
        {
            _context = context;
        }

        [AllowAnonymous]
        public async Task<IActionResult> Index()
        {
            try
            {
                var postagens = await _context.Postagens
                    .Include(p => p.Usuario)
                    .Include(p => p.Comunidade)
                    .Include(p => p.GrupoEstudo)
                    .Where(p => p.Titulo != null && p.Conteudo != null)
                    .OrderByDescending(p => p.DataPostagem)
                    .ToListAsync();

                // Carregar comunidades para o filtro
                var comunidades = await _context.Comunidades
                    .OrderBy(c => c.Nome)
                    .ToListAsync();

                ViewBag.Comunidades = comunidades;

                return View(postagens);
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = "Erro ao carregar postagens: " + ex.Message;
                ViewBag.Comunidades = new List<Comunidade>();
                return View(new List<Postagem>());
            }
        }
    }
}