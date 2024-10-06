using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Drogueria_Elcafetero.Data;
using Drogueria_el_cafetero.Models;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages;
using Drogueria_Elcafetero.Models;
using Drogueria_Elcafetero.Servicios;

namespace Drogueria_Elcafetero.Controllers
{
    public class customersController : Controller
    {
        private readonly Drogueria_ElcafeteroContext _context;

        public customersController(Drogueria_ElcafeteroContext context)
        {
            _context = context;
        }

        // GET: customers
        public async Task<IActionResult> Index()
        {
            return View(await _context.customers.ToListAsync());
        }

        // GET: customers/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var customers = await _context.customers
                .FirstOrDefaultAsync(m => m.id_customer == id);
            if (customers == null)
            {
                return NotFound();
            }

            return View(customers);
        }

        // GET: customers/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: customers/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("id_customer,customer_name,telephone,email,password_hash,reset_password,token")] customers customers)
        {

            if (ModelState.IsValid)
            {
                customers.token = UtilidadServicio.GenerarToken();
                _context.Add(customers);
                var resultado = await _context.SaveChangesAsync();

                if (resultado > 0)
                {
                    //string path = HttpContext.Server.MapPath("~/Plantilla/Confirmar.html");
                    var path = Path.Combine(Directory.GetCurrentDirectory(), "Plantilla", "Confirmar.html");

                    //string content = System.IO.File.ReadAllText(path);
                    string content = System.IO.File.ReadAllText(path);

                    //string url = string.Format("{0}://{1}{2}", Request.Url.Scheme, Request.Headers["host"],"/Inicio/Confirmar?token=" + customers.token);
                    var url = $"{Request.Scheme}://{Request.Host}/Plantilla/Confirmar?token={customers.token}";

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
                    //ViewBag.Mensaje = "No se pudo crear su cuenta";
                }
                return RedirectToAction(nameof(Index));
            }
            return View(customers);
        }

        public IActionResult Registrar()
        {

            return View();
        }

        // GET: customers/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var customers = await _context.customers.FindAsync(id);
            if (customers == null)
            {
                return NotFound();
            }
            return View(customers);
        }

        // POST: customers/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("id_customer,customer_name,telephone,email,password_hash,reset_password,confirmed_password,token,confirmed")] customers customers)
        {
            if (id != customers.id_customer)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(customers);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!customersExists(customers.id_customer))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(customers);
        }

        // GET: customers/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var customers = await _context.customers
                .FirstOrDefaultAsync(m => m.id_customer == id);
            if (customers == null)
            {
                return NotFound();
            }

            return View(customers);
        }

        // POST: customers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var customers = await _context.customers.FindAsync(id);
            if (customers != null)
            {
                _context.customers.Remove(customers);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool customersExists(int id)
        {
            return _context.customers.Any(e => e.id_customer == id);
        }
    }
}
