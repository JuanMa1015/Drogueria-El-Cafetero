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

    public class salesController : Controller
    {
        private readonly Drogueria_ElcafeteroContext _context;

        public salesController(Drogueria_ElcafeteroContext context)
        {
            _context = context;
        }

        // GET: sales
        public async Task<IActionResult> Index()
        {
            var detailssales = await _context.detailssales
                    .FromSqlRaw(@"SELECT 
                                    s.id_sale AS IdSale,
                                    c.customer_name AS CustomerName,
                                    e.employee_name AS EmployeeName,
                                    s.sale_date AS SaleDate,
                                    s.total_sale AS TotalSale
                                FROM 
                                    Sales s
                                JOIN 
                                    Customers c ON s.id_customer = c.id_customer
                                JOIN 
                                    Employees e ON s.id_employee = e.id_employee
                                ")
                .ToListAsync();

            if (detailssales == null)
            {
                detailssales = new List<detailssales>(); // Initialize an empty list to prevent null reference
                Console.WriteLine("No se encontraron productos.");
            }

            return View(detailssales);
        }


        // GET: sales/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var sales = await _context.sales
                .FirstOrDefaultAsync(m => m.id_sale == id);
            if (sales == null)
            {
                return NotFound();
            }

            return View(sales);
        }

        // GET: sales/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: sales/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("id_Sale,id_customer,id_employee,sale_date,total_sale")] sales sales)
        {
            if (ModelState.IsValid)
            {
                if (sales.sale_date.Kind == DateTimeKind.Unspecified)
                {
                    sales.sale_date = DateTime.SpecifyKind(sales.sale_date, DateTimeKind.Utc);

                }
                else
                {
                    sales.sale_date = sales.sale_date.ToUniversalTime();
                }
                _context.Add(sales);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(sales);
        }

        // GET: sales/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var sales = await _context.sales.FindAsync(id);
            if (sales == null)
            {
                return NotFound();
            }
            return View(sales);
        }

        // POST: sales/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("id_Sale,id_customer,id_employee,sale_date,total_sale")] sales sales)
        {
            if (id != sales.id_sale)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(sales);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!salesExists(sales.id_sale))
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
            return View(sales);
        }

        // GET: sales/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var sales = await _context.sales
                .FirstOrDefaultAsync(m => m.id_sale == id);
            if (sales == null)
            {
                return NotFound();
            }

            return View(sales);
        }

        // POST: sales/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var sales = await _context.sales.FindAsync(id);
            if (sales != null)
            {
                _context.sales.Remove(sales);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool salesExists(int id)
        {
            return _context.sales.Any(e => e.id_sale == id);
        }
    }
}