using Microsoft.AspNetCore.Mvc;
using Drogueria_el_cafetero.Models;
using Drogueria_Elcafetero.Datos;
using Drogueria_Elcafetero.Servicios;
using Drogueria_Elcafetero.Models;
using WebAppCorreo.Servicios;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Security.Claims;
using System.Configuration;

namespace WebAppCorreo.Controllers
{
    public class InicioController : Controller
    {
        private readonly IWebHostEnvironment _env;
        private readonly DBusers _dbusers;

        public InicioController(IWebHostEnvironment env, IConfiguration configuration)
        {
            _env = env; // Inicializa el IWebHostEnvironment
            _dbusers = new DBusers(configuration); // Inicializa _dbusers con la clase DBusers
        }

        // GET: Inicio
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> Login(string email, string password_hash)
        {
            users Users = _dbusers.Validar(email, UtilidadServicio.ConvertirSHA256(password_hash));

            if (Users != null)
            {
                if (!Users.confirmed)
                {
                    ViewBag.Mensaje = $"Falta confirmar su cuenta. Se le envio un correo a {email}";
                }
                else if (Users.reset_password)
                {
                    ViewBag.Mensaje = $"Se ha solicitado restablecer su cuenta, favor revise su bandeja del correo {email}";
                }
                else
                {

                    var claims = new List<Claim>
                    {
                        new Claim(ClaimTypes.Name, Users.user_name),
                        new Claim(ClaimTypes.Role, Users.rol)
                    };

                    var clasimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

                    await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(clasimsIdentity));

                    if (Users.rol == "Cliente")
                    {
                        return RedirectToAction("Index", "Home");
                    }
                    else if (Users.rol == "Administrador")
                    {
                        return RedirectToAction("Admin", "Home");
                    }
                    else if (Users.rol == "Empleado")
                    {
                        return RedirectToAction("EmployeesPage", "Employees");
                    }
                    else
                            {
                                return RedirectToAction("", "");
                            }
                   
                }

            }
            else
            {
                ViewBag.Mensaje = "No se encontraron coincidencias";
            }


            return View();
        }

        public async Task<IActionResult> Salir()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            return RedirectToAction("IndexSinLogin", "Home"); 
        }

        public ActionResult Registrar()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Registrar(users User)
        {
            if (User.password_hash != User.confirmed_password)
            {
                ViewBag.Nombre = User.user_name;
                ViewBag.Correo = User.email;
                ViewBag.Mensaje = "Las contraseñas no coinciden";
                return View();
            }

            if (_dbusers.Obtener(User.email) == null)
            {
                User.password_hash = UtilidadServicio.ConvertirSHA256(User.password_hash);
                User.token = UtilidadServicio.GenerarToken();
                User.reset_password = false;
                User.confirmed = false;

                bool respuesta = _dbusers.Registrar(User);

                if (respuesta)
                {
                    string path = Path.Combine(_env.ContentRootPath, "Plantilla", "Confirmar.html");

                    string content = System.IO.File.ReadAllText(path);
                    string url = $"{Request.Scheme}://{Request.Host}/Inicio/Confirmar?token={User.token}";

                    string htmlbody = string.Format(content, User.user_name, url);


                    Correo correoDTO = new Correo()
                    {
                        Para = User.email,
                        Asunto = "Correo confirmacion",
                        Contenido = htmlbody
                    };

                    bool enviado = CorreoServicio.Enviar(correoDTO);
                    ViewBag.Creado = true;
                    ViewBag.Mensaje = $"Su cuenta ha sido creada. Hemos enviado un mensaje al correo {User.email} para confirmar su cuenta";
                }
                else
                {
                    ViewBag.Mensaje = "No se pudo crear su cuenta";
                };



            }
            else
            {
                ViewBag.Mensaje = "El correo ya se encuentra registrado";
            }


            return View();
        }

        public ActionResult Confirmar(string token)
        {
            ViewBag.Respuesta = _dbusers.Confirmar(token);
            return View();
        }

        public ActionResult Restablecer()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Restablecer(string email)
        {
            users Users = _dbusers.Obtener(email);
            ViewBag.Correo = email;
            if (Users != null)
            {
                bool respuesta = _dbusers.RestablecerActualizar(true, Users.password_hash, Users.token);

                if (respuesta)
                {
                    string path = Path.Combine(_env.ContentRootPath, "Plantilla", "Restablecer.html");


                    string content = System.IO.File.ReadAllText(path);
                    string url = $"{Request.Scheme}://{Request.Host}/Inicio/Actualizar?token={Users.token}";


                    string htmlbody = string.Format(content, Users.user_name, url);


                    Correo correoDTO = new Correo()
                    {
                        Para = email,
                        Asunto = "Restablecer cuenta",
                        Contenido = htmlbody
                    };

                    bool enviado = CorreoServicio.Enviar(correoDTO);
                    ViewBag.Restablecido = true;
                }
                else
                {
                    ViewBag.Mensaje = "No se pudo restablecer la cuenta";
                }

            }
            else
            {
                ViewBag.Mensaje = "No se encontraron coincidencias con el correo";
            }

            return View();
        }

        public ActionResult Actualizar(string token)
        {
            ViewBag.Token = token;
            return View();
        }

        [HttpPost]
        public ActionResult Actualizar(string token, string password_hash, string confirmed_password)
        {
            ViewBag.Token = token;
            if (password_hash != confirmed_password)
            {
                ViewBag.Mensaje = "Las contraseñas no coinciden";
                return View();
            }

            bool respuesta = _dbusers.RestablecerActualizar(false, UtilidadServicio.ConvertirSHA256(password_hash), token);

            if (respuesta)
                ViewBag.Restablecido = true;
            else
                ViewBag.Mensaje = "No se pudo actualizar";

            return View();
        }
    }
}