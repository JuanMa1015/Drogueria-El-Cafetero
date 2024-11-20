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
                    .FromSqlRaw(@"SELECT p.id_product AS IdProduct, 
                                   p.product_name AS ProductName, 
                                   s.supplier_name AS SupplierName,
                                   c.category_name AS CategoryName, 
                                   p.price AS Price, 
                                   p.units_in_stock AS UnitsInStock, 
                                   p.expiration_date AS ExpirationDate, 
                                   p.active AS Active,
                                   p.image AS Image
                            FROM products p
                            JOIN suppliers s ON p.id_supplier = s.id_supplier
                            JOIN category c ON p.id_category = c.id_category
                            WHERE p.expiration_date >= CURRENT_DATE AND p.active = TRUE")
                .ToListAsync();

            if (detailsProducts == null)
            {
                detailsProducts = new List<detailsProduct>(); // Initialize an empty list to prevent null reference
                Console.WriteLine("No se encontraron productos.");
            }

            return View(detailsProducts);
        }

        public async Task<IActionResult> ProductosVencidos()
        {
            var productosVencidos = await _context.expiredproduct
                .FromSqlRaw(@"SELECT p.id_product AS IdProduct, 
                               p.product_name AS ProductName, 
                               s.supplier_name AS SupplierName, 
                               c.category_name AS CategoryName, 
                               p.price AS Price, 
                               p.units_in_stock AS UnitsInStock, 
                               p.expiration_date AS ExpirationDate, 
                               p.active AS Active,
                               p.image AS Image
                        FROM products p
                        JOIN suppliers s ON p.id_supplier = s.id_supplier  
                        JOIN category c ON p.id_category = c.id_category
                        WHERE p.expiration_date < CURRENT_DATE")
                .ToListAsync();

            if (productosVencidos == null || !productosVencidos.Any())
            {
                productosVencidos = new List<expiredproduct>();
                Console.WriteLine("No se encontraron productos.");
            }

            return View(productosVencidos);
        }

        public async Task<IActionResult> DesactivarProductosVencidos()
        {
            var productosVencidos = await _context.products
                .Where(p => p.expiration_date < DateTime.Now)
                .ToListAsync();

            foreach (var producto in productosVencidos)
            {
                producto.active = false;  // Desactivamos el producto
            }

            await _context.SaveChangesAsync();

            return RedirectToAction("Index", "Home");
        }

        public async Task<IActionResult> DesactivarProductosSinStock()
        {
            var productosSinStock = await _context.products
                .Where(p => p.units_in_stock == 0)
                .ToListAsync();

            foreach (var producto in productosSinStock)
            {
                producto.active = false;  // Marcamos el producto como inactivo
            }

            await _context.SaveChangesAsync();

            return RedirectToAction("Index", "Products");
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
            var categorias = _context.category
       .Select(c => new { c.id_category, c.category_name })
       .ToList();

            var proveedores = _context.suppliers
                .Select(p => new { p.id_supplier, p.supplier_name })
                .ToList();

            // Si estas listas son nulas o están vacías, arroja una excepción
            if (categorias == null || !categorias.Any())
            {
                throw new Exception("No se encontraron categorías en la base de datos.");
            }
            if (proveedores == null || !proveedores.Any())
            {
                throw new Exception("No se encontraron proveedores en la base de datos.");
            }

            // Pasar las listas a la vista a través de ViewBag
            ViewBag.Categories = new SelectList(categorias, "id_category", "category_name");
            ViewBag.Suppliers = new SelectList(proveedores, "id_supplier", "supplier_name");

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
                ViewBag.Categories = new SelectList(_context.category, "id_category", "category_name", products.id_category);
                ViewBag.Suppliers = new SelectList(_context.suppliers, "id_supplier", "supplier_name", products.id_supplier);
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

            var categorias = _context.category
          .Select(c => new { c.id_category, c.category_name })
          .ToList();

            var proveedores = _context.suppliers
          .Select(p => new { p.id_supplier, p.supplier_name })
          .ToList();

            // Pasar las listas a la vista a través de ViewBag
            ViewBag.Categories = new SelectList(categorias, "id_category", "category_name", products.id_category);
            ViewBag.Suppliers = new SelectList(proveedores, "id_supplier", "supplier_name", products.id_supplier);


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
        // Método GET para mostrar la vista de confirmación de eliminación
        [HttpGet]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            // Obtener los detalles del producto junto con los datos relacionados (Proveedor y Categoría)
            var product = await _context.detailsProduct.FromSqlRaw
                                          (@"SELECT 
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
                                              INNER JOIN 
                                                  Suppliers s ON p.id_supplier = s.id_supplier
                                              INNER JOIN 
                                                  category c ON p.id_category = c.id_category
                                              WHERE p.id_product = {0}", id)
                                          .FirstOrDefaultAsync();

            // Verificar si el producto existe
            if (product == null)
            {
                return NotFound();
            }

            // Retornar la vista con los detalles del producto
            return View(product);
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
