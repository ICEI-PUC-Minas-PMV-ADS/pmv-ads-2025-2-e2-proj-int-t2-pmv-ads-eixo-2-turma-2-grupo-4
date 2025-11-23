using Atria.Data;
using Atria.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

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
            try
            {
                var postagens = await _context.Postagens
                    .Include(p => p.Usuario)
                    .Include(p => p.Comunidade)
                    .Include(p => p.GrupoEstudo)
                    .Where(p => p.Titulo != null && p.Conteudo != null)
                    .OrderByDescending(p => p.DataPostagem)
                    .ToListAsync();

                // Carregar comunidades para o filtro
                var comunidades = await _context.Comunidades
                    .OrderBy(c => c.Nome)
                    .ToListAsync();

                ViewBag.Comunidades = comunidades;

                return View(postagens);
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = "Erro ao carregar postagens: " + ex.Message;
                ViewBag.Comunidades = new List<Comunidade>();
                return View(new List<Postagem>());
            }
        }

        [AllowAnonymous]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                TempData["ErrorMessage"] = "ID da postagem não fornecido.";
                return RedirectToAction(nameof(Index));
            }

            try
            {
                var postagem = await _context.Postagens
                    .Include(p => p.Usuario)
                    .Include(p => p.Comunidade)
                    .Include(p => p.GrupoEstudo)
                    .Include(p => p.Comentarios!)
                        .ThenInclude(c => c.Usuario)
                    .Include(p => p.Comentarios!)
                        .ThenInclude(c => c.Respostas!)
                            .ThenInclude(r => r.Usuario)
                    .FirstOrDefaultAsync(p => p.Id == id);

                if (postagem == null)
                {
                    TempData["ErrorMessage"] = "Postagem não encontrada.";
                    return RedirectToAction(nameof(Index));
                }

                return View(postagem);
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Erro ao carregar postagem: " + ex.Message;
                return RedirectToAction(nameof(Index));
            }
        }

        // GET: Postagens/Create
        public async Task<IActionResult> Create(int? comunidadeId, int? grupoId)
        {
            ViewBag.ComunidadeId = comunidadeId;
            ViewBag.GrupoId = grupoId;

            try
            {
                // Carregar listas para dropdowns
                var comunidades = await _context.Comunidades
                    .OrderBy(c => c.Nome)
                    .ToListAsync();

                var grupos = await _context.GruposEstudo
                    .OrderBy(g => g.Nome)
                    .ToListAsync();

                ViewBag.Comunidades = new SelectList(comunidades, "Id", "Nome", comunidadeId);
                ViewBag.Grupos = new SelectList(grupos, "Id", "Nome", grupoId);

                return View();
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = "Erro ao carregar dados: " + ex.Message;
                ViewBag.Comunidades = new SelectList(new List<Comunidade>(), "Id", "Nome");
                ViewBag.Grupos = new SelectList(new List<GrupoEstudo>(), "Id", "Nome");
                return View();
            }
        }

        // POST: Postagens/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Titulo,Conteudo,FKComunidade,FKGrupo,NoForumGeral")] Postagem postagem)
        {
            try
            {
                // Obter o ID do usuário logado
                var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
                if (string.IsNullOrEmpty(userIdClaim) || !int.TryParse(userIdClaim, out int userId))
                {
                    ViewBag.ErrorMessage = "Usuário não identificado. Faça login novamente.";
                    await PopulateDropdowns(postagem.FKComunidade, postagem.FKGrupo);
                    return View(postagem);
                }

                // Validar campos obrigatórios
                if (string.IsNullOrWhiteSpace(postagem.Titulo))
                {
                    ModelState.AddModelError("Titulo", "O título é obrigatório.");
                }

                if (string.IsNullOrWhiteSpace(postagem.Conteudo))
                {
                    ModelState.AddModelError("Conteudo", "O conteúdo é obrigatório.");
                }

                // Converter valores vazios para null
                if (postagem.FKComunidade == 0)
                {
                    postagem.FKComunidade = null;
                }

                if (postagem.FKGrupo == 0)
                {
                    postagem.FKGrupo = null;
                }

                if (ModelState.IsValid)
                {
                    // Definir dados da postagem
                    postagem.FKUsuario = userId;
                    postagem.DataPostagem = DateTime.UtcNow;

                    // Aplicar regra de visibilidade no fórum geral
                    postagem.SetVisibleOnGeral();

                    _context.Add(postagem);
                    await _context.SaveChangesAsync();

                    TempData["SuccessMessage"] = "Postagem criada com sucesso!";
                    return RedirectToAction(nameof(Index));
                }

                // Se houver erros de validação, recarregar os dropdowns
                await PopulateDropdowns(postagem.FKComunidade, postagem.FKGrupo);
                return View(postagem);
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = "Erro ao criar postagem: " + ex.Message;
                await PopulateDropdowns(postagem.FKComunidade, postagem.FKGrupo);
                return View(postagem);
            }
        }

        // GET: Postagens/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var postagem = await _context.Postagens.FindAsync(id);
            if (postagem == null)
            {
                return NotFound();
            }

            // Carregar listas para edição
            await PopulateDropdowns(postagem.FKComunidade, postagem.FKGrupo);

            return View(postagem);
        }

        // POST: Postagens/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Titulo,Conteudo,FKComunidade,FKGrupo,NoForumGeral")] Postagem postagemEditada)
        {
            if (id != postagemEditada.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    // Buscar postagem original do banco para preservar FK_USUARIO e DATA_POSTAGEM
                    var postagemOriginal = await _context.Postagens.FindAsync(id);
                    if (postagemOriginal == null)
                    {
                        return NotFound();
                    }

                    // Atualizar APENAS os campos editáveis
                    postagemOriginal.Titulo = postagemEditada.Titulo;
                    postagemOriginal.Conteudo = postagemEditada.Conteudo;
                    postagemOriginal.FKComunidade = postagemEditada.FKComunidade == 0 ? null : postagemEditada.FKComunidade;
                    postagemOriginal.FKGrupo = postagemEditada.FKGrupo == 0 ? null : postagemEditada.FKGrupo;
                    postagemOriginal.NoForumGeral = postagemEditada.NoForumGeral;

                    // Aplicar regra de visibilidade
                    postagemOriginal.SetVisibleOnGeral();

                    _context.Update(postagemOriginal);
                    await _context.SaveChangesAsync();

                    TempData["SuccessMessage"] = "Postagem atualizada com sucesso!";
                    return RedirectToAction(nameof(Details), new { id = postagemOriginal.Id });
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PostagemExists(postagemEditada.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                catch (Exception ex)
                {
                    ViewBag.ErrorMessage = "Erro ao atualizar postagem: " + ex.Message;
                }
            }

            // Recarregar listas em caso de erro
            await PopulateDropdowns(postagemEditada.FKComunidade, postagemEditada.FKGrupo);
            return View(postagemEditada);
        }

        // GET: Postagens/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var postagem = await _context.Postagens
                .Include(p => p.Usuario)
                .Include(p => p.Comunidade)
                .Include(p => p.GrupoEstudo)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (postagem == null)
            {
                return NotFound();
            }

            return View(postagem);
        }

        // POST: Postagens/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                var postagem = await _context.Postagens.FindAsync(id);
                if (postagem != null)
                {
                    _context.Postagens.Remove(postagem);
                    await _context.SaveChangesAsync();
                    TempData["SuccessMessage"] = "Postagem removida com sucesso!";
                }
                else
                {
                    TempData["ErrorMessage"] = "Postagem não encontrada.";
                }
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Erro ao remover postagem: " + ex.Message;
            }

            return RedirectToAction(nameof(Index));
        }

        // Método auxiliar para popular os dropdowns
        private async Task PopulateDropdowns(int? comunidadeId = null, int? grupoId = null)
        {
            var comunidades = await _context.Comunidades
                .OrderBy(c => c.Nome)
                .ToListAsync();

            ViewBag.Comunidades = new SelectList(comunidades, "Id", "Nome", comunidadeId);

            var grupos = await _context.GruposEstudo
                .OrderBy(g => g.Nome)
                .ToListAsync();

            ViewBag.Grupos = new SelectList(grupos, "Id", "Nome", grupoId);
        }

        // Método auxiliar para verificar se a postagem existe
        private bool PostagemExists(int id)
        {
            return _context.Postagens.Any(e => e.Id == id);
        }
    }
}