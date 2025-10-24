using Atria.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

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

        // GET: Profile/Index
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

      // Atualizar apenas os campos de perfil
            user.Nome = model.Nome;
            user.AreaEstudo = model.AreaEstudo;
            user.TrilhaConhecimento = model.TrilhaConhecimento;
            user.Projetos = model.Projetos;

            // Atualizar email se foi alterado
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
