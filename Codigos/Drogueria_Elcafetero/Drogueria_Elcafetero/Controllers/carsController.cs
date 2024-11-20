using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Drogueria_Elcafetero.Data;
using Drogueria_Elcafetero.Models;
using Npgsql;
using System.Data;
using Drogueria_el_cafetero.Models;

namespace Drogueria_Elcafetero.Controllers
{
    public class carsController : Controller
    {
        private readonly Drogueria_ElcafeteroContext _context;

        public carsController(Drogueria_ElcafeteroContext context)
        {
            _context = context;
        }


        [HttpPost]
        public IActionResult UpdateQuantity(int id_car, int nuevaCantidad)
        {
            try
            {
                using (var connection = (NpgsqlConnection)_context.Database.GetDbConnection())
                {
                    connection.Open();

                    using (var checkStockCommand = new NpgsqlCommand(@"
                    SELECT p.units_in_stock
                    FROM car c 
                    JOIN products p ON c.id_product = p.id_product 
                    WHERE c.id_car = @id_car", connection))
                    {
                        checkStockCommand.Parameters.AddWithValue("carrito_id", id_car);
                        int unidadesStock = (int)checkStockCommand.ExecuteScalar();

                        if (nuevaCantidad > unidadesStock)
                        {
                            return Json(new { success = false, message = "No hay suficiente stock disponible" });
                        }
                    }

                    using (var command = new NpgsqlCommand("SELECT actualizar_cantidad_producto(@id_car, @nueva_cantidad)", connection))
                    {
                        command.CommandType = CommandType.Text;
                        command.Parameters.AddWithValue("id_car", id_car);
                        command.Parameters.AddWithValue("nueva_cantidad", nuevaCantidad);

                        // Ejecuta la función y obtiene el nuevo precio
                        var nuevoPrecio = (decimal)command.ExecuteScalar();

                        return Json(new { success = true, precio = nuevoPrecio });
                    }
                }
            }
            catch (Exception ex)
            {
                // Log de error y respuesta de fallo
                Console.WriteLine($"Error al actualizar cantidad: {ex.Message}");
                return Json(new { success = false, error = ex.Message });
            }
        }


        [HttpPost]
        public async Task<IActionResult> AgregarAlCarrito(int idProducto)
        {
            var producto = await _context.products.FindAsync(idProducto);
            if (producto == null)
            {
                TempData["Error"] = "Producto no encontrado.";
                return RedirectToAction("Index", "Home");
            }

            if (producto.units_in_stock <= 0)
            {
                TempData["Error"] = "El producto está agotado.";
                return RedirectToAction("Index", "Home");
            }

            // Obtener el usuario logueado
            var usuario = await _context.users.FirstOrDefaultAsync(u => u.user_name == User.Identity.Name);
            if (usuario == null)
            {
                return RedirectToAction("Login", "Inicio"); // Redirigir si el usuario no está logueado
            }

            // Llamar al procedimiento almacenado para agregar al carrito
            try
            {
                await _context.Database.ExecuteSqlRawAsync("CALL agregar_al_carrito({0}, {1})", idProducto, usuario.id_user);
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;
                return RedirectToAction("Index", "Home");
            }

            return RedirectToAction("Index", "cars"); // Redirigir al carrito
        }

        public async Task<IActionResult> Index()
        {
            // Ejecutar la consulta SQL y mapear los resultados a la lista de CarritoViewModel
            List<details_car> carrito = new List<details_car>();

            try
            {
                carrito = await _context.Set<details_car>()
                    .FromSqlRaw(@"SELECT 
                            c.id_car AS CarritoId,
                            u.user_name AS NombreUsuario, 
                            p.product_name AS NombreProducto, 
                            c.quantity AS Cantidad, 
                            c.date AS Fecha,
                            c.price AS Precio
                          FROM 
                            car c
                          JOIN 
                            users u ON c.id_user = u.id_user
                          JOIN 
                            products p ON c.id_product = p.id_product
                          ")
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;
                return RedirectToAction("Index", "Home");
            }

            return View("Index", carrito);
        }

        public  async Task<IActionResult> FinzalizarCompra()
        {
            List<details_car> carrito = new List<details_car>();

            try
            {
                carrito = await _context.Set<details_car>()
                    .FromSqlRaw(@"SELECT 
                            c.id_car AS CarritoId,
                            u.user_name AS NombreUsuario, 
                            p.product_name AS NombreProducto, 
                            c.quantity AS Cantidad, 
                            c.date AS Fecha,
                            c.price AS Precio
                          FROM 
                            car c
                          JOIN 
                            users u ON c.id_user = u.id_user
                          JOIN 
                            products p ON c.id_product = p.id_product;
                          ")
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;
                return RedirectToAction("Index", "Home");
            }

            return View("FinzalizarCompra", carrito);
        }
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var car = await _context.car
                .FirstOrDefaultAsync(m => m.id_car == id);
            if (car == null)
            {
                return NotFound();
            }

            return View(car);
        }

        // GET: cars/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: cars/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("id_car,id_user,id_product,quantity,date")] car car)
        {
            if (ModelState.IsValid)
            {
                _context.Add(car);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(car);
        }

        // GET: cars/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var car = await _context.car.FindAsync(id);
            if (car == null)
            {
                return NotFound();
            }
            return View(car);
        }

        // POST: cars/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("id_car,id_user,id_product,quantity,date")] car car)
        {
            if (id != car.id_car)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(car);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!carExists(car.id_car))
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
            return View(car);
        }

        // GET: cars/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var car = await _context.car
                .FirstOrDefaultAsync(m => m.id_car == id);
            if (car == null)
            {
                return NotFound();
            }

            return View(car);
        }

        [HttpPost]
        public IActionResult DeleteItem(int id_car)
        {
            using (var connection = (NpgsqlConnection)_context.Database.GetDbConnection())
            {
                connection.Open();

                using (var command = new NpgsqlCommand("CALL eliminar_del_carrito(@p_id_car)", connection))
                {
                    command.Parameters.AddWithValue("p_id_car", id_car);

                    try
                    {
                        command.ExecuteNonQuery();
                        return Json(new { success = true });
                    }
                    catch (Exception ex)
                    {
                        return Json(new { success = false, message = "Error al eliminar el producto del carrito.", error = ex.Message });
                    }
                }
            }
        }


        // POST: cars/Delete/5
        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> DeleteConfirmed(int id)
        //{
        //    var car = await _context.car.FindAsync(id);
        //    if (car != null)
        //    {
        //        _context.car.Remove(car);
        //    }

        //    await _context.SaveChangesAsync();
        //    return RedirectToAction(nameof(Index));
        //}

        private bool carExists(int id)
        {
            return _context.car.Any(e => e.id_car == id);
        }
    }
}
