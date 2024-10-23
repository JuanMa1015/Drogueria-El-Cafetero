using Microsoft.AspNetCore.Mvc;

namespace Drogueria_Elcafetero.Controllers
{
    public class AdminController : Controller
    {
        public IActionResult AdminVM()
        {
            return View();
        }
    }
}
