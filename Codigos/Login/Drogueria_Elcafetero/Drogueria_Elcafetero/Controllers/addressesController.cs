using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Drogueria_Elcafetero.Data;
using Drogueria_Elcafetero.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.Blazor;


namespace Drogueria_Elcafetero.Controllers
{
    [Authorize(Roles = "Administrador")]

    public class addressesController : Controller
    {

        private readonly Drogueria_ElcafeteroContext _context;

        public addressesController(Drogueria_ElcafeteroContext context)
        {
            _context = context;
        }


        // GET: addresses
        public async Task<IActionResult> Index()
        {
            var detailsadress = await _context.detailsadressess
                    .FromSqlRaw(@"SELECT 
                                    a.id_address AS IdAddress,
                                    a.city_name AS CityName,
                                    a.description Description,
                                    s.supplier_name AS SupplierName  
                                FROM 
                                    Address a
                                JOIN 
                                    Suppliers s ON a.id_supplier = s.id_supplier
                                JOIN 
                                    City_towns c ON a.city_name = c.city_name
                                ")
                .ToListAsync();

            if (detailsadress == null)
            {
                detailsadress = new List<detailsadressess>(); // Initialize an empty list to prevent null reference
                Console.WriteLine("No se encontraron productos.");
            }

            return View(detailsadress);
        }

        // GET: addresses/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var adress = await _context.address
                .FirstOrDefaultAsync(m => m.id_address == id);
            if (adress == null)
            {
                return NotFound();
            }

            return View(adress);
        }





        // GET: addresses/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: addresses/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("id_address,city_name,department_name,description,id_supplier")] address address)
        {
            if (ModelState.IsValid)
            {
                _context.Add(address);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(address);
        }

        // GET: addresses/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var address = await _context.address.FindAsync(id);
            if (address == null)
            {
                return NotFound();
            }
            return View(address);
        }

        // POST: addresses/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("id_address,city_name,department_name,description,id_supplier")] address address)
        {
            if (id != address.id_address)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(address);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!addressExists(address.id_address))
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
            return View(address);
        }

        // GET: addresses/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var address = await _context.address
                .FirstOrDefaultAsync(m => m.id_address == id);
            if (address == null)
            {
                return NotFound();
            }

            return View(address);
        }

        // POST: addresses/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var address = await _context.address.FindAsync(id);
            if (address != null)
            {
                _context.address.Remove(address);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool addressExists(int id)
        {
            return _context.address.Any(e => e.id_address == id);
        }
    }
}
