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
    public class sales_detailsController : Controller
    {
        private readonly Drogueria_ElcafeteroContext _context;

        public sales_detailsController(Drogueria_ElcafeteroContext context)
        {
            _context = context;
        }

        // GET: sales_details
        public async Task<IActionResult> Index()
        {
            return View(await _context.sales_details.ToListAsync());
        }

        // GET: sales_details/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var sales_details = await _context.sales_details
                .FirstOrDefaultAsync(m => m.id_detail == id);
            if (sales_details == null)
            {
                return NotFound();
            }

            return View(sales_details);
        }

        // GET: sales_details/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: sales_details/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("id_detail,id_sale,id_product,amount_products,unit_price,subtotal")] sales_details sales_details)
        {
            if (ModelState.IsValid)
            {
                _context.Add(sales_details);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(sales_details);
        }

        // GET: sales_details/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var sales_details = await _context.sales_details.FindAsync(id);
            if (sales_details == null)
            {
                return NotFound();
            }
            return View(sales_details);
        }

        // POST: sales_details/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("id_detail,id_sale,id_product,amount_products,unit_price,subtotal")] sales_details sales_details)
        {
            if (id != sales_details.id_detail)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(sales_details);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!sales_detailsExists(sales_details.id_detail))
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
            return View(sales_details);
        }

        // GET: sales_details/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var sales_details = await _context.sales_details
                .FirstOrDefaultAsync(m => m.id_detail == id);
            if (sales_details == null)
            {
                return NotFound();
            }

            return View(sales_details);
        }

        // POST: sales_details/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var sales_details = await _context.sales_details.FindAsync(id);
            if (sales_details != null)
            {
                _context.sales_details.Remove(sales_details);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool sales_detailsExists(int id)
        {
            return _context.sales_details.Any(e => e.id_detail == id);
        }
    }
}
