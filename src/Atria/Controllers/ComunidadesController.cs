using Atria.Data;
using Atria.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Atria.Controllers
{
    [Authorize]
    public class ComunidadesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ComunidadesController(ApplicationDbContext context)
        {
            _context = context;
        }

        [AllowAnonymous]
        public async Task<IActionResult> Index()
        {
            return View(await _context.Comunidades.ToListAsync());
        }

        // --- MÉTODO DETAILS CORRIGIDO (SEM WARNINGS) ---
        [AllowAnonymous]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var comunidade = await _context.Comunidades
                // O sinal '!' (exclamação) remove o aviso CS8620
                .Include(c => c.Postagens!)
                    .ThenInclude(p => p.Usuario)
                .Include(c => c.Postagens!)
                    .ThenInclude(p => p.Comentarios)
                .Include(c => c.GruposEstudo)
                .FirstOrDefaultAsync(c => c.Id == id);

            if (comunidade == null) return NotFound();

            // Ordenação feita em memória
            if (comunidade.Postagens != null)
            {
                comunidade.Postagens = comunidade.Postagens
                    .OrderByDescending(p => p.DataPostagem)
                    .ToList();
            }

            return View(comunidade);
        }
        // --------------------------------

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Nome,Descricao")] Comunidade comunidade)
        {
            if (ModelState.IsValid)
            {
                comunidade.DataCriacao = DateTime.UtcNow;
                _context.Add(comunidade);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(comunidade);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();
            var comunidade = await _context.Comunidades.FindAsync(id);
            if (comunidade == null) return NotFound();
            return View(comunidade);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Nome,Descricao")] Comunidade comunidade)
        {
            if (id != comunidade.Id) return NotFound();
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(comunidade);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_context.Comunidades.Any(e => e.Id == comunidade.Id)) return NotFound();
                    else throw;
                }
                return RedirectToAction(nameof(Index));
            }
            return View(comunidade);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();
            var comunidade = await _context.Comunidades.FirstOrDefaultAsync(c => c.Id == id);
            if (comunidade == null) return NotFound();
            return View(comunidade);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var comunidade = await _context.Comunidades.FindAsync(id);
            if (comunidade != null)
            {
                _context.Comunidades.Remove(comunidade);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }
    }
}