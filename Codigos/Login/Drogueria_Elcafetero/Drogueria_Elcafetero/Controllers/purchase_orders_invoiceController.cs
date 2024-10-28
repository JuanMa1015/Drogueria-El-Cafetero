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
    public class purchase_orders_invoiceController : Controller
    {
        private readonly Drogueria_ElcafeteroContext _context;

        public purchase_orders_invoiceController(Drogueria_ElcafeteroContext context)
        {
            _context = context;
        }

        // GET: purchase_orders_invoice
        public async Task<IActionResult> Index()
        {
            return View(await _context.purchase_orders_invoice.ToListAsync());
        }

        // GET: purchase_orders_invoice/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var purchase_orders_invoice = await _context.purchase_orders_invoice
                .FirstOrDefaultAsync(m => m.id_purchase_invoice == id);
            if (purchase_orders_invoice == null)
            {
                return NotFound();
            }

            return View(purchase_orders_invoice);
        }

        // GET: purchase_orders_invoice/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: purchase_orders_invoice/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("id_purchase_invoice,id_purchase_order,invoice_number,issue_date,total_invoice,state")] purchase_orders_invoice purchase_orders_invoice)
        {
            if (ModelState.IsValid)
            {
                if (purchase_orders_invoice.issue_date.Kind == DateTimeKind.Unspecified)
                {
                    purchase_orders_invoice.issue_date = DateTime.SpecifyKind(purchase_orders_invoice.issue_date, DateTimeKind.Utc);

                }
                else
                {
                    purchase_orders_invoice.issue_date = purchase_orders_invoice.issue_date.ToUniversalTime();
                }
                _context.Add(purchase_orders_invoice);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(purchase_orders_invoice);
        }

        // GET: purchase_orders_invoice/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var purchase_orders_invoice = await _context.purchase_orders_invoice.FindAsync(id);
            if (purchase_orders_invoice == null)
            {
                return NotFound();
            }
            return View(purchase_orders_invoice);
        }

        // POST: purchase_orders_invoice/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("id_purchase_invoice,id_purchase_order,invoice_number,issue_date,total_invoice,state")] purchase_orders_invoice purchase_orders_invoice)
        {
            if (id != purchase_orders_invoice.id_purchase_invoice)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(purchase_orders_invoice);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!purchase_orders_invoiceExists(purchase_orders_invoice.id_purchase_invoice))
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
            return View(purchase_orders_invoice);
        }

        // GET: purchase_orders_invoice/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var purchase_orders_invoice = await _context.purchase_orders_invoice
                .FirstOrDefaultAsync(m => m.id_purchase_invoice == id);
            if (purchase_orders_invoice == null)
            {
                return NotFound();
            }

            return View(purchase_orders_invoice);
        }

        // POST: purchase_orders_invoice/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var purchase_orders_invoice = await _context.purchase_orders_invoice.FindAsync(id);
            if (purchase_orders_invoice != null)
            {
                _context.purchase_orders_invoice.Remove(purchase_orders_invoice);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool purchase_orders_invoiceExists(int id)
        {
            return _context.purchase_orders_invoice.Any(e => e.id_purchase_invoice == id);
        }
    }
}
