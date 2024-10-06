using Microsoft.AspNetCore.Mvc;
using Drogueria_el_cafetero.Models;
using Drogueria_Elcafetero.Datos;
using Drogueria_Elcafetero.Servicios;
using Drogueria_Elcafetero.Models;

namespace Drogueria_Elcafetero.Controllers
{
    public class InicioController : Controller
    {
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login(string correo, string clave)
        {
            customers customers = DBcustomers.Validar(correo, UtilidadServicio.ConvertirSHA256(clave));

            if (customers == null)
            {
                
                if (customers.reset_password == "si")
                {
                    ViewBag.Mensaje = $"Se ha solicitado restablecer su cuenta, por favor revise su bandeja de correo {correo}";
                }
                else
                {
                    return RedirectToAction("Index", "Home");
                }
            }
            else
            {
                ViewBag.Mensaje = "No se encontraron coincidencias";
            }
            return View();
        }

        public IActionResult Registrar()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Registrar(customers customers)
        {
            if (customers.password_hash != customers.confirmed_password)
            {
                ViewBag.Nombre = customers.customer_name;
                ViewBag.Correo = customers.email;
                ViewBag.Mensaje = "Las contraseñas no coinciden";
                return View();
            }

            if (DBcustomers.Obtener(customers.email) == null)
            {
                customers.password_hash = UtilidadServicio.ConvertirSHA256(customers.password_hash);
                customers.token = UtilidadServicio.GenerarToken();
                customers.reset_password = "";

                bool respuesta = DBcustomers.Registrar(customers);

                if (respuesta)
                {
                    //string path = HttpContext.Server.MapPath("~/Plantilla/Confirmar.html");
                    var path = Path.Combine(Directory.GetCurrentDirectory(), "Plantilla", "Confirmar.html");

                    //string content = System.IO.File.ReadAllText(path);
                    string content = System.IO.File.ReadAllText(path);

                    //string url = string.Format("{0}://{1}{2}", Request.Url.Scheme, Request.Headers["host"],"/Inicio/Confirmar?token=" + customers.token);
                    var url = $"{Request.Scheme}://{Request.Host}/customers/Confirmar?token={customers.token}";

                    string htmlbody = string.Format(content, customers.customer_name, url);

                    Correo correo = new Correo()
                    {
                        Para = customers.email,
                        Asunto = "Correo confirmado",
                        Contenido = htmlbody
                    };

                    bool enviado = CorreoServicio.Enviar(correo); 
                    ViewBag.Creado = true;
                    ViewBag.Mensaje = $"Su cuenta ha sido creada. Hemos enviado un mensaje al correo {customers.email} para confirmar su cuenta";
                }
                else
                {
                    ViewBag.Mensaje = "No se pudo crear su cuenta";
                }
            }
            else
            {
                ViewBag.Mensaje = "El correo ya se encuentra registrado";
            }

            return View();
        }

        public IActionResult Confirmar(string token)
        {
            ViewBag.Respuesta = DBcustomers.Confirmar(token);
            return View();
        }

        public IActionResult Restablecer()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Restablecer(string correo)
        {
            customers customers = DBcustomers.Obtener(correo);
            ViewBag.Correo = correo;

            if (customers != null)
            {
                bool respuesta = DBcustomers.RestablecerActualizar(1,customers.password_hash, customers.token);

                if (respuesta)
                {
                    //string path = HttpContext.Server.MapPath("~/Plantilla/Restablecer.html");
                    var path = Path.Combine(Directory.GetCurrentDirectory(), "Plantilla", "Restablecer.html");

                    //string content = System.IO.File.ReadAllText(path);
                    string content = System.IO.File.ReadAllText(path);

                    //string url = string.Format("{0}://{1}{2}", Request.Url.Scheme, Request.Headers["host"], "/Inicio/Actualizar?token=" + customers.token);
                    var url = $"{Request.Scheme}://{Request.Host}/Inicio/Confirmar?token={customers.token}";


                    string htmlbody = string.Format(content, customers.customer_name, url);

                    Correo correoo = new Correo()
                    {
                        Para = customers.email,
                        Asunto = "Restablecer cuenta",
                        Contenido = htmlbody
                    };

                    bool enviado = CorreoServicio.Enviar(correoo);
                    ViewBag.Restablecido = true;
                }
                else
                {
                    ViewBag.Mensaje = "No se pudo restablecer la cuenta";
                }
            }
            else
            {
                ViewBag.Mensaje = "No se encontaron coincidencias con el correo";
            }
            return View();
        }

        public IActionResult Actualizar(string token)
        {
            ViewBag.token = token;
            return View();
        }

        [HttpPost]
        public IActionResult Actualizar(string token, string clave, string confirmarclave)
        {
            ViewBag.token = token;

            if (clave != confirmarclave)
            {
                ViewBag.Mensaje = "Las contraseñas no coinciden";
                return View();
            }

            bool respuesta = DBcustomers.RestablecerActualizar(0,UtilidadServicio.ConvertirSHA256(clave), token);

            if (respuesta)
            {
                ViewBag.Restablecido = true;
            }
            else
            {
                ViewBag.Mensaje = "No se puedo actualizar";
            }


            return View();
        }
    }
}
