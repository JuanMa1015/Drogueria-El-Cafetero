using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Drogueria_Elcafetero.Data;
using Drogueria_Elcafetero.Models;

namespace Drogueria_Elcafetero.Controllers
{
    public class suppliers_productsController : Controller
    {
        private readonly Drogueria_ElcafeteroContext _context;

        public suppliers_productsController(Drogueria_ElcafeteroContext context)
        {
            _context = context;
        }

        // GET: suppliers_products
        public async Task<IActionResult> Index()
        {
            return View(await _context.suppliers_products.ToListAsync());
        }

        // GET: suppliers_products/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var suppliers_products = await _context.suppliers_products
                .FirstOrDefaultAsync(m => m.id_supplier_product == id);
            if (suppliers_products == null)
            {
                return NotFound();
            }

            return View(suppliers_products);
        }

        // GET: suppliers_products/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: suppliers_products/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("id_supplier_product,id_product,id_supplier,price,agreement_date")] suppliers_products suppliers_products)
        {
            if (ModelState.IsValid)
            {
                if (suppliers_products.agreement_date.Kind == DateTimeKind.Unspecified)
                {
                    suppliers_products.agreement_date = DateTime.SpecifyKind(suppliers_products.agreement_date, DateTimeKind.Utc);

                }
                else
                {
                    suppliers_products.agreement_date = suppliers_products.agreement_date.ToUniversalTime();
                }
                _context.Add(suppliers_products);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(suppliers_products);
        }

        // GET: suppliers_products/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var suppliers_products = await _context.suppliers_products.FindAsync(id);
            if (suppliers_products == null)
            {
                return NotFound();
            }
            return View(suppliers_products);
        }

        // POST: suppliers_products/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("id_supplier_product,id_product,id_supplier,price,agreement_date")] suppliers_products suppliers_products)
        {
            if (id != suppliers_products.id_supplier_product)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(suppliers_products);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!suppliers_productsExists(suppliers_products.id_supplier_product))
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
            return View(suppliers_products);
        }

        // GET: suppliers_products/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var suppliers_products = await _context.suppliers_products
                .FirstOrDefaultAsync(m => m.id_supplier_product == id);
            if (suppliers_products == null)
            {
                return NotFound();
            }

            return View(suppliers_products);
        }

        // POST: suppliers_products/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var suppliers_products = await _context.suppliers_products.FindAsync(id);
            if (suppliers_products != null)
            {
                _context.suppliers_products.Remove(suppliers_products);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool suppliers_productsExists(int id)
        {
            return _context.suppliers_products.Any(e => e.id_supplier_product == id);
        }
    }
}
