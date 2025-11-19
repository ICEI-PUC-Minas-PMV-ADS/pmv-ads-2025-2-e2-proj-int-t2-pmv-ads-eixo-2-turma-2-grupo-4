using Atria.Data;
using Atria.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Atria.Controllers
{
    [Authorize]
    public class AvaliacoesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AvaliacoesController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var avaliacoes = await _context.Avaliacoes.Include(a => a.Usuario).Include(a => a.Material).ToListAsync();
            return View(avaliacoes);
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();
            var avaliacao = await _context.Avaliacoes.Include(a => a.Usuario).Include(a => a.Material).FirstOrDefaultAsync(a => a.Id == id);
            if (avaliacao == null) return NotFound();
            return View(avaliacao);
        }

        public IActionResult Create(int? materialId)
        {
            ViewBag.MaterialId = materialId;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Nota,TipoAvaliacao,Resenha,FKMaterial")] Avaliacao avaliacao)
        {
            if (ModelState.IsValid)
            {
                var userIdClaim = User.FindFirst("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier");
                if (userIdClaim == null) return Challenge();
                if (!int.TryParse(userIdClaim.Value, out var userId)) return BadRequest();

                avaliacao.FKUsuario = userId;
                _context.Add(avaliacao);
                await _context.SaveChangesAsync();
                return RedirectToAction("Details", "Materiais", new { id = avaliacao.FKMaterial });
            }
            return View(avaliacao);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();
            var avaliacao = await _context.Avaliacoes.FindAsync(id);
            if (avaliacao == null) return NotFound();
            return View(avaliacao);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Nota,TipoAvaliacao,Resenha")] Avaliacao avaliacao)
        {
            if (id != avaliacao.Id) return NotFound();
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(avaliacao);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_context.Avaliacoes.Any(e => e.Id == avaliacao.Id)) return NotFound();
                    else throw;
                }
                return RedirectToAction(nameof(Index));
            }
            return View(avaliacao);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();
            var avaliacao = await _context.Avaliacoes.Include(a => a.Usuario).Include(a => a.Material).FirstOrDefaultAsync(a => a.Id == id);
            if (avaliacao == null) return NotFound();
            return View(avaliacao);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var avaliacao = await _context.Avaliacoes.FindAsync(id);
            if (avaliacao != null)
            {
                _context.Avaliacoes.Remove(avaliacao);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }
    }
}