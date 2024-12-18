using Drogueria_el_cafetero.Models;
using Drogueria_Elcafetero.Data;
using Drogueria_Elcafetero.Datos;
using Drogueria_Elcafetero.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using System.Security.Claims;

namespace Drogueria_Elcafetero.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly Drogueria_ElcafeteroContext _context;

        public HomeController(Drogueria_ElcafeteroContext context, ILogger<HomeController> logger)
        {
            _context = context;
            _logger = logger;
        }

        [Authorize(Roles = "Cliente")]
        public async Task<IActionResult> Index()
        {
        //    var productos = await _context.products
        //.Where(p => p.expiration_date >= DateTime.Now && p.active == true)
        //.ToListAsync();
            var productossinunidades = await _context.products
        .Where(p => p.units_in_stock > 0 && p.active == true)
         .ToListAsync();

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
                            WHERE p.units_in_stock > 0 AND p.active = TRUE")
                .ToListAsync();

            if (detailsProducts == null)
            {
                detailsProducts = new List<detailsProduct>(); // Initialize an empty list to prevent null reference
                Console.WriteLine("No se encontraron productos.");
            }

            return View(detailsProducts);
        }


        [Authorize(Roles = "Administrador")]
        public IActionResult Admin()
        {
            return View();
        }

        public async Task<IActionResult> Search(string query)
        {
            var products = await _context.detailsProduct
                .Where(p => p.ProductName.Contains(query) || p.CategoryName.Contains(query))
                .ToListAsync();

            return View("Index", products);
        }

        public async Task<IActionResult> IndexSinLogin()
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


        public IActionResult About()
        {
            return View();
        }

      
        public IActionResult SinPermiso()
        {
            ViewBag.Message = "Usted no cuenta con el permiso para acceder a esta p�gina";
            return View();
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
