using DogBreeds.Mvc.Controllers;
using DogBreeds.Mvc.Dal.Models;
using DogBreeds.Mvc.Models;
using DogBreeds.Test;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace DogIndividuals.Test
{
    [Collection("Sequential")]
    public class IndividualControllerTests
    {
        private IndividualViewModel CheckViewModel(IActionResult result)
        {
            ViewResult viewResult = Assert.IsType<ViewResult>(result);
            IndividualViewModel viewModel = Assert.IsAssignableFrom<IndividualViewModel>(viewResult.ViewData.Model);

            Assert.NotEmpty(viewModel.Breeds);

            return viewModel;
        }

        private void CheckMessage(IndividualViewModel viewModel, string message)
        {
            Assert.True(viewModel.Messages.Any());
            Assert.Equal(message, viewModel.Messages.FirstOrDefault());
        }

        [Fact]
        public async void GetDetailsFound()
        {
            using (var database = new TestDb())
            {
                var controller = new IndividualsController(database.Context);

                IActionResult result = await controller.Details(5);

                IndividualViewModel viewModel = CheckViewModel(result);
                Individual individual = database.Context.Individuals.SingleOrDefault(i => i.Id == 5);

                Assert.Equal(5, viewModel.Individual.Id);
                Assert.Equal(individual.Name, viewModel.Individual.Name);
                Assert.NotEmpty(viewModel.Breeds);
            }
        }

        [Fact]
        public async void GetDetailsNullId()
        {
            using (var database = new TestDb())
            {
                var controller = new IndividualsController(database.Context);

                IActionResult result = await controller.Details(null);

                IndividualViewModel viewModel = CheckViewModel(result);

                CheckMessage(viewModel, "The Individual ID provided was invalid.");
            }
        }

        [Fact]
        public async void GetDetailsZeroId()
        {
            using (var database = new TestDb())
            {
                var controller = new IndividualsController(database.Context);

                IActionResult result = await controller.Details(0);

                IndividualViewModel viewModel = CheckViewModel(result);

                CheckMessage(viewModel, $"The Individual ID must be between 1 and {int.MaxValue}.");
            }
        }

        [Fact]
        public async void GetDetailsNegativeId()
        {
            using (var database = new TestDb())
            {
                var controller = new IndividualsController(database.Context);

                IActionResult result = await controller.Details(-1);

                IndividualViewModel viewModel = CheckViewModel(result);

                CheckMessage(viewModel, $"The Individual ID must be between 1 and {int.MaxValue}.");
            }
        }

        [Fact]
        public async void GetDetailsNotFound()
        {
            using (var database = new TestDb())
            {
                var controller = new IndividualsController(database.Context);

                IActionResult result = await controller.Details(50);

                IndividualViewModel viewModel = CheckViewModel(result);

                CheckMessage(viewModel, "The selected Individual could not be found.");
            }
        }

        [Fact]
        public void GetCreate()
        {
            using (var database = new TestDb())
            {
                var controller = new IndividualsController(database.Context);

                IActionResult result = controller.Create();

                IndividualViewModel viewModel = CheckViewModel(result);

                Assert.Equal(0, viewModel.Individual.Id);
                Assert.Equal(0, viewModel.Individual.Breed.Id);
                Assert.True(string.IsNullOrEmpty(viewModel.Individual.Name));
                Assert.NotEmpty(viewModel.Breeds);
            }
        }

        [Fact]
        public async void PostCreateSuccess()
        {
            using (var database = new TestDb())
            {
                var controller = new IndividualsController(database.Context);

                IActionResult result = await controller.Create(new Individual("Test Individual Success", 1));

                RedirectToActionResult redirect = Assert.IsType<RedirectToActionResult>(result);

                Assert.Equal("Index", redirect.ActionName);

                Individual individual = database.Context.Individuals.SingleOrDefault(i => i.Name == "Test Individual Success");

                Assert.False(individual == null);
                Assert.Equal(1, individual.Breed.Id);
            }
        }

        [Fact]
        public async void PostCreateNullName()
        {
            using (var database = new TestDb())
            {
                var controller = new IndividualsController(database.Context);

                IActionResult result = await controller.Create(new Individual());

                IndividualViewModel viewModel = CheckViewModel(result);

                Assert.Equal(0, viewModel.Individual.Id);
                Assert.True(string.IsNullOrEmpty(viewModel.Individual.Name));

                CheckMessage(viewModel, "You must enter a name for this Individual.");
            }
        }

        [Fact]
        public async void PostCreateNullBreedId()
        {
            using (var database = new TestDb())
            {
                var controller = new IndividualsController(database.Context);

                IActionResult result = await controller.Create(new Individual() { Name = "Test Individual 11" });

                IndividualViewModel viewModel = CheckViewModel(result);

                Assert.Equal(0, viewModel.Individual.Id);
                Assert.Equal("Test Individual 11", viewModel.Individual.Name);

                CheckMessage(viewModel, "You must select a Breed for this Individual.");
            }
        }

        [Fact]
        public async void PostCreateZeroBreedId()
        {
            using (var database = new TestDb())
            {
                var controller = new IndividualsController(database.Context);

                IActionResult result = await controller.Create(new Individual("Test Individual 11", 0));

                IndividualViewModel viewModel = CheckViewModel(result);

                Assert.Equal(0, viewModel.Individual.Id);
                Assert.Equal("Test Individual 11", viewModel.Individual.Name);

                CheckMessage(viewModel, "You must select a Breed for this Individual.");
            }
        }

        [Fact]
        public async void PostCreateNegativeBreedId()
        {
            using (var database = new TestDb())
            {
                var controller = new IndividualsController(database.Context);

                IActionResult result = await controller.Create(new Individual("Test Individual 11", -1));

                IndividualViewModel viewModel = CheckViewModel(result);

                Assert.Equal(0, viewModel.Individual.Id);
                Assert.Equal("Test Individual 11", viewModel.Individual.Name);

                CheckMessage(viewModel, "The Breed selected is invalid.");
            }
        }

        [Fact]
        public async void PostCreateBreedDoesntExist()
        {
            using (var database = new TestDb())
            {
                var controller = new IndividualsController(database.Context);

                IActionResult result = await controller.Create(new Individual("Test Individual 11", 100));

                IndividualViewModel viewModel = CheckViewModel(result);

                Assert.Equal(0, viewModel.Individual.Id);
                Assert.Equal("Test Individual 11", viewModel.Individual.Name);

                CheckMessage(viewModel, "The Breed selected is invalid.");
            }
        }

        [Fact]
        public async void GetEditFound()
        {
            using (var database = new TestDb())
            {
                var controller = new IndividualsController(database.Context);

                IActionResult result = await controller.Edit(2);

                IndividualViewModel viewModel = CheckViewModel(result);
                Individual individual = database.Context.Individuals.SingleOrDefault(i => i.Id == 2);

                Assert.Equal(2, viewModel.Individual.Id);
                Assert.Equal(individual.Name, viewModel.Individual.Name);
                Assert.NotEmpty(viewModel.Breeds);
            }
        }

        [Fact]
        public async void GetEditNullId()
        {
            using (var database = new TestDb())
            {
                var controller = new IndividualsController(database.Context);

                IActionResult result = await controller.Edit(null);

                IndividualViewModel viewModel = CheckViewModel(result);

                CheckMessage(viewModel, "The Individual ID provided was invalid.");
            }
        }

        [Fact]
        public async void GetEditZeroId()
        {
            using (var database = new TestDb())
            {
                var controller = new IndividualsController(database.Context);

                IActionResult result = await controller.Edit(0);

                IndividualViewModel viewModel = CheckViewModel(result);

                CheckMessage(viewModel, $"The Individual ID must be between 1 and {int.MaxValue}.");
            }
        }

        [Fact]
        public async void GetEditNegativeId()
        {
            using (var database = new TestDb())
            {
                var controller = new IndividualsController(database.Context);

                IActionResult result = await controller.Edit(-1);

                IndividualViewModel viewModel = CheckViewModel(result);

                CheckMessage(viewModel, $"The Individual ID must be between 1 and {int.MaxValue}.");
            }
        }

        [Fact]
        public async void GetEditNotFound()
        {
            using (var database = new TestDb())
            {
                var controller = new IndividualsController(database.Context);

                IActionResult result = await controller.Edit(100);

                IndividualViewModel viewModel = CheckViewModel(result);

                CheckMessage(viewModel, "The selected Individual could not be found.");
            }
        }

        [Fact]
        public async void PostEditSaved()
        {
            using (var database = new TestDb())
            {
                var controller = new IndividualsController(database.Context);

                IActionResult result = await controller.Edit(6, new Individual(6, "Modified Individual", 4));

                RedirectToActionResult redirect = Assert.IsType<RedirectToActionResult>(result);

                Assert.Equal("Index", redirect.ActionName);

                Individual individual = database.Context.Individuals.SingleOrDefault(b => b.Id == 6);

                Assert.Equal("Modified Individual", individual.Name);
            }
        }

        [Fact]
        public async void PostEditZeroId()
        {
            using (var database = new TestDb())
            {
                var controller = new IndividualsController(database.Context);

                IActionResult result = await controller.Edit(0, new Individual());

                IndividualViewModel viewModel = CheckViewModel(result);

                CheckMessage(viewModel, $"The Individual ID must be between 1 and {int.MaxValue}.");
            }
        }

        [Fact]
        public async void PostEditMismatchedIds()
        {
            using (var database = new TestDb())
            {
                var controller = new IndividualsController(database.Context);

                IActionResult result = await controller.Edit(1, new Individual { Id = 2 });

                IndividualViewModel viewModel = CheckViewModel(result);

                CheckMessage(viewModel, "The Individual ID provided was invalid.");
            }
        }

        [Fact]
        public async void PostEditNegativeId()
        {
            using (var database = new TestDb())
            {
                var controller = new IndividualsController(database.Context);

                IActionResult result = await controller.Edit(-1, new Individual(-1, "Test Individual", 3));

                IndividualViewModel viewModel = CheckViewModel(result);

                CheckMessage(viewModel, $"The Individual ID must be between 1 and {int.MaxValue}.");
            }
        }

        [Fact]
        public async void PostEditNoName()
        {
            using (var database = new TestDb())
            {
                var controller = new IndividualsController(database.Context);

                IActionResult result = await controller.Edit(2, new Individual(2, string.Empty, 2));

                ViewResult viewResult = Assert.IsType<ViewResult>(result);
                IndividualViewModel viewModel = Assert.IsAssignableFrom<IndividualViewModel>(viewResult.ViewData.Model);

                CheckMessage(viewModel, "An Individual must have a name.");
            }
        }
        
        [Fact]
        public async void PostEditNegativeBreedId()
        {
            using (var database = new TestDb())
            {
                var controller = new IndividualsController(database.Context);

                IActionResult result = await controller.Edit(2, new Individual(2, "Fido", -1));

                ViewResult viewResult = Assert.IsType<ViewResult>(result);
                IndividualViewModel viewModel = Assert.IsAssignableFrom<IndividualViewModel>(viewResult.ViewData.Model);

                CheckMessage(viewModel, "The Breed selected is invalid.");
            }
        }

        [Fact]
        public async void PostEditZeroBreedId()
        {
            using (var database = new TestDb())
            {
                var controller = new IndividualsController(database.Context);

                IActionResult result = await controller.Edit(2, new Individual(2, "Fido", 0));

                ViewResult viewResult = Assert.IsType<ViewResult>(result);
                IndividualViewModel viewModel = Assert.IsAssignableFrom<IndividualViewModel>(viewResult.ViewData.Model);

                CheckMessage(viewModel, "You must select a Breed for this Individual.");
            }
        }

        [Fact]
        public async void PostEditBreedDoesntExist()
        {
            using (var database = new TestDb())
            {
                var controller = new IndividualsController(database.Context);

                IActionResult result = await controller.Edit(2, new Individual(2, "Fido", 100));

                ViewResult viewResult = Assert.IsType<ViewResult>(result);
                IndividualViewModel viewModel = Assert.IsAssignableFrom<IndividualViewModel>(viewResult.ViewData.Model);

                CheckMessage(viewModel, "The Breed selected is invalid.");
            }
        }

        [Fact]
        public async void PostEditNotFound()
        {
            using (var database = new TestDb())
            {
                var controller = new IndividualsController(database.Context);

                IActionResult result = await controller.Edit(50, new Individual(50, "Test Individual 6 Modified", 2));

                IndividualViewModel viewModel = CheckViewModel(result);

                CheckMessage(viewModel, "The selected Individual could not be found.");
            }
        }

        [Fact]
        public async void GetIndex()
        {
            using (var database = new TestDb())
            {
                var controller = new IndividualsController(database.Context);

                IActionResult result = await controller.Index();

                ViewResult viewResult = Assert.IsType<ViewResult>(result);
                IEnumerable<Individual> individuals = Assert.IsAssignableFrom<IEnumerable<Individual>>(viewResult.ViewData.Model);

                Assert.Equal(database.Context.Individuals.Count(), individuals.Count());
            }
        }

        [Fact]
        public async void GetDeleteFound()
        {
            using (var database = new TestDb())
            {
                var controller = new IndividualsController(database.Context);

                IActionResult result = await controller.Delete(10);

                IndividualViewModel viewModel = CheckViewModel(result);
                Individual individual = database.Context.Individuals.SingleOrDefault(i => i.Id == 10);

                Assert.Equal(10, viewModel.Individual.Id);
                Assert.Equal(individual.Name, viewModel.Individual.Name);
                Assert.Equal(individual.BreedId, viewModel.Individual.Breed.Id);
            }
        }

        [Fact]
        public async void GetDeleteNotFound()
        {
            using (var database = new TestDb())
            {
                var controller = new IndividualsController(database.Context);

                IActionResult result = await controller.Delete(50);

                IndividualViewModel viewModel = CheckViewModel(result);

                CheckMessage(viewModel, "The selected Individual could not be found.");
            }
        }

        [Fact]
        public async void GetDeleteNullId()
        {
            using (var database = new TestDb())
            {
                var controller = new IndividualsController(database.Context);

                IActionResult result = await controller.Delete(null);

                IndividualViewModel viewModel = CheckViewModel(result);

                CheckMessage(viewModel, "The Individual ID provided was invalid.");
            }
        }

        [Fact]
        public async void GetDeleteZeroId()
        {
            using (var database = new TestDb())
            {
                var controller = new IndividualsController(database.Context);

                IActionResult result = await controller.Delete(0);

                IndividualViewModel viewModel = CheckViewModel(result);

                CheckMessage(viewModel, $"The Individual ID must be between 1 and {int.MaxValue}.");
            }
        }
        
        [Fact]
        public async void GetDeleteNegativeId()
        {
            using (var database = new TestDb())
            {
                var controller = new IndividualsController(database.Context);

                IActionResult result = await controller.Delete(-1);

                IndividualViewModel viewModel = CheckViewModel(result);

                CheckMessage(viewModel, $"The Individual ID must be between 1 and {int.MaxValue}.");
            }
        }

        [Fact]
        public async void PostDeleteConfirmNotFound()
        {
            using (var database = new TestDb())
            {
                var controller = new IndividualsController(database.Context);

                IActionResult result = await controller.DeleteConfirmed(8);

                RedirectToActionResult redirect = Assert.IsType<RedirectToActionResult>(result);

                Assert.Equal("Index", redirect.ActionName);
            }
        }

        [Fact]
        public async void PostDeleteConfirmZeroId()
        {
            using (var database = new TestDb())
            {
                var controller = new IndividualsController(database.Context);

                IActionResult result = await controller.DeleteConfirmed(0);

                RedirectToActionResult redirect = Assert.IsType<RedirectToActionResult>(result);

                Assert.Equal("Index", redirect.ActionName);
            }
        }

        [Fact]
        public async void PostDeleteConfirmNegativeId()
        {
            using (var database = new TestDb())
            {
                var controller = new IndividualsController(database.Context);

                IActionResult result = await controller.DeleteConfirmed(-1);

                RedirectToActionResult redirect = Assert.IsType<RedirectToActionResult>(result);

                Assert.Equal("Index", redirect.ActionName);
            }
        }

        [Fact]
        public async void PostDeleteConfirmSuccess()
        {
            using (var database = new TestDb())
            {
                Assert.True(database.Context.Individuals.Any(i => i.Id == 3));

                var controller = new IndividualsController(database.Context);

                IActionResult result = await controller.DeleteConfirmed(3);

                RedirectToActionResult redirect = Assert.IsType<RedirectToActionResult>(result);

                Assert.Equal("Index", redirect.ActionName);

                Assert.False(database.Context.Individuals.Any(i => i.Id == 3));
            }
        }
    }
}

