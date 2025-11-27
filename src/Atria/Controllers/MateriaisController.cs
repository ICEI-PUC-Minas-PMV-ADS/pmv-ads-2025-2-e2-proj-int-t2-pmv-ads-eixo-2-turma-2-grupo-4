using Atria.Data;
using Atria.Models;
using Atria.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Atria.Controllers
{
    [Authorize]
    public class MateriaisController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly INotificacaoService _notificacaoService;

        public MateriaisController(ApplicationDbContext context, INotificacaoService notificacaoService)
        {
            _context = context;
            _notificacaoService = notificacaoService;
        }

        [AllowAnonymous]
        public async Task<IActionResult> Index(string? tipo, string? ordenacao)
        {
            try
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
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = "Erro ao carregar materiais: " + ex.Message;
                ViewBag.TipoFiltro = tipo;
                ViewBag.Ordenacao = ordenacao;
                return View(new List<Material>());
            }
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
                    var materialOriginal = await _context.Materiais.FindAsync(id);
                    if (materialOriginal == null) return NotFound();

                    // Capturar o status anterior para verificar mudanças
                    var statusAnterior = materialOriginal.Status;

                    // Preservar campos que não devem ser editados
                    materialOriginal.Titulo = material.Titulo;
                    materialOriginal.Descricao = material.Descricao;
                    materialOriginal.Tipo = material.Tipo;
                    materialOriginal.Status = material.Status;

                    _context.Update(materialOriginal);
                    await _context.SaveChangesAsync();

                    // Se o status mudou para Aprovado ou Rejeitado, criar notificação
                    if (statusAnterior != material.Status && 
                        (material.Status == "Aprovado" || material.Status == "Rejeitado"))
                    {
                        await _notificacaoService.CriarNotificacaoMaterialAvaliado(id, material.Status);
                    }
                     
                    TempData["SuccessMessage"] = "Material atualizado com sucesso!";
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_context.Materiais.Any(e => e.Id == material.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        TempData["ErrorMessage"] = "Erro de concorrência. O material foi modificado por outro usuário.";
                        throw;
                    }
                }
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