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
            if (!materialId.HasValue || materialId.Value == 0)
            {
                TempData["ErrorMessage"] = "ID do material não foi especificado. Por favor, acesse esta página a partir dos detalhes de um material.";
                return RedirectToAction("Index", "Materiais");
            }

            // Verificar se o material existe
            var material = _context.Materiais.Find(materialId.Value);
            if (material == null)
            {
                TempData["ErrorMessage"] = $"Material com ID {materialId} não foi encontrado.";
                return RedirectToAction("Index", "Materiais");
            }

            ViewBag.MaterialId = materialId;
            ViewBag.MaterialTitulo = material.Titulo;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Nota,Resenha,FKMaterial")] Avaliacao avaliacao)
        {
            if (!ModelState.IsValid)
            {
                TempData["ErrorMessage"] = "Há erros no formulário. Verifique os campos.";
                return RedirectToAction("Details", "Materiais", new { id = avaliacao.FKMaterial });
            }

            var userIdClaim = User.FindFirst("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier");
            if (userIdClaim == null)
            {
                TempData["ErrorMessage"] = "Você precisa estar logado para avaliar materiais.";
                return RedirectToAction("Details", "Materiais", new { id = avaliacao.FKMaterial });
            }

            if (!int.TryParse(userIdClaim.Value, out var userId))
            {
                TempData["ErrorMessage"] = "Erro ao identificar usuário.";
                return RedirectToAction("Details", "Materiais", new { id = avaliacao.FKMaterial });
            }

            try
            {
                // Verificar se usuário já avaliou este material
                var avaliacaoExistente = await _context.Avaliacoes
                    .FirstOrDefaultAsync(a => a.FKUsuario == userId && a.FKMaterial == avaliacao.FKMaterial);

                if (avaliacaoExistente != null)
                {
                    TempData["ErrorMessage"] = "Você já avaliou este material. Edite sua avaliação existente.";
                    return RedirectToAction("Details", "Materiais", new { id = avaliacao.FKMaterial });
                }

                avaliacao.FKUsuario = userId;
                avaliacao.TipoAvaliacao = "Comum"; // Definir tipo padrão
                _context.Add(avaliacao);
                await _context.SaveChangesAsync();

                TempData["SuccessMessage"] = "Avaliação criada com sucesso! Obrigado por compartilhar sua opinião.";
                return RedirectToAction("Details", "Materiais", new { id = avaliacao.FKMaterial });
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Erro ao salvar avaliação: {ex.Message}";
                return RedirectToAction("Details", "Materiais", new { id = avaliacao.FKMaterial });
            }
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