using Atria.Data;
using Atria.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Rendering;

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
            var listas = await _context.ListasLeitura
                .Include(l => l.Usuario)
                .Include(l => l.ListaTemMateriais)
                    .ThenInclude(lm => lm.Material)
                .ToListAsync();

            return View(listas);
        }

        [AllowAnonymous]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var lista = await _context.ListasLeitura
                .Include(l => l.Usuario)
                .Include(l => l.ListaTemMateriais!)
                    .ThenInclude(lm => lm.Material)
                .FirstOrDefaultAsync(l => l.Id == id);

            if (lista == null) return NotFound();
            return View(lista);
        }

        // GET: ListasLeitura/Create
        public IActionResult Create()
        {
            var materiais = _context.Materiais
                                    .Select(m => new { m.Id, m.Titulo })
                                    .ToList();

            ViewBag.Materiais = new SelectList(materiais, "Id", "Titulo");

            return View();
        }

        // POST: ListasLeitura/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Nome,Descricao")] ListaLeitura lista, int[] materialIds)
        {
            if (ModelState.IsValid)
            {
                var userIdClaim = User.FindFirst("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier");
                if (userIdClaim == null) return Challenge();
                if (!int.TryParse(userIdClaim.Value, out var userId)) return BadRequest();

                lista.FKUsuario = userId;
                _context.Add(lista);
                await _context.SaveChangesAsync();

                if (materialIds != null && materialIds.Length > 0)
                {
                    foreach (var materialId in materialIds)
                    {
                        var associacao = new ListaTemMaterial
                        {
                            FKLista = lista.Id,
                            FKMaterial = materialId
                        };
                        _context.ListaTemMateriais.Add(associacao);
                    }
                    await _context.SaveChangesAsync();
                }

                return RedirectToAction(nameof(Index));
            }

            var materiais = _context.Materiais
                                    .Select(m => new { m.Id, m.Titulo })
                                    .ToList();
            ViewBag.Materiais = new SelectList(materiais, "Id", "Titulo");

            return View(lista);
        }

        // GET: ListasLeitura/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var lista = await _context.ListasLeitura
                .Include(l => l.ListaTemMateriais)
                .FirstOrDefaultAsync(l => l.Id == id);

            if (lista == null) return NotFound();

            // --- VERIFICAÇÃO DE SEGURANÇA ---
            var userIdClaim = User.FindFirst("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier");
            if (userIdClaim == null) return Challenge();
            int.TryParse(userIdClaim.Value, out var userId);

            if (lista.FKUsuario != userId)
            {
                return Forbid(); // Retorna erro 403 se não for o dono
            }
            // --------------------------------

            var todosMateriais = await _context.Materiais
                                        .Select(m => new { m.Id, m.Titulo })
                                        .ToListAsync();

            var idsMateriaisNaLista = lista.ListaTemMateriais.Select(lm => lm.FKMaterial).ToList();
            ViewBag.Materiais = new MultiSelectList(todosMateriais, "Id", "Titulo", idsMateriaisNaLista);

            return View(lista);
        }

        // POST: ListasLeitura/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Nome,Descricao")] ListaLeitura lista, int[] materialIds)
        {
            if (id != lista.Id) return NotFound();

            // --- VERIFICAÇÃO DE SEGURANÇA (POST) ---
            var listaOriginalCheck = await _context.ListasLeitura.AsNoTracking().FirstOrDefaultAsync(l => l.Id == id);
            if (listaOriginalCheck == null) return NotFound();

            var userIdClaim = User.FindFirst("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier");
            if (userIdClaim == null) return Challenge();
            int.TryParse(userIdClaim.Value, out var userId);

            if (listaOriginalCheck.FKUsuario != userId)
            {
                return Forbid();
            }
            // ---------------------------------------

            try
            {
                var listaOriginal = await _context.ListasLeitura
                    .Include(l => l.ListaTemMateriais)
                    .FirstOrDefaultAsync(l => l.Id == id);

                if (listaOriginal == null) return NotFound();

                listaOriginal.Nome = lista.Nome;
                listaOriginal.Descricao = lista.Descricao;

                var materiaisParaRemover = listaOriginal.ListaTemMateriais
                    .Where(lm => !materialIds.Contains(lm.FKMaterial)).ToList();
                _context.ListaTemMateriais.RemoveRange(materiaisParaRemover);

                var idsMateriaisAtuais = listaOriginal.ListaTemMateriais.Select(lm => lm.FKMaterial);
                var idsMateriaisParaAdicionar = materialIds.Where(id => !idsMateriaisAtuais.Contains(id)).ToList();

                foreach (var materialId in idsMateriaisParaAdicionar)
                {
                    _context.ListaTemMateriais.Add(new ListaTemMaterial { FKLista = id, FKMaterial = materialId });
                }

                _context.Update(listaOriginal);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.ListasLeitura.Any(e => e.Id == lista.Id)) return NotFound();
                else throw;
            }
            return RedirectToAction(nameof(Index));
        }

        // GET: ListasLeitura/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var lista = await _context.ListasLeitura
                .Include(l => l.ListaTemMateriais)
                .FirstOrDefaultAsync(l => l.Id == id);

            if (lista == null) return NotFound();

            // --- VERIFICAÇÃO DE SEGURANÇA ---
            var userIdClaim = User.FindFirst("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier");
            if (userIdClaim == null) return Challenge();
            int.TryParse(userIdClaim.Value, out var userId);

            if (lista.FKUsuario != userId)
            {
                return Forbid();
            }
            // --------------------------------

            return View(lista);
        }

        // POST: ListasLeitura/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var lista = await _context.ListasLeitura.FindAsync(id);
            if (lista != null)
            {
                // --- VERIFICAÇÃO DE SEGURANÇA ---
                var userIdClaim = User.FindFirst("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier");
                if (userIdClaim == null) return Challenge();
                int.TryParse(userIdClaim.Value, out var userId);

                if (lista.FKUsuario != userId)
                {
                    return Forbid();
                }
                // --------------------------------

                _context.ListasLeitura.Remove(lista);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }
    }
}