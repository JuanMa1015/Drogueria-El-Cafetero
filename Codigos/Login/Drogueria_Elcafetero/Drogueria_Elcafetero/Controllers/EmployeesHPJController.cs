using Drogueria_Elcafetero.Datos;
using Drogueria_Elcafetero.Models;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Newtonsoft.Json;


namespace Drogueria_Elcafetero.Controllers
{
    public class EmployeesHPJController : Controller
    {
        public IActionResult IndexEmployee()
        {
            return View();
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(string email, string password_hash)
        {
            Employees employees = new DBEmployee().EncontrarUsuarios(email, password_hash);

            if (employees.id_rol == Rol.Empleado)
            {
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, employees.employee_name),
                    new Claim(ClaimTypes.Role, "Empleado")
                };

                var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

                var authProperties = new AuthenticationProperties
                {
                    IsPersistent = true,
                    ExpiresUtc = DateTimeOffset.UtcNow.AddMinutes(5)
                };

                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity), authProperties);

                HttpContext.Session.SetString("Empleado",JsonConvert.SerializeObject(employees));

                return RedirectToAction("IndexEmployee", "EmployeesHPJ");
            }
            else 
            {
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, employees.employee_name),
                    new Claim(ClaimTypes.Role, "Administrador")
                };

                var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

                var authProperties = new AuthenticationProperties
                {
                    IsPersistent = true,
                    ExpiresUtc = DateTimeOffset.UtcNow.AddMinutes(5)
                };

                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity), authProperties);

                HttpContext.Session.SetString("Administrador", JsonConvert.SerializeObject(employees));

                return RedirectToAction("AdminVM", "Admin");
            }

            return View();
        }

        
    }
}
