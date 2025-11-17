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

        // VERSÃO NOVA - CORRIGIDA
        [AllowAnonymous]
        public async Task<IActionResult> Index()
        {
            var listas = await _context.ListasLeitura
                .Include(l => l.Usuario) // <--- ADICIONE ESTA LINHA
                .Include(l => l.ListaTemMateriais)
                    .ThenInclude(lm => lm.Material)
                .ToListAsync();

            return View(listas);
        }

        // VERSÃO NOVA - CORRIGIDA
        [AllowAnonymous]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var lista = await _context.ListasLeitura
                .Include(l => l.Usuario) // <--- ADICIONE ESTA LINHA
                .Include(l => l.ListaTemMateriais!)
                    .ThenInclude(lm => lm.Material)
                .FirstOrDefaultAsync(l => l.Id == id);

            if (lista == null) return NotFound();
            return View(lista);
        }

        // GET: ListasLeitura/Create
        public IActionResult Create()
        {
            // 1. Buscar todos os materiais do banco
            // Estou assumindo que seu DbContext tem "Materiais" e o model "Material" tem "Titulo"
            var materiais = _context.Materiais
                                    .Select(m => new { m.Id, m.Titulo })
                                    .ToList();

            // 2. Passar a lista de materiais para a View
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

                // 2. Salve a lista PRIMEIRO para que ela receba um "Id" do banco
                await _context.SaveChangesAsync();

                // 3. Agora, crie as associações na tabela "ListaTemMaterial"
                if (materialIds != null && materialIds.Length > 0)
                {
                    foreach (var materialId in materialIds)
                    {
                        var associacao = new ListaTemMaterial
                        {
                            FKLista = lista.Id, // O ID da lista que acabamos de criar
                            FKMaterial = materialId
                            // Você pode adicionar a lógica de "Ordem" aqui se precisar
                        };
                        _context.ListaTemMateriais.Add(associacao); // Precisa de um DbSet<ListaTemMaterial> no DbContext
                    }

                    // 4. Salve as novas associações
                    await _context.SaveChangesAsync();
                }

                return RedirectToAction(nameof(Index));
            }

            // 5. Se o modelo for inválido, precisa recarregar o ViewBag!
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

            // 1. Buscar a lista E seus materiais atuais
            var lista = await _context.ListasLeitura
                .Include(l => l.ListaTemMateriais) // Carrega as associações
                .FirstOrDefaultAsync(l => l.Id == id);

            if (lista == null) return NotFound();

            // 2. Buscar TODOS os materiais
            var todosMateriais = await _context.Materiais
                                        .Select(m => new { m.Id, m.Titulo })
                                        .ToListAsync();

            // 3. Pegar os IDs dos materiais que JÁ ESTÃO na lista
            var idsMateriaisNaLista = lista.ListaTemMateriais.Select(lm => lm.FKMaterial).ToList();

            // 4. Passar para o ViewBag usando um MultiSelectList para PRÉ-SELECIONAR os itens
            ViewBag.Materiais = new MultiSelectList(todosMateriais, "Id", "Titulo", idsMateriaisNaLista);

            return View(lista);
        }

        // POST: ListasLeitura/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Nome,Descricao")] ListaLeitura lista, int[] materialIds)
        {
            if (id != lista.Id) return NotFound();

            // Não podemos usar o [Bind] simples e ModelState.IsValid
            // pois o EF precisa rastrear a entidade original para atualizar as relações M-N

            try
            {
                // 1. Carregar a lista original do banco, incluindo os materiais
                var listaOriginal = await _context.ListasLeitura
                    .Include(l => l.ListaTemMateriais)
                    .FirstOrDefaultAsync(l => l.Id == id);

                if (listaOriginal == null) return NotFound();

                // 2. Atualizar os dados simples (Nome, Descricao)
                listaOriginal.Nome = lista.Nome;
                listaOriginal.Descricao = lista.Descricao;

                // 3. Atualizar os materiais (Lógica de "Update")

                // 3a. Remover os que foram desmarcados
                var materiaisParaRemover = listaOriginal.ListaTemMateriais
                    .Where(lm => !materialIds.Contains(lm.FKMaterial)).ToList();
                _context.ListaTemMateriais.RemoveRange(materiaisParaRemover);

                // 3b. Adicionar os que foram recém-marcados
                var idsMateriaisAtuais = listaOriginal.ListaTemMateriais.Select(lm => lm.FKMaterial);
                var idsMateriaisParaAdicionar = materialIds.Where(id => !idsMateriaisAtuais.Contains(id)).ToList();

                foreach (var materialId in idsMateriaisParaAdicionar)
                {
                    _context.ListaTemMateriais.Add(new ListaTemMaterial { FKLista = id, FKMaterial = materialId });
                }

                // 4. Salvar tudo
                _context.Update(listaOriginal); // Marca a entidade principal como modificada
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.ListasLeitura.Any(e => e.Id == lista.Id)) return NotFound();
                else throw;
            }
            return RedirectToAction(nameof(Index));
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