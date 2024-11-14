﻿using System;
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
    public class carsController : Controller
    {
        private readonly Drogueria_ElcafeteroContext _context;

        public carsController(Drogueria_ElcafeteroContext context)
        {
            _context = context;
        }

        //[HttpPost]
        //public async Task<IActionResult> AgregarAlCarrito(int idProducto)
        //{

        //    // Obtener el producto seleccionado
        //    var producto = await _context.products.FindAsync(idProducto);
        //    if (producto == null)
        //    {
        //        return NotFound(); // Si no existe el producto
        //    }

        //    if (producto.units_in_stock == 0)
        //    {
        //        TempData["Mensaje"] = "No hay unidades disponibles en stock para este producto.";
        //        return RedirectToAction("Index", "Home");
        //    }


        //    if (producto == null)
        //    {
        //        // Mostrar mensaje de error, producto vencido no se puede agregar al carrito
        //        TempData["Error"] = "Este producto está vencido y no se puede agregar al carrito.";
        //        return RedirectToAction("Index", "Home");
        //    }



        //    if (producto == null)
        //    {
        //        // Mostrar mensaje de error, producto sin stock no se puede agregar al carrito
        //        TempData["Error"] = "Este producto está agotado y no se puede agregar al carrito.";
        //        return RedirectToAction("Index", "Products");
        //    }

        //    // Obtener el usuario logueado
        //    var usuario = await _context.users.FirstOrDefaultAsync(u => u.user_name == User.Identity.Name);
        //    if (usuario == null)
        //    {
        //        return RedirectToAction("Login", "Inicio"); // Redirigir si el usuario no está logueado
        //    }

        //    // Buscar si el producto ya está en el carrito
        //    var carritoExistente = await _context.car
        //        .FirstOrDefaultAsync(c => c.id_user == usuario.id_user && c.id_product == idProducto);

        //    if (carritoExistente != null)
        //    {
        //        // Si ya existe, incrementar la cantidad y actualizar el precio total dinámicamente
        //        carritoExistente.quantity += 1;
        //        // Actualizar el total_price multiplicando la cantidad por el precio del producto
        //        //carritoExistente.price = Convert.ToDecimal(carritoExistente.quantity * producto.price);

        //        _context.Update(carritoExistente);
        //    }
        //    else
        //    {
        //        // Si no existe, crear un nuevo elemento en el carrito con el precio total calculado
        //        var nuevoCarrito = new car
        //        {
        //            id_user = usuario.id_user,
        //            id_product = idProducto,
        //            quantity = 1,
        //            date = DateTime.UtcNow,// Convertir la fecha a UTC
        //            price = Convert.ToDecimal(producto.price)

        //        };

        //        _context.Add(nuevoCarrito);
        //    }

        //    // Guardar cambios en la base de datos
        //    await _context.SaveChangesAsync();

        //    // Redirigir al carrito
        //    return Ok(); // Asegúrate de tener un índice donde se vea el carrito
        //}

        [HttpPost]
        public async Task<IActionResult> AgregarAlCarrito(int idProducto)
        {
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

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                // Llamar al procedimiento almacenado para eliminar del carrito
                await _context.Database.ExecuteSqlRawAsync("CALL eliminar_del_carrito({0})", id);
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;
                return RedirectToAction(nameof(Index));
            }

            return RedirectToAction(nameof(Index)); // Redirigir al carrito
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
                            products p ON c.id_product = p.id_product;
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


        // GET: cars
        //public async Task<IActionResult> Index()
        //{
        //    var carrito = await _context.details_car
        //                  .FromSqlRaw(@"SELECT 
        //        c.id_car AS CarritoId,
        //        u.user_name AS NombreUsuario, 
        //        p.product_name AS NombreProducto, 
        //        c.quantity AS Cantidad, 
        //        c.date AS Fecha,
        //        c.price As Precio
        //    FROM 
        //        car c
        //    JOIN 
        //        users u ON c.id_user = u.id_user
        //    JOIN 
        //        products p ON c.id_product = p.id_product;
        //    ")
        //                  .ToListAsync();

        //    return View("Index", carrito);

        //}
        // GET: cars/Details/5
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
