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
    public class discount_suppliers_invoicesController : Controller
    {
        private readonly Drogueria_ElcafeteroContext _context;

        public discount_suppliers_invoicesController(Drogueria_ElcafeteroContext context)
        {
            _context = context;
        }

        // GET: discount_suppliers_invoices
        public async Task<IActionResult> Index()
        {
            return View(await _context.discount_suppliers_invoices.ToListAsync());
        }

        // GET: discount_suppliers_invoices/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var discount_suppliers_invoices = await _context.discount_suppliers_invoices
                .FirstOrDefaultAsync(m => m.id_discount_suppliers_invoices == id);
            if (discount_suppliers_invoices == null)
            {
                return NotFound();
            }

            return View(discount_suppliers_invoices);
        }

        // GET: discount_suppliers_invoices/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: discount_suppliers_invoices/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("id_discount_suppliers_invoices,id_discount,id_supplier_invoice,discount_amount")] discount_suppliers_invoices discount_suppliers_invoices)
        {
            if (ModelState.IsValid)
            {
                _context.Add(discount_suppliers_invoices);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(discount_suppliers_invoices);
        }

        // GET: discount_suppliers_invoices/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var discount_suppliers_invoices = await _context.discount_suppliers_invoices.FindAsync(id);
            if (discount_suppliers_invoices == null)
            {
                return NotFound();
            }
            return View(discount_suppliers_invoices);
        }

        // POST: discount_suppliers_invoices/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("id_discount_suppliers_invoices,id_discount,id_supplier_invoice,discount_amount")] discount_suppliers_invoices discount_suppliers_invoices)
        {
            if (id != discount_suppliers_invoices.id_discount_suppliers_invoices)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(discount_suppliers_invoices);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!discount_suppliers_invoicesExists(discount_suppliers_invoices.id_discount_suppliers_invoices))
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
            return View(discount_suppliers_invoices);
        }

        // GET: discount_suppliers_invoices/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var discount_suppliers_invoices = await _context.discount_suppliers_invoices
                .FirstOrDefaultAsync(m => m.id_discount_suppliers_invoices == id);
            if (discount_suppliers_invoices == null)
            {
                return NotFound();
            }

            return View(discount_suppliers_invoices);
        }

        // POST: discount_suppliers_invoices/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var discount_suppliers_invoices = await _context.discount_suppliers_invoices.FindAsync(id);
            if (discount_suppliers_invoices != null)
            {
                _context.discount_suppliers_invoices.Remove(discount_suppliers_invoices);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool discount_suppliers_invoicesExists(int id)
        {
            return _context.discount_suppliers_invoices.Any(e => e.id_discount_suppliers_invoices == id);
        }
    }
}
