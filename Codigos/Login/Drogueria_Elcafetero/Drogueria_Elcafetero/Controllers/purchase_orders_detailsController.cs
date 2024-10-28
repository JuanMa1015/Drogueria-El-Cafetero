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
    public class purchase_orders_detailsController : Controller
    {
        private readonly Drogueria_ElcafeteroContext _context;

        public purchase_orders_detailsController(Drogueria_ElcafeteroContext context)
        {
            _context = context;
        }

        // GET: purchase_orders_details
        public async Task<IActionResult> Index()
        {
            return View(await _context.purchase_orders_details.ToListAsync());
        }

        // GET: purchase_orders_details/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var purchase_orders_details = await _context.purchase_orders_details
                .FirstOrDefaultAsync(m => m.id_order_detail == id);
            if (purchase_orders_details == null)
            {
                return NotFound();
            }

            return View(purchase_orders_details);
        }

        // GET: purchase_orders_details/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: purchase_orders_details/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("id_order_detail,id_purchase_order,id_product,amount_product,unit_price,subtotal")] purchase_orders_details purchase_orders_details)
        {
            if (ModelState.IsValid)
            {
                _context.Add(purchase_orders_details);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(purchase_orders_details);
        }

        // GET: purchase_orders_details/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var purchase_orders_details = await _context.purchase_orders_details.FindAsync(id);
            if (purchase_orders_details == null)
            {
                return NotFound();
            }
            return View(purchase_orders_details);
        }

        // POST: purchase_orders_details/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("id_order_detail,id_purchase_order,id_product,amount_product,unit_price,subtotal")] purchase_orders_details purchase_orders_details)
        {
            if (id != purchase_orders_details.id_order_detail)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(purchase_orders_details);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!purchase_orders_detailsExists(purchase_orders_details.id_order_detail))
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
            return View(purchase_orders_details);
        }

        // GET: purchase_orders_details/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var purchase_orders_details = await _context.purchase_orders_details
                .FirstOrDefaultAsync(m => m.id_order_detail == id);
            if (purchase_orders_details == null)
            {
                return NotFound();
            }

            return View(purchase_orders_details);
        }

        // POST: purchase_orders_details/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var purchase_orders_details = await _context.purchase_orders_details.FindAsync(id);
            if (purchase_orders_details != null)
            {
                _context.purchase_orders_details.Remove(purchase_orders_details);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool purchase_orders_detailsExists(int id)
        {
            return _context.purchase_orders_details.Any(e => e.id_order_detail == id);
        }
    }
}
