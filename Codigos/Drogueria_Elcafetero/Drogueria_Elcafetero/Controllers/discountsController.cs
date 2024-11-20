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
    public class discountsController : Controller
    {
        private readonly Drogueria_ElcafeteroContext _context;

        public discountsController(Drogueria_ElcafeteroContext context)
        {
            _context = context;
        }

        // GET: discounts
        public async Task<IActionResult> Index()
        {
            return View(await _context.discount.ToListAsync());
        }

        // GET: discounts/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var discount = await _context.discount
                .FirstOrDefaultAsync(m => m.id_discount == id);
            if (discount == null)
            {
                return NotFound();
            }

            return View(discount);
        }

        // GET: discounts/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: discounts/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("id_discount,id_supplier_invoice,description,discount_type,discount_value,star_date,end_date,conditions")] discount discount)
        {
            if (ModelState.IsValid)
            {
                if (discount.start_date.Kind == DateTimeKind.Unspecified && discount.start_date.Kind == DateTimeKind.Unspecified)
                {
                    discount.start_date = DateTime.SpecifyKind(discount.start_date, DateTimeKind.Utc);
                    discount.end_date = DateTime.SpecifyKind(discount.end_date, DateTimeKind.Utc);

                }
                else
                {
                    discount.start_date = discount.start_date.ToUniversalTime();
                    discount.end_date = discount.end_date.ToUniversalTime();
                }
                _context.Add(discount);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(discount);
        }

        // GET: discounts/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var discount = await _context.discount.FindAsync(id);
            if (discount == null)
            {
                return NotFound();
            }
            return View(discount);
        }

        // POST: discounts/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("id_discount,id_supplier_invoice,description,discount_type,discount_value,star_date,end_date,conditions")] discount discount)
        {
            if (id != discount.id_discount)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(discount);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!discountExists(discount.id_discount))
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
            return View(discount);
        }

        // GET: discounts/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var discount = await _context.discount
                .FirstOrDefaultAsync(m => m.id_discount == id);
            if (discount == null)
            {
                return NotFound();
            }

            return View(discount);
        }

        // POST: discounts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var discount = await _context.discount.FindAsync(id);
            if (discount != null)
            {
                _context.discount.Remove(discount);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool discountExists(int id)
        {
            return _context.discount.Any(e => e.id_discount == id);
        }
    }
}
