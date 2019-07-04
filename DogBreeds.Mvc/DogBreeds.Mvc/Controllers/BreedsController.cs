using DogBreeds.Mvc.Dal;
using DogBreeds.Mvc.Dal.Models;
using DogBreeds.Mvc.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using System.Linq;
using System.Threading.Tasks;

namespace DogBreeds.Mvc.Controllers
{
    public class BreedsController : Controller
    {
        private readonly DogBreedsContext context;

        public BreedsController(DogBreedsContext context) => this.context = context;

        public async Task<IActionResult> Index()
        {
            IIncludableQueryable<Breed, Breed> dogBreedsContext = context.Breeds.Include(i => i.ParentBreed);
            return View(await dogBreedsContext.ToListAsync());
        }

        public async Task<IActionResult> Details(int? id) => View(await GetBreed(id));

        public IActionResult Create() => View(CreateViewModel());

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Name,BreedId")] Breed breed)
        {
            string validationMessage = ValidateBreed(breed);

            if (!string.IsNullOrEmpty(validationMessage))
            {
                return View(CreateViewModel(validationMessage, breed));
            }

            if (breed.BreedId > 0)
            {
                Breed parentBreed = context.Breeds.SingleOrDefault(b => b.Id == breed.BreedId);

                if (parentBreed is null)
                {
                    return View(CreateViewModel("The parent Breed selected could not be found.", breed));
                }

                breed.ParentBreed = parentBreed;
            }

            context.Add(breed);
            await context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(int? id) => View(await GetBreed(id));
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,BreedId")] Breed breed)
        {
            string validationMessage = ValidateBreed(id, breed);

            if (!string.IsNullOrEmpty(validationMessage))
            {
                return View(CreateViewModel(validationMessage, breed));
            }

            if (breed.BreedId > 0)
            {
                Breed parentBreed = context.Breeds.SingleOrDefault(b => b.Id == breed.BreedId);

                if (parentBreed is null)
                {
                    return View(CreateViewModel("The parent Breed selected could not be found.", breed));
                }

                breed.ParentBreed = parentBreed;
            }
          
            try
            {
                Breed existingBreed = context.Breeds.Single(b => b.Id == id);

                existingBreed.Name = breed.Name;
                existingBreed.BreedId = breed.BreedId;
                await context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!BreedExists(breed.Id))
                {
                    return View(CreateViewModel("The selected Breed could not be found."));
                }
                else
                {
                    throw;
                }
            }

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(int? id) => View(await GetBreed(id));

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            Breed breed = await context.Breeds.SingleOrDefaultAsync(m => m.Id == id);

            if (breed != null)
            {
                if (!context.Individuals.Any(i => i.BreedId == id) && !context.Breeds.Any(b => b.ParentBreed.Id == id))
                {
                    try
                    {
                        context.Breeds.Remove(breed);

                        await context.SaveChangesAsync();
                    }
                    catch (DbUpdateConcurrencyException ex)
                    {
                        if (BreedExists(breed.Id))
                        {
                            throw;
                        }
                    }
                }                   
            }

            return RedirectToAction(nameof(Index));
        }

        private bool BreedExists(int id) => context.Breeds.Any(e => e.Id == id);

        private async Task<BreedViewModel> GetBreed(int? id)
        {
            if (id == null)
            {
                return CreateViewModel("The Breed ID provided was invalid.");
            }

            if (id <= 0 || id > int.MaxValue)
            {
                return CreateViewModel($"The Breed ID must be between 1 and {int.MaxValue}.");
            }

            Breed breed = await context.Breeds.SingleOrDefaultAsync(m => m.Id == id);

            if (breed == null)
            {
                return CreateViewModel("The selected Breed could not be found.");
            }

            return CreateViewModel(breed);
        }

        private BreedViewModel CreateViewModel() =>  CreateViewModel(string.Empty, new Breed());

        private  BreedViewModel CreateViewModel(string message) =>  CreateViewModel(message, new Breed());

        private  BreedViewModel CreateViewModel(Breed breed) =>  CreateViewModel(string.Empty, breed);

        private  BreedViewModel CreateViewModel(string message, Breed breed)
        {
            var viewModel = new BreedViewModel(breed)
            {
                Individuals = context.Individuals.Where(individual => individual.BreedId == breed.Id),
                Breeds = context.Breeds.Where(b => b.Id != breed.Id),
                ChildBreeds = context.Breeds.Where(b => b.BreedId == breed.Id)
            };

            if (!string.IsNullOrEmpty(message))
            {
                viewModel.Messages.Add(message);
            }

            return viewModel;
        }

        private string ValidateBreed(Breed breed)
        {
            if (string.IsNullOrEmpty(breed.Name))
            {
                return "A Breed must have a name.";
            }

            if (breed.BreedId < 0 || breed.BreedId > int.MaxValue)
            {
                return $"The Parent Breed ID cannot be negative or greater than {int.MaxValue}.";
            }

            return string.Empty;
        }

        private string ValidateBreed(int id, Breed breed)
        {
            if (id != breed.Id)
            {
                return "The Breed ID provided doesn't match the Breed being updated.";
            }
            
            if (id <= 0 || id > int.MaxValue)
            {
                return $"The Breed ID must be between 1 and {int.MaxValue}.";
            }

            if (!context.Breeds.Any(b => b.Id == id))
            {
                return "The selected Breed could not be found.";
            }

            if (breed.BreedId >= 0 && breed.BreedId == id)
            {
                return "A breed cannot be set as it's own Parent Breed.";
            }

            return ValidateBreed(breed);
        }
    }
}
