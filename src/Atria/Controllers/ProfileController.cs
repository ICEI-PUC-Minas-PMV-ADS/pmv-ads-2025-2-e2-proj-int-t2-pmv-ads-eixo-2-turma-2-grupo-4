using Atria.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Atria.Controllers
{
    [Authorize]
    public class ProfileController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ILogger<ProfileController> _logger;

        public ProfileController(
            UserManager<ApplicationUser> userManager,
            ILogger<ProfileController> logger)
        {
            _userManager = userManager;
            _logger = logger;
        }

        // GET: Profile/Index (Meu Perfil)
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound();
            }

            var model = new ViewProfileViewModel
            {
                Id = user.Id,
                Nome = user.Nome,
                Email = user.Email ?? string.Empty,
                AreaEstudo = user.AreaEstudo,
                TrilhaConhecimento = user.TrilhaConhecimento,
                Projetos = user.Projetos,
                TipoUsuario = user.TipoUsuario,
                DataCadastro = user.DataCadastro
            };

            return View(model);
        }

        // GET: Profile/Pesquisar
        [HttpGet]
        public async Task<IActionResult> Pesquisar(string termo)
        {
            if (string.IsNullOrWhiteSpace(termo))
            {
                return View(new List<ApplicationUser>());
            }

            var usuarios = await _userManager.Users
                .Where(u => u.Nome.Contains(termo))
                .OrderBy(u => u.Nome)
                .Take(20)
                .ToListAsync();

            ViewBag.Termo = termo;
            return View(usuarios);
        }

        // GET: Profile/Visualizar/5
        [HttpGet]
        public async Task<IActionResult> Visualizar(int id)
        {
            var user = await _userManager.Users.FirstOrDefaultAsync(u => u.Id == id);

            if (user == null) return NotFound();

            var meuUser = await _userManager.GetUserAsync(User);
            bool souEu = meuUser != null && meuUser.Id == id;

            if (souEu) return RedirectToAction(nameof(Index));

            var model = new ViewProfileViewModel
            {
                Id = user.Id,
                Nome = user.Nome,
                Email = user.Email ?? string.Empty,
                AreaEstudo = user.AreaEstudo,
                TrilhaConhecimento = user.TrilhaConhecimento,
                Projetos = user.Projetos,
                TipoUsuario = user.TipoUsuario,
                DataCadastro = user.DataCadastro
            };

            ViewBag.SouEu = souEu;

            return View(model);
        }

        // GET: Profile/Edit
        [HttpGet]
        public async Task<IActionResult> Edit()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound();
            }

            var model = new EditProfileViewModel
            {
                Nome = user.Nome,
                Email = user.Email ?? string.Empty,
                AreaEstudo = user.AreaEstudo,
                TrilhaConhecimento = user.TrilhaConhecimento,
                Projetos = user.Projetos,
                TipoUsuario = user.TipoUsuario
            };

            return View(model);
        }

        // POST: Profile/Edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(EditProfileViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound();
            }

            user.Nome = model.Nome;
            user.AreaEstudo = model.AreaEstudo;
            user.TrilhaConhecimento = model.TrilhaConhecimento;
            user.Projetos = model.Projetos;

            if (user.Email != model.Email)
            {
                user.Email = model.Email;
                user.UserName = model.Email;
            }

            var result = await _userManager.UpdateAsync(user);
            if (result.Succeeded)
            {
                _logger.LogInformation("Usuário {Email} atualizou o perfil com sucesso", user.Email);
                TempData["SuccessMessage"] = "Perfil atualizado com sucesso!";
                return RedirectToAction(nameof(Index));
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }

            return View(model);
        }
    }
}