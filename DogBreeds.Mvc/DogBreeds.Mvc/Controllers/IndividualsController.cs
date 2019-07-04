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
    public class IndividualsController : Controller
    {
        private readonly DogBreedsContext context;

        public IndividualsController(DogBreedsContext context) => this.context = context;

        public async Task<IActionResult> Index()
        {
            IIncludableQueryable<Individual, Breed> dogBreedsContext = context.Individuals.Include(i => i.Breed).Include(b=> b.Breed.ParentBreed);
            return View(await dogBreedsContext.ToListAsync());
        }

        private async Task<IndividualViewModel> GetIndividual(int? id)
        {
            if (id == null)
            {
                return CreateViewModel("The Individual ID provided was invalid.");
            }

            if (id <= 0 || id > int.MaxValue)
            {
                return CreateViewModel($"The Individual ID must be between 1 and {int.MaxValue}.");
            }

            Individual individual = await context.Individuals.Include(i => i.Breed).SingleOrDefaultAsync(m => m.Id == id);

            if (individual == null)
            {
                return  CreateViewModel("The selected Individual could not be found.");
            }

            return CreateViewModel(individual);
        }

        public async Task<IActionResult> Details(int? id) => View(await GetIndividual(id));

        public IActionResult Create() => View(CreateViewModel());

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Name,BreedId")] Individual individual)
        {
            string validationMessage = ValidateIndividual(individual);

            if (!string.IsNullOrEmpty(validationMessage))
            {
                return View(CreateViewModel(validationMessage, individual));
            }

            Breed breed = context.Breeds.SingleOrDefault(b => b.Id == individual.BreedId);

            individual.Breed = breed;

            context.Add(individual);
            await context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(int? id) => View(await GetIndividual(id));

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,BreedId")] Individual individual)
        {
            string validationMessage = ValidateIndividual(id, individual);

            if (!string.IsNullOrEmpty(validationMessage))
            {
                return View(CreateViewModel(validationMessage, individual));
            }

            Breed breed = context.Breeds.SingleOrDefault(b => b.Id == individual.BreedId);
            individual.Breed = breed;

            try
            {
                Individual existingIndividual = context.Individuals.Single(b => b.Id == id);

                existingIndividual.Name = individual.Name;
                existingIndividual.BreedId = individual.BreedId;
                await context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!IndividualExists(individual.Id))
                {
                    return View(CreateViewModel("The selected Individual could not be found."));
                }
                else
                {
                    return View(CreateViewModel("Could not update the Individual"));
                }
            }

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(int? id) => View(await GetIndividual(id));

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            Individual individual = await context.Individuals.SingleOrDefaultAsync(m => m.Id == id);

            if (individual != null)
            {
                try
                {
                    context.Individuals.Remove(individual);

                    await context.SaveChangesAsync();
                }
                catch(DbUpdateConcurrencyException ex)
                {
                    if (IndividualExists(individual.Id))
                    {
                        throw;
                    }
                }
                
            }

            return RedirectToAction(nameof(Index));
        }

        private bool IndividualExists(int id) => context.Individuals.Any(e => e.Id == id);

        private IndividualViewModel CreateViewModel() => CreateViewModel(string.Empty, new Individual());

        private IndividualViewModel CreateViewModel(string message) => CreateViewModel(message, new Individual());

        private IndividualViewModel CreateViewModel(Individual individual) => CreateViewModel(string.Empty, individual);

        private IndividualViewModel CreateViewModel(string message, Individual individual)
        {
            var viewModel = new IndividualViewModel(individual)
            {
                Breeds = context.Breeds
            };

            if (!string.IsNullOrEmpty(message))
            {
                viewModel.Messages.Add(message);
            }

            return viewModel;
        }
        
        private string ValidateIndividual(int id, Individual individual)
        {
            if (id != individual.Id)
            {
                return "The Individual ID provided was invalid.";
            }
            
            if (individual.Id <= 0 || individual.Id > int.MaxValue)
            {
                return $"The Individual ID must be between 1 and {int.MaxValue}.";
            }

            if (!context.Individuals.Any(i => i.Id == id))
            {
                return "The selected Individual could not be found.";
            }

            return ValidateIndividual(individual);
        }

        private string ValidateIndividual(Individual individual)
        {
            if (string.IsNullOrEmpty(individual.Name))
            {
                return "An Individual must have a name.";
            }

            if (individual.BreedId == 0)
            {
                return "You must select a Breed for this Individual.";
            }

            if (individual.BreedId <= 0 || individual.BreedId > int.MaxValue)
            {
                return $"The Breed ID must be between 1 and {int.MaxValue}.";
            }

            if(!context.Breeds.Any(b => b.Id == individual.BreedId))
            {
                return "The selected Breed could not be found.";
            }

            return string.Empty;
        }
    }
}
