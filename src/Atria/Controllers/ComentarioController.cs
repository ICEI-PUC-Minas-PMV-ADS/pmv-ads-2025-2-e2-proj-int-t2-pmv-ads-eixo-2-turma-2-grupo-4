using Atria.Data;
using Atria.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Atria.Controllers
{
    [Authorize]
    public class ComentarioController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ComentarioController(ApplicationDbContext context)
        {
            _context = context;
        }

        [AllowAnonymous]
        public async Task<IActionResult> Index()
        {
            var comentarios = await _context.Comentarios
                .Include(c => c.Usuario)
                .Include(c => c.Postagem)
                .ToListAsync();
            return View(comentarios);
        }

        [AllowAnonymous]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();
            var comentario = await _context.Comentarios
                .Include(c => c.Usuario)
                .Include(c => c.Postagem)
                .FirstOrDefaultAsync(c => c.Id == id);
            if (comentario == null) return NotFound();
            return View(comentario);
        }

        // GET: Comentario/Create?postagemId=1
        public IActionResult Create(int? postagemId)
        {
            ViewBag.PostagemId = postagemId;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Conteudo,FKPostagem")] Comentario comentario)
        {
            if (ModelState.IsValid)
            {
                var userIdClaim = User.FindFirst("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier");
                if (userIdClaim == null) return Challenge();
                if (!int.TryParse(userIdClaim.Value, out var userId)) return BadRequest();

                comentario.FKUsuario = userId;
                comentario.DataComentario = DateTime.UtcNow;

                _context.Add(comentario);

                // Atualiza visibilidade da postagem relacionada
                var postagem = await _context.Postagens.FindAsync(comentario.FKPostagem);
                if (postagem != null)
                {
                    postagem.SetVisibleOnGeral();
                    _context.Update(postagem);
                }

                await _context.SaveChangesAsync();
                return RedirectToAction("Details", "Postagens", new { id = comentario.FKPostagem });
            }
            return View(comentario);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();
            var comentario = await _context.Comentarios.FindAsync(id);
            if (comentario == null) return NotFound();
            return View(comentario);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Conteudo,FKPostagem")] Comentario comentario)
        {
            if (id != comentario.Id) return NotFound();
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(comentario);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_context.Comentarios.Any(e => e.Id == comentario.Id)) return NotFound();
                    else throw;
                }
                return RedirectToAction("Details", "Postagens", new { id = comentario.FKPostagem });
            }
            return View(comentario);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();
            var comentario = await _context.Comentarios
                .Include(c => c.Usuario)
                .Include(c => c.Postagem)
                .FirstOrDefaultAsync(c => c.Id == id);
            if (comentario == null) return NotFound();
            return View(comentario);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var comentario = await _context.Comentarios.FindAsync(id);
            if (comentario != null)
            {
                var postagemId = comentario.FKPostagem;
                _context.Comentarios.Remove(comentario);
                await _context.SaveChangesAsync();
                return RedirectToAction("Details", "Postagens", new { id = postagemId });
            }
            return RedirectToAction("Index");
        }
    }
}
