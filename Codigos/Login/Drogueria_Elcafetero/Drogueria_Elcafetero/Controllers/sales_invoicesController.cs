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
    public class sales_invoicesController : Controller
    {
        private readonly Drogueria_ElcafeteroContext _context;

        public sales_invoicesController(Drogueria_ElcafeteroContext context)
        {
            _context = context;
        }

        // GET: sales_invoices
        public async Task<IActionResult> Index()
        {
            return View(await _context.sales_invoices.ToListAsync());
        }

        // GET: sales_invoices/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var sales_invoices = await _context.sales_invoices
                .FirstOrDefaultAsync(m => m.id_invoice == id);
            if (sales_invoices == null)
            {
                return NotFound();
            }

            return View(sales_invoices);
        }

        // GET: sales_invoices/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: sales_invoices/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("id_invoice,id_sale,issue_number,issue_date,total_invoice,state")] sales_invoices sales_invoices)
        {
            if (ModelState.IsValid)
            {
                if (sales_invoices.issue_date.Kind == DateTimeKind.Unspecified)
                {
                    sales_invoices.issue_date = DateTime.SpecifyKind(sales_invoices.issue_date, DateTimeKind.Utc);

                }
                else
                {
                    sales_invoices.issue_date = sales_invoices.issue_date.ToUniversalTime();
                }
                _context.Add(sales_invoices);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(sales_invoices);
        }

        // GET: sales_invoices/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var sales_invoices = await _context.sales_invoices.FindAsync(id);
            if (sales_invoices == null)
            {
                return NotFound();
            }
            return View(sales_invoices);
        }

        // POST: sales_invoices/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("id_invoice,id_sale,issue_number,issue_date,total_invoice,state")] sales_invoices sales_invoices)
        {
            if (id != sales_invoices.id_invoice)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(sales_invoices);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!sales_invoicesExists(sales_invoices.id_invoice))
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
            return View(sales_invoices);
        }

        // GET: sales_invoices/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var sales_invoices = await _context.sales_invoices
                .FirstOrDefaultAsync(m => m.id_invoice == id);
            if (sales_invoices == null)
            {
                return NotFound();
            }

            return View(sales_invoices);
        }

        // POST: sales_invoices/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var sales_invoices = await _context.sales_invoices.FindAsync(id);
            if (sales_invoices != null)
            {
                _context.sales_invoices.Remove(sales_invoices);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool sales_invoicesExists(int id)
        {
            return _context.sales_invoices.Any(e => e.id_invoice == id);
        }
    }
}
