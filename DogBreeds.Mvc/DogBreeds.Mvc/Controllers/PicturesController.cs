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
    public class PicturesController : Controller
    {
        private readonly DogBreedsContext _context;

        public PicturesController(DogBreedsContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Details(int? breedId, int? individualId)
        {
            if (id == null)
            {
                return NotFound();
            }

            var breed = await _context.Breeds
                .SingleOrDefaultAsync(m => m.Id == id);
            if (breed == null)
            {
                return NotFound();
            }

            return View(breed);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Uri,IndividualId,BreedId")] Picture picture)
        {
            if (ModelState.IsValid)
            {
                _context.Add(picture);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            ViewData["BreedId"] = new SelectList(_context.Breeds, "Id", "Name", picture.BreedId);
            ViewData["IndividualId"] = new SelectList(_context.Individuals, "Id", "Name", picture.IndividualId);

            return View(picture);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var picture = await _context.Pictures
                .Include(p => p.Breed)
                .Include(p => p.Individual)
                .SingleOrDefaultAsync(m => m.Id == id);
            if (picture == null)
            {
                return NotFound();
            }

            return View(picture);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var picture = await _context.Pictures.SingleOrDefaultAsync(m => m.Id == id);
            _context.Pictures.Remove(picture);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PictureExists(int id)
        {
            return _context.Pictures.Any(e => e.Id == id);
        }
    }
}
