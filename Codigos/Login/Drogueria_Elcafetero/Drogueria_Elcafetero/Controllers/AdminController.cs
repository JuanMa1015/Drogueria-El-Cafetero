using Microsoft.AspNetCore.Mvc;
using Drogueria_Elcafetero.Models;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;


namespace Drogueria_Elcafetero.Controllers
{
    public class AdminController : Controller
    {
        
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
