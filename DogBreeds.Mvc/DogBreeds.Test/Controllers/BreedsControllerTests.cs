using DogBreeds.Mvc.Controllers;
using DogBreeds.Mvc.Dal.Models;
using DogBreeds.Mvc.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace DogBreeds.Test
{
    [Collection("Sequential")]
    public class BreedControllerTests
    { 
        private BreedViewModel CheckViewModel(IActionResult result)
        {
            ViewResult viewResult = Assert.IsType<ViewResult>(result);
            BreedViewModel viewModel = Assert.IsAssignableFrom<BreedViewModel>(viewResult.ViewData.Model);

            return viewModel;
        }

        private void CheckMessage(BreedViewModel viewModel, string message)
        {
            Assert.True(viewModel.Messages.Any());
            Assert.Equal(message, viewModel.Messages.FirstOrDefault());
        }

        [Fact]
        public async void GetDetailsFound()
        {
            using (var database = new TestDb())
            {
                var controller = new BreedsController(database.Context);

                IActionResult result = await controller.Details(8);

                BreedViewModel viewModel = CheckViewModel(result);
                Breed breed = database.Context.Breeds.SingleOrDefault(b => b.Id == 8);

                Assert.Equal(8, viewModel.Breed.Id);
                Assert.Equal(breed.Name, viewModel.Breed.Name);
            }
        }

        [Fact]
        public async void GetDetailsNullId()
        {
            using (var database = new TestDb())
            {

                var controller = new BreedsController(database.Context);

                IActionResult result = await controller.Details(null);

                BreedViewModel viewModel = CheckViewModel(result);

                CheckMessage(viewModel, "The Breed ID provided was invalid.");
            }
        }

        [Fact]
        public async void GetDetailsZeroId()
        {
            using (var database = new TestDb())
            {

                var controller = new BreedsController(database.Context);

                IActionResult result = await controller.Details(0);

                BreedViewModel viewModel = CheckViewModel(result);

                CheckMessage(viewModel, $"The Breed ID must be between 1 and {int.MaxValue}.");
            }
        }

        [Fact]
        public async void GetDetailsNegativeId()
        {
            using (var database = new TestDb())
            {

                var controller = new BreedsController(database.Context);

                IActionResult result = await controller.Details(-1);

                BreedViewModel viewModel = CheckViewModel(result);

                CheckMessage(viewModel, $"The Breed ID must be between 1 and {int.MaxValue}.");
            }
        }

        [Fact]
        public async void GetDetailsNotFound()
        {
            using (var database = new TestDb())
            {

                var controller = new BreedsController(database.Context);

                IActionResult result = await controller.Details(50);

                BreedViewModel viewModel = CheckViewModel(result);

                CheckMessage(viewModel, "The selected Breed could not be found.");
            }
        }

        [Fact]
        public void GetCreate()
        {
            using (var database = new TestDb())
            {

                var controller = new BreedsController(database.Context);

                IActionResult result = controller.Create();

                BreedViewModel viewModel = CheckViewModel(result);

                Assert.Equal(0, viewModel.Breed.Id);
                Assert.True(string.IsNullOrEmpty(viewModel.Breed.Name));
            }
        }

        [Fact]
        public async void PostCreateSuccess()
        {
            using (var database = new TestDb())
            {
                var controller = new BreedsController(database.Context);

                IActionResult result = await controller.Create(new Breed("Test Breed 11"));

                RedirectToActionResult redirect = Assert.IsType<RedirectToActionResult>(result);

                Assert.Equal("Index", redirect.ActionName);

                Breed breed = database.Context.Breeds.LastOrDefault();

                Assert.Equal("Test Breed 11", breed.Name);
            }
        }

        [Fact]
        public async void PostCreateNoName()
        {
            using (var database = new TestDb())
            {

                var controller = new BreedsController(database.Context);

                IActionResult result = await controller.Create(new Breed());

                BreedViewModel viewModel = CheckViewModel(result);

                Assert.Equal(0, viewModel.Breed.Id);
                Assert.True(string.IsNullOrEmpty(viewModel.Breed.Name));

                CheckMessage(viewModel, "A Breed must have a name.");
            }
        }

        [Fact]
        public async void GetDetailsWithIndividuals()
        {
            using (var database = new TestDb())
            {
                var controller = new BreedsController(database.Context);

                IActionResult result = await controller.Details(8);

                BreedViewModel viewModel = CheckViewModel(result);
                Breed breed = database.Context.Breeds.SingleOrDefault(b => b.Id == 8);

                Assert.Equal(8, viewModel.Breed.Id);
                Assert.Equal(breed.Name, viewModel.Breed.Name);
                Assert.Equal(viewModel.Individuals.Count(), database.Context.Individuals.Count(m => m.Breed.Id == 8));
            }
        }
        
        [Fact]
        public async void GetEditFound()
        {
            using (var database = new TestDb())
            {

                var controller = new BreedsController(database.Context);

                IActionResult result = await controller.Edit(2);

                BreedViewModel viewModel = CheckViewModel(result);
                Breed breed = database.Context.Breeds.SingleOrDefault(b => b.Id == 2);

                Assert.Equal(2, viewModel.Breed.Id);
                Assert.Equal(breed.Name, viewModel.Breed.Name);
            }
        }

        [Fact]
        public async void GetEditNullId()
        {
            using (var database = new TestDb())
            {

                var controller = new BreedsController(database.Context);

                IActionResult result = await controller.Edit(null);

                BreedViewModel viewModel = CheckViewModel(result);

                CheckMessage(viewModel, "The Breed ID provided was invalid.");
            }
        }

        [Fact]
        public async void GetEditZeroId()
        {
            using (var database = new TestDb())
            {

                var controller = new BreedsController(database.Context);

                IActionResult result = await controller.Edit(0);

                BreedViewModel viewModel = CheckViewModel(result);

                CheckMessage(viewModel, $"The Breed ID must be between 1 and {int.MaxValue}.");
            }
        }

        [Fact]
        public async void GetEditNegativeId()
        {
            using (var database = new TestDb())
            {

                var controller = new BreedsController(database.Context);

                IActionResult result = await controller.Edit(-1);

                BreedViewModel viewModel = CheckViewModel(result);

                CheckMessage(viewModel, $"The Breed ID must be between 1 and {int.MaxValue}.");
            }
        }

        [Fact]
        public async void GetEditNotFound()
        {
            using (var database = new TestDb())
            {

                var controller = new BreedsController(database.Context);

                IActionResult result = await controller.Edit(50);

                BreedViewModel viewModel = CheckViewModel(result);

                CheckMessage(viewModel, "The selected Breed could not be found.");
            }
        }

        [Fact]
        public async void PostEditSuccess()
        {
            using (var database = new TestDb())
            {
                var controller = new BreedsController(database.Context);

                IActionResult result = await controller.Edit(6, new Breed(6, "Modified Breed"));

                RedirectToActionResult redirect = Assert.IsType<RedirectToActionResult>(result);

                Assert.Equal("Index", redirect.ActionName);

                Breed breed = database.Context.Breeds.SingleOrDefault(b => b.Id == 6);

                Assert.Equal("Modified Breed", breed.Name);    
            }
        }

        [Fact]
        public async void PostEditNullId()
        {
            using (var database = new TestDb())
            {

                var controller = new BreedsController(database.Context);

                IActionResult result = await controller.Edit(null);

                BreedViewModel viewModel = CheckViewModel(result);

                CheckMessage(viewModel, "The Breed ID provided was invalid.");
            }
        }

        [Fact]
        public async void PostEditZeroId()
        {
            using (var database = new TestDb())
            {

                var controller = new BreedsController(database.Context);

                IActionResult result = await controller.Edit(6, new Breed(6));

                BreedViewModel viewModel = CheckViewModel(result);

                CheckMessage(viewModel, "A Breed must have a name.");
            }
        }

        [Fact]
        public async void PostEditNegativeId()
        {
            using (var database = new TestDb())
            {

                var controller = new BreedsController(database.Context);

                IActionResult result = await controller.Edit(-1, new Breed(-1, string.Empty));

                BreedViewModel viewModel = CheckViewModel(result);

                CheckMessage(viewModel, $"The Breed ID must be between 1 and {int.MaxValue}.");
            }
        }

        [Fact]
        public async void PostEditNotFound()
        {
            using (var database = new TestDb())
            {

                var controller = new BreedsController(database.Context);

                IActionResult result = await controller.Edit(50, new Breed(50, "Test Breed 6 Modified"));

                BreedViewModel viewModel = CheckViewModel(result);

                CheckMessage(viewModel, "The selected Breed could not be found.");
            }
        }

        [Fact]
        public async void GetIndex()
        {
            using (var database = new TestDb())
            {

                var controller = new BreedsController(database.Context);

                IActionResult result = await controller.Index();

                ViewResult viewResult = Assert.IsType<ViewResult>(result);
                IEnumerable<Breed> breeds = Assert.IsAssignableFrom<IEnumerable<Breed>>(viewResult.ViewData.Model);

                Assert.Equal(database.Context.Breeds.Count(), breeds.Count());
            }
        }
  
        [Fact]
        public async void GetDeleteSuccess()
        {
            using (var database = new TestDb())
            {
                var controller = new BreedsController(database.Context);

                IActionResult result = await controller.Delete(3);

                BreedViewModel viewModel = CheckViewModel(result);
                Breed breed = database.Context.Breeds.SingleOrDefault(b => b.Id == 3);

                Assert.Equal(3, viewModel.Breed.Id);
                Assert.Equal(breed.Name, viewModel.Breed.Name);
            }
        }

        [Fact]
        public async void GetDeleteNotFound()
        {
            using (var database = new TestDb())
            {

                var controller = new BreedsController(database.Context);

                IActionResult result = await controller.Delete(50);

                BreedViewModel viewModel = CheckViewModel(result);

                CheckMessage(viewModel, "The selected Breed could not be found.");
            }
        }

        [Fact]
        public async void GetDeleteNullId()
        {
            using (var database = new TestDb())
            {

                var controller = new BreedsController(database.Context);

                IActionResult result = await controller.Delete(null);

                BreedViewModel viewModel = CheckViewModel(result);

                CheckMessage(viewModel, "The Breed ID provided was invalid.");
            }
        }

        [Fact]
        public async void GetDeleteZeroId()
        {
            using (var database = new TestDb())
            {

                var controller = new BreedsController(database.Context);

                IActionResult result = await controller.Delete(0);

                BreedViewModel viewModel = CheckViewModel(result);

                CheckMessage(viewModel, $"The Breed ID must be between 1 and {int.MaxValue}.");
            }
        }

        [Fact]
        public async void GetDeleteNegativeId()
        {
            using (var database = new TestDb())
            {

                var controller = new BreedsController(database.Context);

                IActionResult result = await controller.Delete(-1);

                BreedViewModel viewModel = CheckViewModel(result);

                CheckMessage(viewModel, $"The Breed ID must be between 1 and {int.MaxValue}.");
            }
        }

        [Fact]
        public async void PostDeleteConfirmNotFound()
        {
            using (var database = new TestDb())
            {

                var controller = new BreedsController(database.Context);

                IActionResult result = await controller.DeleteConfirmed(50);

                RedirectToActionResult redirect = Assert.IsType<RedirectToActionResult>(result);

                Assert.Equal("Index", redirect.ActionName);
            }
        }

        [Fact]
        public async void PostDeleteConfirmZeroId()
        {
            using (var database = new TestDb())
            {

                var controller = new BreedsController(database.Context);

                IActionResult result = await controller.DeleteConfirmed(0);

                RedirectToActionResult redirect = Assert.IsType<RedirectToActionResult>(result);

                Assert.Equal("Index", redirect.ActionName);
            }
        }

        [Fact]
        public async void PostDeleteConfirmSuccess()
        {
            using (var database = new TestDb())
            {
                Assert.True(database.Context.Individuals.Any(i => i.Id == 6));

                var controller = new BreedsController(database.Context);

                IActionResult result = await controller.DeleteConfirmed(3);

                RedirectToActionResult redirect = Assert.IsType<RedirectToActionResult>(result);

                Assert.Equal("Index", redirect.ActionName);

                Assert.False(database.Context.Breeds.Any(i => i.Id == 3));
            }
        }
    }
}