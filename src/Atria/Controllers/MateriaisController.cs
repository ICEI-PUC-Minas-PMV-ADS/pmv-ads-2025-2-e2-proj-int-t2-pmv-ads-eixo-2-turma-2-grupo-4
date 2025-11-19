using Atria.Data;
using Atria.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Atria.Controllers
{
    [Authorize]
    public class MateriaisController : Controller
    {
        private readonly ApplicationDbContext _context;

        public MateriaisController(ApplicationDbContext context)
        {
            _context = context;
        }

        [AllowAnonymous]
        public async Task<IActionResult> Index(string? tipo, string? ordenacao)
        {
            // Buscar materiais com relacionamentos
            var query = _context.Materiais
                .Include(m => m.Criador)
                .Include(m => m.Avaliacoes)
                .AsQueryable();

            // Filtrar por tipo se especificado
            if (!string.IsNullOrEmpty(tipo))
            {
                query = query.Where(m => m.Tipo == tipo);
            }

            // Aplicar ordenação
            query = ordenacao switch
            {
                "titulo" => query.OrderBy(m => m.Titulo),
                "tipo" => query.OrderBy(m => m.Tipo).ThenBy(m => m.Titulo),
                "recentes" => query.OrderByDescending(m => m.DataCriacao),
                _ => query.OrderByDescending(m => m.DataCriacao) // Padrão: mais recentes
            };

            var materiais = await query.ToListAsync();

            // Passar parâmetros para a view via ViewBag
            ViewBag.TipoFiltro = tipo;
            ViewBag.Ordenacao = ordenacao;

            return View(materiais);
        }

        [AllowAnonymous]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();
            
            try
            {
                var material = await _context.Materiais
                    .Include(m => m.Criador)
                    .Include(m => m.Avaliacoes!)
                    .ThenInclude(a => a.Usuario)
                    .Include(m => m.Avaliacoes!)
                    .ThenInclude(a => a.Comentarios!) // Carregar comentários das avaliações
                    .ThenInclude(c => c.Usuario)
                    .FirstOrDefaultAsync(m => m.Id == id);
        
                if (material == null) return NotFound();
                
                return View(material);
            }
            catch (Exception ex)
            {
                // Se der erro ao carregar comentários (tabela não existe), carrega sem eles
                var material = await _context.Materiais
                    .Include(m => m.Criador)
                    .Include(m => m.Avaliacoes!)
                    .ThenInclude(a => a.Usuario)
                    .FirstOrDefaultAsync(m => m.Id == id);
    
                if (material == null) return NotFound();
        
                // Logar erro para debug
                Console.WriteLine($"Aviso: Não foi possível carregar comentários das avaliações. Erro: {ex.Message}");
                
                return View(material);
            }
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Titulo,Descricao,Tipo,Status")] Material material)
        {
            if (ModelState.IsValid)
            {
                var userIdClaim = User.FindFirst("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier");
                if (userIdClaim == null)
                {
                    return Challenge();
                }

                if (!int.TryParse(userIdClaim.Value, out var userId))
                {
                    return BadRequest();
                }

                material.FKUsuarioCriador = userId;
                material.DataCriacao = DateTime.UtcNow;

                // Garantir status padrão se não informado
                if (string.IsNullOrEmpty(material.Status))
                {
                    material.Status = "Pendente";
                }

                _context.Add(material);
                await _context.SaveChangesAsync();

                TempData["SuccessMessage"] = "Material criado com sucesso! Aguarde aprovação do moderador.";
                return RedirectToAction(nameof(Index));
            }
            return View(material);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();
            var material = await _context.Materiais.FindAsync(id);
            if (material == null) return NotFound();
            return View(material);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Titulo,Descricao,Tipo,Status")] Material material)
        {
            if (id != material.Id) return NotFound();
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(material);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_context.Materiais.Any(e => e.Id == material.Id)) return NotFound();
                    else throw;
                }
                return RedirectToAction(nameof(Index));
            }
            return View(material);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();
            var material = await _context.Materiais.Include(m => m.Criador).FirstOrDefaultAsync(m => m.Id == id);
            if (material == null) return NotFound();
            return View(material);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var material = await _context.Materiais.FindAsync(id);
            if (material != null)
            {
                _context.Materiais.Remove(material);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }
    }
}