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
    public class categoriesController : Controller
    {
        private readonly Drogueria_ElcafeteroContext _context;

        public categoriesController(Drogueria_ElcafeteroContext context)
        {
            _context = context;
        }

        // GET: categories
        public async Task<IActionResult> Index()
        {
            return View(await _context.category.ToListAsync());
        }

        // GET: categories/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var category = await _context.category
                .FirstOrDefaultAsync(m => m.id_category == id);
            if (category == null)
            {
                return NotFound();
            }

            return View(category);
        }

        // GET: categories/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: categories/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("id_category,category_name,description,active")] category category)
        {
            if (ModelState.IsValid)
            {
                _context.Add(category);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(category);
        }

        // GET: categories/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var category = await _context.category.FindAsync(id);
            if (category == null)
            {
                return NotFound();
            }
            return View(category);
        }

        // POST: categories/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("id_category,category_name,description,active")] category category)
        {
            if (id != category.id_category)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(category);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!categoryExists(category.id_category))
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
            return View(category);
        }

        // GET: categories/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var category = await _context.category
                .FirstOrDefaultAsync(m => m.id_category == id);
            if (category == null)
            {
                return NotFound();
            }

            return View(category);
        }

        // POST: categories/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var category = await _context.category.FindAsync(id);
            if (category != null)
            {
                _context.category.Remove(category);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool categoryExists(int id)
        {
            return _context.category.Any(e => e.id_category == id);
        }

        public async Task<IActionResult> Analgesicos()
        {
            var detailsProducts = await _context.detailsProduct
                    .FromSqlRaw(@"SELECT 
                         p.id_product AS IdProduct,
                         p.product_name AS ProductName,
                         s.supplier_name AS SupplierName,
                         c.category_name AS CategoryName,
                         p.price AS Price,
                         p.units_in_stock AS UnitsInStock,
                         p.expiration_date AS ExpirationDate,
                         p.active AS Active,
                         p.image AS Image
                     FROM 
                         Products p
                     JOIN 
                         Suppliers s ON p.id_supplier = s.id_supplier
                     JOIN 
                         Category c ON p.id_category = c.id_category
                           Where c.category_name = 'Analgésicos'")

                .ToListAsync();

            if (detailsProducts == null)
            {
                detailsProducts = new List<detailsProduct>(); // Initialize an empty list to prevent null reference
                Console.WriteLine("No se encontraron productos.");
            }

            return View(detailsProducts);
        }

        public async Task<IActionResult> Laxantes()
        {
            var detailsProducts = await _context.detailsProduct
                    .FromSqlRaw(@"SELECT 
                         p.id_product AS IdProduct,
                         p.product_name AS ProductName,
                         s.supplier_name AS SupplierName,
                         c.category_name AS CategoryName,
                         p.price AS Price,
                         p.units_in_stock AS UnitsInStock,
                         p.expiration_date AS ExpirationDate,
                         p.active AS Active,
                         p.image AS Image
                     FROM 
                         Products p
                     JOIN 
                         Suppliers s ON p.id_supplier = s.id_supplier
                     JOIN 
                         Category c ON p.id_category = c.id_category
                           Where c.category_name = 'Laxantes'")

                .ToListAsync();

            if (detailsProducts == null)
            {
                detailsProducts = new List<detailsProduct>(); // Initialize an empty list to prevent null reference
                Console.WriteLine("No se encontraron productos.");
            }

            return View(detailsProducts);
        }

        public async Task<IActionResult> Antiinflamatorios()
        {
            var detailsProducts = await _context.detailsProduct
                    .FromSqlRaw(@"SELECT 
                         p.id_product AS IdProduct,
                         p.product_name AS ProductName,
                         s.supplier_name AS SupplierName,
                         c.category_name AS CategoryName,
                         p.price AS Price,
                         p.units_in_stock AS UnitsInStock,
                         p.expiration_date AS ExpirationDate,
                         p.active AS Active,
                         p.image AS Image
                     FROM 
                         Products p
                     JOIN 
                         Suppliers s ON p.id_supplier = s.id_supplier
                     JOIN 
                         Category c ON p.id_category = c.id_category
                           Where c.category_name = 'Antiinflamatorios'")

                .ToListAsync();

            if (detailsProducts == null)
            {
                detailsProducts = new List<detailsProduct>(); // Initialize an empty list to prevent null reference
                Console.WriteLine("No se encontraron productos.");
            }

            return View(detailsProducts);
        }

    }



}
