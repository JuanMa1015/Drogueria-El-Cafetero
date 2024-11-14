using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Drogueria_Elcafetero.Data;
using Drogueria_Elcafetero.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;


namespace Drogueria_Elcafetero.Controllers
{


    public class employeesController : Controller
    {
        private readonly Drogueria_ElcafeteroContext _context;

        public employeesController(Drogueria_ElcafeteroContext context)
        {
            _context = context;
        }

        // GET: Employees
        public async Task<IActionResult> Index()
        {
            return View(await _context.employees.ToListAsync());
        }

        // GET: Employees/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var Employees = await _context.employees
                .FirstOrDefaultAsync(m => m.id_employee == id);
            if (Employees == null)
            {
                return NotFound();
            }

            return View(Employees);
        }

        // GET: Employees/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Employees/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("id_employee,employee_name,salary,hiring_date,email,password_hash,id_Rol")] Employees Employees)
        {
            if (ModelState.IsValid)
            {

                if (Employees.hiring_date.Kind == DateTimeKind.Unspecified)
                {
                    Employees.hiring_date = DateTime.SpecifyKind(Employees.hiring_date, DateTimeKind.Utc);

                }
                else
                {
                    Employees.hiring_date = Employees.hiring_date.ToUniversalTime();
                }
                _context.Add(Employees);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(Employees);
        }

        // Acción para mostrar el panel de control del empleado
        public IActionResult EmployeesPage()
        {
            return View();
        }

        // GET: Employees/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var Employees = await _context.employees.FindAsync(id);
            if (Employees == null)
            {
                return NotFound();
            }
            return View(Employees);
        }

        // POST: Employees/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("id_employee,employee_name,salary,hiring_date,email,password_hash,id_Rol")] Employees Employees)
        {
            if (id != Employees.id_employee)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(Employees);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EmployeesExists(Employees.id_employee))
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
            return View(Employees);
        }

        // GET: Employees/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var Employees = await _context.employees
                .FirstOrDefaultAsync(m => m.id_employee == id);
            if (Employees == null)
            {
                return NotFound();
            }

            return View(Employees);
        }

        // POST: Employees/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var Employees = await _context.employees.FindAsync(id);
            if (Employees != null)
            {
                _context.employees.Remove(Employees);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool EmployeesExists(int id)
        {
            return _context.employees.Any(e => e.id_employee == id);
        }
    }
}
