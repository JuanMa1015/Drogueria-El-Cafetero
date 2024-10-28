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
    public class suppliers_invoicesController : Controller
    {
        private readonly Drogueria_ElcafeteroContext _context;

        public suppliers_invoicesController(Drogueria_ElcafeteroContext context)
        {
            _context = context;
        }

        // GET: suppliers_invoices
        public async Task<IActionResult> Index()
        {
            return View(await _context.suppliers_invoices.ToListAsync());
        }

        // GET: suppliers_invoices/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var suppliers_invoices = await _context.suppliers_invoices
                .FirstOrDefaultAsync(m => m.id_supplier_invoice == id);
            if (suppliers_invoices == null)
            {
                return NotFound();
            }

            return View(suppliers_invoices);
        }

        // GET: suppliers_invoices/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: suppliers_invoices/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("id_supplier_invoice,id_purchase_order,invoice_number,issue_date,total_invoice,state")] suppliers_invoices suppliers_invoices)
        {
            if (ModelState.IsValid)
            {
                _context.Add(suppliers_invoices);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(suppliers_invoices);
        }

        // GET: suppliers_invoices/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var suppliers_invoices = await _context.suppliers_invoices.FindAsync(id);
            if (suppliers_invoices == null)
            {
                return NotFound();
            }
            return View(suppliers_invoices);
        }

        // POST: suppliers_invoices/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("id_supplier_invoice,id_purchase_order,invoice_number,issue_date,total_invoice,state")] suppliers_invoices suppliers_invoices)
        {
            if (id != suppliers_invoices.id_supplier_invoice)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(suppliers_invoices);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!suppliers_invoicesExists(suppliers_invoices.id_supplier_invoice))
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
            return View(suppliers_invoices);
        }

        // GET: suppliers_invoices/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var suppliers_invoices = await _context.suppliers_invoices
                .FirstOrDefaultAsync(m => m.id_supplier_invoice == id);
            if (suppliers_invoices == null)
            {
                return NotFound();
            }

            return View(suppliers_invoices);
        }

        // POST: suppliers_invoices/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var suppliers_invoices = await _context.suppliers_invoices.FindAsync(id);
            if (suppliers_invoices != null)
            {
                _context.suppliers_invoices.Remove(suppliers_invoices);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool suppliers_invoicesExists(int id)
        {
            return _context.suppliers_invoices.Any(e => e.id_supplier_invoice == id);
        }
    }
}
