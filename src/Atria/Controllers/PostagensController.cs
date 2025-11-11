using Atria.Data;
using Atria.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
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
       var postagens = await _context.Postagens.Include(p => p.Usuario).Include(p => p.Comunidade).ToListAsync();
     return View(postagens);
        }

     [AllowAnonymous]
    public async Task<IActionResult> Details(int? id)
   {
       if (id == null) return NotFound();
     var postagem = await _context.Postagens.Include(p => p.Usuario).Include(p => p.Comunidade).FirstOrDefaultAsync(p => p.Id == id);
    if (postagem == null) return NotFound();
    return View(postagem);
 }

        public IActionResult Create(int? comunidadeId)
        {
        ViewBag.ComunidadeId = comunidadeId;
 return View();
 }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Conteudo,FKComunidade,NoForumGeral")] Postagem postagem)
   {
   if (ModelState.IsValid)
     {
     var userIdClaim = User.FindFirst("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier");
         if (userIdClaim == null) return Challenge();
   if (!int.TryParse(userIdClaim.Value, out var userId)) return BadRequest();

     postagem.FKUsuario = userId;
    postagem.DataPostagem = DateTime.UtcNow;
        _context.Add(postagem);
   await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
       }
      return View(postagem);
        }

  public async Task<IActionResult> Edit(int? id)
        {
       if (id == null) return NotFound();
  var postagem = await _context.Postagens.FindAsync(id);
    if (postagem == null) return NotFound();
         return View(postagem);
        }

     [HttpPost]
        [ValidateAntiForgeryToken]
public async Task<IActionResult> Edit(int id, [Bind("Id,Conteudo,FKComunidade,NoForumGeral")] Postagem postagem)
  {
       if (id != postagem.Id) return NotFound();
          if (ModelState.IsValid)
   {
                try
      {
      _context.Update(postagem);
      await _context.SaveChangesAsync();
   }
        catch (DbUpdateConcurrencyException)
  {
       if (!_context.Postagens.Any(e => e.Id == postagem.Id)) return NotFound();
      else throw;
  }
           return RedirectToAction(nameof(Index));
    }
   return View(postagem);
        }

        public async Task<IActionResult> Delete(int? id)
        {
     if (id == null) return NotFound();
          var postagem = await _context.Postagens.Include(p => p.Usuario).Include(p => p.Comunidade).FirstOrDefaultAsync(p => p.Id == id);
     if (postagem == null) return NotFound();
 return View(postagem);
    }

   [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
      public async Task<IActionResult> DeleteConfirmed(int id)
        {
     var postagem = await _context.Postagens.FindAsync(id);
      if (postagem != null)
   {
    _context.Postagens.Remove(postagem);
  await _context.SaveChangesAsync();
  }
   return RedirectToAction(nameof(Index));
 }
    }
}