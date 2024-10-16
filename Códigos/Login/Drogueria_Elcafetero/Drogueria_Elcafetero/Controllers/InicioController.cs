using Microsoft.AspNetCore.Mvc;
using Drogueria_el_cafetero.Models;
using Drogueria_Elcafetero.Datos;
using Drogueria_Elcafetero.Servicios;
using Drogueria_Elcafetero.Models;
using WebAppCorreo.Servicios;




namespace WebAppCorreo.Controllers
{
    public class InicioController : Controller
    {
        private readonly IWebHostEnvironment _env;
        public InicioController(IWebHostEnvironment env)
        {
            _env = env; // Inicializa el IWebHostEnvironment
        }

        // GET: Inicio
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(string email, string password_hash)
        {
            customers customers = DBcustomers.Validar(email, UtilidadServicio.ConvertirSHA256(password_hash));

            if (customers != null)
            {
                if (!customers.confirmed)
                {
                    ViewBag.Mensaje = $"Falta confirmar su cuenta. Se le envio un correo a {email}";
                }
                else if (customers.reset_password)
                {
                    ViewBag.Mensaje = $"Se ha solicitado restablecer su cuenta, favor revise su bandeja del correo {email}";
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

        public ActionResult Registrar()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Registrar(customers customers)
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
                customers.reset_password = false;
                customers.confirmed = false;

                bool respuesta = DBcustomers.Registrar(customers);

                if (respuesta)
                {
                    string path = Path.Combine(_env.ContentRootPath, "Plantilla", "Confirmar.html");

                    string content = System.IO.File.ReadAllText(path);
                    string url = $"{Request.Scheme}://{Request.Host}/Inicio/Confirmar?token={customers.token}";

                    string htmlbody = string.Format(content, customers.customer_name, url);


                    Correo correoDTO = new Correo()
                    {
                        Para = customers.email,
                        Asunto = "Correo confirmacion",
                        Contenido = htmlbody
                    };

                    bool enviado = CorreoServicio.Enviar(correoDTO);
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

        public ActionResult Confirmar(string token)
        {
            ViewBag.Respuesta = DBcustomers.Confirmar(token);
            return View();
        }

        public ActionResult Restablecer()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Restablecer(string email)
        {
            customers customers = DBcustomers.Obtener(email);
            ViewBag.Correo = email;
            if (customers != null)
            {
                bool respuesta = DBcustomers.RestablecerActualizar(true, customers.password_hash, customers.token);

                if (respuesta)
                {
                    string path = Path.Combine(_env.ContentRootPath, "Plantilla", "Restablecer.html");


                    string content = System.IO.File.ReadAllText(path);
                    string url = $"{Request.Scheme}://{Request.Host}/Inicio/Actualizar?token={customers.token}";


                    string htmlbody = string.Format(content, customers.customer_name, url);


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

            bool respuesta = DBcustomers.RestablecerActualizar(false, UtilidadServicio.ConvertirSHA256(password_hash), token);

            if (respuesta)
                ViewBag.Restablecido = true;
            else
                ViewBag.Mensaje = "No se pudo actualizar";

            return View();
        }
    }
}