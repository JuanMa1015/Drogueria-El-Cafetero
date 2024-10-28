using Drogueria_Elcafetero.Permisos;
using Microsoft.AspNetCore.Mvc;
using Drogueria_Elcafetero.Models;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;


namespace Drogueria_Elcafetero.Controllers
{
    public class AdminController : Controller
    {
        [PermisosRol(rol.Administrador)]
        public IActionResult AdminVM()
        {
            return View();
        }

        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            return RedirectToAction("Login", "Inicio");
        }
    }
}
