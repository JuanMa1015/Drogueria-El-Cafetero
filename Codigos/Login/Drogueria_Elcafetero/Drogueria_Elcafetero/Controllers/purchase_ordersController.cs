using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Drogueria_Elcafetero.Data;
using Drogueria_Elcafetero.Models;
using Drogueria_Elcafetero.Permisos;

namespace Drogueria_Elcafetero.Controllers
{
    [PermisosRol(Rol.Administrador)]
    public class purchase_ordersController : Controller
    {
        private readonly Drogueria_ElcafeteroContext _context;

        public purchase_ordersController(Drogueria_ElcafeteroContext context)
        {
            _context = context;
        }

        // GET: Purchase_orders
        public async Task<IActionResult> Index()
        {
            return View(await _context.purchase_orders.ToListAsync());
        }

        // GET: Purchase_orders/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var purchase_orders = await _context.purchase_orders
                .FirstOrDefaultAsync(m => m.id_purchase_order == id);
            if (purchase_orders == null)
            {
                return NotFound();
            }

            return View(purchase_orders);
        }

        // GET: Purchase_orders/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Purchase_orders/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("id_purchase_order,id_supplier,order_date,total_order,state")] purchase_orders purchase_orders)
        {
            if (ModelState.IsValid)
            {
                _context.Add(purchase_orders);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(purchase_orders);
        }

        // GET: Purchase_orders/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var purchase_orders = await _context.purchase_orders.FindAsync(id);
            if (purchase_orders == null)
            {
                return NotFound();
            }
            return View(purchase_orders);
        }

        // POST: Purchase_orders/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("id_purchase_order,id_supplier,order_date,total_order,state")] purchase_orders purchase_orders)
        {
            if (id != purchase_orders.id_purchase_order)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(purchase_orders);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!Purchase_ordersExists(purchase_orders.id_purchase_order))
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
            return View(purchase_orders);
        }

        // GET: Purchase_orders/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var purchase_orders = await _context.purchase_orders
                .FirstOrDefaultAsync(m => m.id_purchase_order == id);
            if (purchase_orders == null)
            {
                return NotFound();
            }

            return View(purchase_orders);
        }

        // POST: Purchase_orders/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var purchase_orders = await _context.purchase_orders.FindAsync(id);
            if (purchase_orders != null)
            {
                _context.purchase_orders.Remove(purchase_orders);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool Purchase_ordersExists(int id)
        {
            return _context.purchase_orders.Any(e => e.id_purchase_order == id);
        }
    }
}
