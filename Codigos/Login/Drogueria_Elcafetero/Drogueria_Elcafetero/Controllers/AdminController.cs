using Drogueria_Elcafetero.Permisos;
using Microsoft.AspNetCore.Mvc;
using Drogueria_Elcafetero.Models;


namespace Drogueria_Elcafetero.Controllers
{
    public class AdminController : Controller
    {
        [PermisosRol(Rol.Administrador)]
        public IActionResult AdminVM()
        {
            return View();
        }
    }
}
