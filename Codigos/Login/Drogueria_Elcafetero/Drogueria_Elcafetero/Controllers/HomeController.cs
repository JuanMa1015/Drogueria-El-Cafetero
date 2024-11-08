using Drogueria_el_cafetero.Models;
using Drogueria_Elcafetero.Data;
using Drogueria_Elcafetero.Datos;
using Drogueria_Elcafetero.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProyectoTest.Logica;
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


        [Authorize(Roles = "Administrador")]
        public IActionResult Admin()
        {
            return View();
        }


        public IActionResult IndexSinLogin()
        {
            return View();
        }


        public IActionResult About()
        {
            return View();
        }

      
        public IActionResult SinPermiso()
        {
            ViewBag.Message = "Usted no cuenta con el permiso para acceder a esta página";
            return View();
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
