using Atria.Data;
using Atria.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Atria.Controllers
{
    [Authorize]
    public class ListasLeituraController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ListasLeituraController(ApplicationDbContext context)
        {
            _context = context;
        }

        [AllowAnonymous]
        public async Task<IActionResult> Index()
        {
            return View(await _context.ListasLeitura.ToListAsync());
        }

        [AllowAnonymous]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();
            var lista = await _context.ListasLeitura
                .Include(l => l.ListaTemMateriais!)
                .ThenInclude(lm => lm.Material)
                .FirstOrDefaultAsync(l => l.Id == id);
            if (lista == null) return NotFound();
            return View(lista);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Nome,Descricao")] ListaLeitura lista)
        {
            if (ModelState.IsValid)
            {
                var userIdClaim = User.FindFirst("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier");
                if (userIdClaim == null) return Challenge();
                if (!int.TryParse(userIdClaim.Value, out var userId)) return BadRequest();

                lista.FKUsuario = userId;
                _context.Add(lista);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(lista);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();
            var lista = await _context.ListasLeitura.FindAsync(id);
            if (lista == null) return NotFound();
            return View(lista);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Nome,Descricao")] ListaLeitura lista)
        {
            if (id != lista.Id) return NotFound();
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(lista);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_context.ListasLeitura.Any(e => e.Id == lista.Id)) return NotFound();
                    else throw;
                }
                return RedirectToAction(nameof(Index));
            }
            return View(lista);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();
            var lista = await _context.ListasLeitura.Include(l => l.ListaTemMateriais).FirstOrDefaultAsync(l => l.Id == id);
            if (lista == null) return NotFound();
            return View(lista);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var lista = await _context.ListasLeitura.FindAsync(id);
            if (lista != null)
            {
                _context.ListasLeitura.Remove(lista);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }
    }
}