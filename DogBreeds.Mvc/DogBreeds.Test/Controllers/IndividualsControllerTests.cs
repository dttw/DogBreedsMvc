using DogBreeds.Mvc.Controllers;
using DogBreeds.Mvc.Dal.Models;
using DogBreeds.Mvc.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace DogBreeds.Test.Controllers
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

        private void CheckMessage(IActionResult result, string message)
        {
            var viewModel = CheckViewModel(result);

            Assert.True(viewModel.Messages.Any());
            Assert.Equal(message, viewModel.Messages.FirstOrDefault());
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

        #region Create

        #region Get
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

        #endregion

        #region Post
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
            }
        }

        [Fact]
        public async void PostCreateNullName()
        {
            using (var database = new TestDb())
            {
                var controller = new IndividualsController(database.Context);

                IActionResult result = await controller.Create(new Individual());

                CheckMessage(result, "An Individual must have a name.");
            }
        }

        [Fact]
        public async void PostCreateNullBreedId()
        {
            using (var database = new TestDb())
            {
                var controller = new IndividualsController(database.Context);

                IActionResult result = await controller.Create(new Individual() { Name = "Test Individual 11" });

                CheckMessage(result, "You must select a Breed for this Individual.");
            }
        }

        [Fact]
        public async void PostCreateZeroBreedId()
        {
            using (var database = new TestDb())
            {
                var controller = new IndividualsController(database.Context);

                IActionResult result = await controller.Create(new Individual("Test Individual 11", 0));

                CheckMessage(result, "You must select a Breed for this Individual.");
            }
        }

        [Fact]
        public async void PostCreateNegativeBreedId()
        {
            using (var database = new TestDb())
            {
                var controller = new IndividualsController(database.Context);

                IActionResult result = await controller.Create(new Individual("Test Individual 11", -1));

                CheckMessage(result, $"The Breed ID must be between 1 and {int.MaxValue}.");
            }
        }

        [Fact]
        public async void PostCreateBreedDoesntExist()
        {
            using (var database = new TestDb())
            {
                var controller = new IndividualsController(database.Context);

                IActionResult result = await controller.Create(new Individual("Test Individual 11", 100));

                CheckMessage(result, "The selected Breed could not be found.");
            }
        }
        #endregion

        #endregion

        #region Details

        #region Get
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

                CheckMessage(result, "The Individual ID provided was invalid.");
            }
        }

        [Fact]
        public async void GetDetailsZeroId()
        {
            using (var database = new TestDb())
            {
                var controller = new IndividualsController(database.Context);

                IActionResult result = await controller.Details(0);

                CheckMessage(result, $"The Individual ID must be between 1 and {int.MaxValue}.");
            }
        }

        [Fact]
        public async void GetDetailsNegativeId()
        {
            using (var database = new TestDb())
            {
                var controller = new IndividualsController(database.Context);

                IActionResult result = await controller.Details(-1);

                CheckMessage(result, $"The Individual ID must be between 1 and {int.MaxValue}.");
            }
        }

        [Fact]
        public async void GetDetailsNotFound()
        {
            using (var database = new TestDb())
            {
                var controller = new IndividualsController(database.Context);

                IActionResult result = await controller.Details(50);

                CheckMessage(result, "The selected Individual could not be found.");
            }
        }
        #endregion

        #endregion

        #region Edit

        #region Get
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

                CheckMessage(result, "The Individual ID provided was invalid.");
            }
        }

        [Fact]
        public async void GetEditZeroId()
        {
            using (var database = new TestDb())
            {
                var controller = new IndividualsController(database.Context);

                IActionResult result = await controller.Edit(0);

                CheckMessage(result, $"The Individual ID must be between 1 and {int.MaxValue}.");
            }
        }

        [Fact]
        public async void GetEditNegativeId()
        {
            using (var database = new TestDb())
            {
                var controller = new IndividualsController(database.Context);

                IActionResult result = await controller.Edit(-1);

                CheckMessage(result, $"The Individual ID must be between 1 and {int.MaxValue}.");
            }
        }

        [Fact]
        public async void GetEditNotFound()
        {
            using (var database = new TestDb())
            {
                var controller = new IndividualsController(database.Context);

                IActionResult result = await controller.Edit(100);

                CheckMessage(result, "The selected Individual could not be found.");
            }
        }

        #endregion

        #region Post
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

                CheckMessage(result, $"The Individual ID must be between 1 and {int.MaxValue}.");
            }
        }

        [Fact]
        public async void PostEditMismatchedIds()
        {
            using (var database = new TestDb())
            {
                var controller = new IndividualsController(database.Context);

                IActionResult result = await controller.Edit(1, new Individual { Id = 2 });

                CheckMessage(result, "The Individual ID provided was invalid.");
            }
        }

        [Fact]
        public async void PostEditNegativeId()
        {
            using (var database = new TestDb())
            {
                var controller = new IndividualsController(database.Context);

                IActionResult result = await controller.Edit(-1, new Individual(-1, "Test Individual", 3));

                CheckMessage(result, $"The Individual ID must be between 1 and {int.MaxValue}.");
            }
        }

        [Fact]
        public async void PostEditNoName()
        {
            using (var database = new TestDb())
            {
                var controller = new IndividualsController(database.Context);

                IActionResult result = await controller.Edit(2, new Individual(2, string.Empty, 2));

                CheckMessage(result, "An Individual must have a name.");
            }
        }

        [Fact]
        public async void PostEditNegativeBreedId()
        {
            using (var database = new TestDb())
            {
                var controller = new IndividualsController(database.Context);

                IActionResult result = await controller.Edit(2, new Individual(2, "Fido", -1));

                CheckMessage(result, $"The Breed ID must be between 1 and {int.MaxValue}.");
            }
        }

        [Fact]
        public async void PostEditZeroBreedId()
        {
            using (var database = new TestDb())
            {
                var controller = new IndividualsController(database.Context);

                IActionResult result = await controller.Edit(2, new Individual(2, "Fido", 0));

                CheckMessage(result, "You must select a Breed for this Individual.");
            }
        }

        [Fact]
        public async void PostEditNullBreedId()
        {
            using (var database = new TestDb())
            {
                var controller = new IndividualsController(database.Context);

                IActionResult result = await controller.Edit(2, new Individual { Id = 2, Name = "Fido" });

                CheckMessage(result, "You must select a Breed for this Individual.");
            }
        }

        [Fact]
        public async void PostEditBreedDoesntExist()
        {
            using (var database = new TestDb())
            {
                var controller = new IndividualsController(database.Context);

                IActionResult result = await controller.Edit(2, new Individual(2, "Fido", 100));

                CheckMessage(result, "The selected Breed could not be found.");
            }
        }

        [Fact]
        public async void PostEditNotFound()
        {
            using (var database = new TestDb())
            {
                var controller = new IndividualsController(database.Context);

                IActionResult result = await controller.Edit(50, new Individual(50, "Test Individual Modified", 2));

                CheckMessage(result, "The selected Individual could not be found.");
            }
        }

        #endregion
        
        #endregion

        #region Delete

        #region Get
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

                CheckMessage(result, "The selected Individual could not be found.");
            }
        }

        [Fact]
        public async void GetDeleteNullId()
        {
            using (var database = new TestDb())
            {
                var controller = new IndividualsController(database.Context);

                IActionResult result = await controller.Delete(null);

                CheckMessage(result, "The Individual ID provided was invalid.");
            }
        }

        [Fact]
        public async void GetDeleteZeroId()
        {
            using (var database = new TestDb())
            {
                var controller = new IndividualsController(database.Context);

                IActionResult result = await controller.Delete(0);

                CheckMessage(result, $"The Individual ID must be between 1 and {int.MaxValue}.");
            }
        }

        [Fact]
        public async void GetDeleteNegativeId()
        {
            using (var database = new TestDb())
            {
                var controller = new IndividualsController(database.Context);

                IActionResult result = await controller.Delete(-1);

                CheckMessage(result, $"The Individual ID must be between 1 and {int.MaxValue}.");
            }
        }

        #endregion
        
        #region Post
        [Fact]
        public async void PostDeleteConfirmNotFound()
        {
            using (var database = new TestDb())
            {
                var controller = new IndividualsController(database.Context);

                IActionResult result = await controller.DeleteConfirmed(500);

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
        #endregion
        
        #endregion
    }
}

