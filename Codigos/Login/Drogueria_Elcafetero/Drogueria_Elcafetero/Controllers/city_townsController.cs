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

    public class city_townsController : Controller
    {
        private readonly Drogueria_ElcafeteroContext _context;

        public city_townsController(Drogueria_ElcafeteroContext context)
        {
            _context = context;
        }

        // GET: city_towns
        public async Task<IActionResult> Index()
        {
            return View(await _context.city_towns.ToListAsync());
        }

        public async Task<IActionResult> IndexCity_towns()
        {
            var departmentcity_towns = await _context.department_City_Towns
                .FromSqlRaw(@" SELECT d.department_name,
                                c.city_name
                                FROM 
                                Department d
                                LEFT JOIN 
                                City_towns c ON d.department_name = c.department_name;
                ").ToListAsync();
            return View(await _context.city_towns.ToListAsync());
        }

        // GET: city_towns/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var city_towns = await _context.city_towns
                .FirstOrDefaultAsync(m => m.city_name == id);
            if (city_towns == null)
            {
                return NotFound();
            }

            return View(city_towns);
        }

        // GET: city_towns/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: city_towns/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("city_name,department_name")] city_towns city_towns)
        {
            if (ModelState.IsValid)
            {
                _context.Add(city_towns);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(city_towns);
        }

        // GET: city_towns/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var city_towns = await _context.city_towns.FindAsync(id);
            if (city_towns == null)
            {
                return NotFound();
            }
            return View(city_towns);
        }

        // POST: city_towns/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("city_name,department_name")] city_towns city_towns)
        {
            if (id != city_towns.city_name)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(city_towns);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!city_townsExists(city_towns.city_name))
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
            return View(city_towns);
        }

        // GET: city_towns/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var city_towns = await _context.city_towns
                .FirstOrDefaultAsync(m => m.city_name == id);
            if (city_towns == null)
            {
                return NotFound();
            }

            return View(city_towns);
        }

        // POST: city_towns/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var city_towns = await _context.city_towns.FindAsync(id);
            if (city_towns != null)
            {
                _context.city_towns.Remove(city_towns);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool city_townsExists(string id)
        {
            return _context.city_towns.Any(e => e.city_name == id);
        }
    }
}
