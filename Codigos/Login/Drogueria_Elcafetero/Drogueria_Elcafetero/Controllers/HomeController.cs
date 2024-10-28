using Drogueria_el_cafetero.Models;
using Drogueria_Elcafetero.Datos;
using Drogueria_Elcafetero.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Security.Claims;
using Drogueria_Elcafetero.Permisos;

namespace Drogueria_Elcafetero.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {

            return View();
        }

        [PermisosRol(rol.Empleado)]
        public IActionResult Privacy()
        {
            return View();
        }

      
        public IActionResult SinPermiso()
        {
            ViewBag.Message = "Usted no cuenta con el permiso para acceder a esta página";
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
