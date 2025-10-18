using Atria.Data;
using Atria.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Atria.Controllers
{
    [Authorize]
    public class GruposEstudoController : Controller
    {
        private readonly ApplicationDbContext _context;

        public GruposEstudoController(ApplicationDbContext context)
        {
            _context = context;
        }

        [AllowAnonymous]
        public async Task<IActionResult> Index()
        {
            return View(await _context.GruposEstudo.ToListAsync());
        }

        [AllowAnonymous]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();
            var grupo = await _context.GruposEstudo.Include(g => g.Usuarios).FirstOrDefaultAsync(g => g.Id == id);
            if (grupo == null) return NotFound();
            return View(grupo);
        }

        public IActionResult Create(int? comunidadeId)
        {
            ViewBag.ComunidadeId = comunidadeId;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Nome,Descricao,FKComunidade")] GrupoEstudo grupo)
        {
            if (ModelState.IsValid)
            {
                grupo.DataCriacao = DateTime.UtcNow;
                _context.Add(grupo);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(grupo);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();
            var grupo = await _context.GruposEstudo.FindAsync(id);
            if (grupo == null) return NotFound();
            return View(grupo);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Nome,Descricao,FKComunidade")] GrupoEstudo grupo)
        {
            if (id != grupo.Id) return NotFound();
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(grupo);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_context.GruposEstudo.Any(e => e.Id == grupo.Id)) return NotFound();
                    else throw;
                }
                return RedirectToAction(nameof(Index));
            }
            return View(grupo);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();
            var grupo = await _context.GruposEstudo.FirstOrDefaultAsync(g => g.Id == id);
            if (grupo == null) return NotFound();
            return View(grupo);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var grupo = await _context.GruposEstudo.FindAsync(id);
            if (grupo != null)
            {
                _context.GruposEstudo.Remove(grupo);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }
    }
}