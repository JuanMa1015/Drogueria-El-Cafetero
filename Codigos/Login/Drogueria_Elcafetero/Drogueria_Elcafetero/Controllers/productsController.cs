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
    public class productsController : Controller
    {
        private readonly Drogueria_ElcafeteroContext _context;

        public productsController(Drogueria_ElcafeteroContext context)
        {
            _context = context;
        }


        // GET: products
        public async Task<IActionResult> Index()
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
                         Category c ON p.id_category = c.id_category")
                .ToListAsync();

            if (detailsProducts == null)
            {
                detailsProducts = new List<detailsProduct>(); // Initialize an empty list to prevent null reference
                Console.WriteLine("No se encontraron productos.");
            }

            return View(detailsProducts);
        }

        // GET: products/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var products = await _context.products
                .FirstOrDefaultAsync(m => m.id_product == id);
            if (products == null)
            {
                return NotFound();
            }

            return View(products);
        }

        // GET: products/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: products/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("id_product,product_name,price,units_in_stock,id_supplier,expiration_date,active,image,id_category")] products products)
        {
                if (ModelState.IsValid)
                {
                    products.expiration_date = products.expiration_date.ToUniversalTime();

                    if (products.expiration_date.Kind == DateTimeKind.Unspecified)
                    {
                        products.expiration_date = DateTime.SpecifyKind(products.expiration_date, DateTimeKind.Utc);

                    }
                    else
                    {
                        products.expiration_date = products.expiration_date.ToUniversalTime();
                    }                
                _context.Add(products);
                await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
            
            return View(products);
        }

        // GET: products/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var products = await _context.products.FindAsync(id);
            if (products == null)
            {
                return NotFound();
            }
            return View(products);
        }

        // POST: products/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("id_product,product_name,price,units_in_stock,id_supplier,expiration_date,active,image,id_category")] products products)
        {
            if (id != products.id_product)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                products.expiration_date = products.expiration_date.ToUniversalTime();

                if (products.expiration_date.Kind == DateTimeKind.Unspecified)
                {
                    products.expiration_date = DateTime.SpecifyKind(products.expiration_date, DateTimeKind.Utc);

                }
                else
                {
                    products.expiration_date = products.expiration_date.ToUniversalTime();
                }
                try
                {
                    _context.Update(products);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!productsExists(products.id_product))
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
            return View(products);
        }

        // GET: products/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var products = await _context.products
                .FirstOrDefaultAsync(m => m.id_product == id);
            if (products == null)
            {
                return NotFound();
            }

            return View(products);
        }

        // POST: products/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var products = await _context.products.FindAsync(id);
            if (products != null)
            {
                _context.products.Remove(products);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool productsExists(int id)
        {
            return _context.products.Any(e => e.id_product == id);
        }
    }
}
