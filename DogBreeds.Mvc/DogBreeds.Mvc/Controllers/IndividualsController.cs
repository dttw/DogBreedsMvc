using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using DogBreeds.Mvc.Dal;
using DogBreeds.Mvc.Dal.Models;

namespace DogBreeds.Mvc.Controllers
{
    public class IndividualsController : Controller
    {
        private readonly DogBreedsContext _context;

        public IndividualsController(DogBreedsContext context)
        {
            _context = context;
        }

        // GET: Individuals
        public async Task<IActionResult> Index()
        {
            var dogBreedsContext = _context.Individuals.Include(i => i.Breed);
            return View(await dogBreedsContext.ToListAsync());
        }

        // GET: Individuals/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var individual = await _context.Individuals
                .Include(i => i.Breed)
                .SingleOrDefaultAsync(m => m.Id == id);
            if (individual == null)
            {
                return NotFound();
            }

            return View(individual);
        }

        // GET: Individuals/Create
        public IActionResult Create()
        {
            ViewData["BreedId"] = new SelectList(_context.Breeds, "Id", "Name");
            return View();
        }

        // POST: Individuals/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,BreedId")] Individual individual)
        {
            if (ModelState.IsValid)
            {
                _context.Add(individual);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["BreedId"] = new SelectList(_context.Breeds, "Id", "Name", individual.BreedId);
            return View(individual);
        }

        // GET: Individuals/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var individual = await _context.Individuals.SingleOrDefaultAsync(m => m.Id == id);
            if (individual == null)
            {
                return NotFound();
            }
            ViewData["BreedId"] = new SelectList(_context.Breeds, "Id", "Name", individual.BreedId);
            return View(individual);
        }

        // POST: Individuals/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,BreedId")] Individual individual)
        {
            if (id != individual.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(individual);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!IndividualExists(individual.Id))
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
            ViewData["BreedId"] = new SelectList(_context.Breeds, "Id", "Name", individual.BreedId);
            return View(individual);
        }

        // GET: Individuals/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var individual = await _context.Individuals
                .Include(i => i.Breed)
                .SingleOrDefaultAsync(m => m.Id == id);
            if (individual == null)
            {
                return NotFound();
            }

            return View(individual);
        }

        // POST: Individuals/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var individual = await _context.Individuals.SingleOrDefaultAsync(m => m.Id == id);
            _context.Individuals.Remove(individual);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool IndividualExists(int id)
        {
            return _context.Individuals.Any(e => e.Id == id);
        }
    }
}
