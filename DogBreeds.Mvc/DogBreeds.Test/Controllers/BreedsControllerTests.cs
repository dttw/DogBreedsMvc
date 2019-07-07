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
    public class BreedControllerTests
    { 
        private BreedViewModel CheckViewModel(IActionResult result)
        {
            ViewResult viewResult = Assert.IsType<ViewResult>(result);
            BreedViewModel viewModel = Assert.IsAssignableFrom<BreedViewModel>(viewResult.ViewData.Model);

            return viewModel;
        }

        private void CheckMessage(IActionResult result, string message)
        {
            BreedViewModel viewModel = CheckViewModel(result);

            Assert.True(viewModel.Messages.Any());
            Assert.Equal(message, viewModel.Messages.FirstOrDefault());
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

        #region Create
        #region Get
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
        #endregion

        #region Post 
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

                CheckMessage(result, "A Breed must have a name.");
            }
        }

        [Fact]
        public async void PostCreateParentBreedNull()
        {
            using (var database = new TestDb())
            {
                var controller = new BreedsController(database.Context);

                IActionResult result = await controller.Create(new Breed { Name = "Test Breed with Null Parent Breed", BreedId = null });

                RedirectToActionResult redirect = Assert.IsType<RedirectToActionResult>(result);

                Assert.Equal("Index", redirect.ActionName);

                Breed breed = database.Context.Breeds.LastOrDefault();

                Assert.Equal("Test Breed with Null Parent Breed", breed.Name);
            }
        }

        [Fact]
        public async void PostCreateParentBreedZero()
        {
            using (var database = new TestDb())
            {
                var controller = new BreedsController(database.Context);

                IActionResult result = await controller.Create(new Breed { Name = "Test Breed with Zero Parent Breed", BreedId = 0 });

                RedirectToActionResult redirect = Assert.IsType<RedirectToActionResult>(result);

                Assert.Equal("Index", redirect.ActionName);

                Breed breed = database.Context.Breeds.LastOrDefault();

                Assert.Equal("Test Breed with Zero Parent Breed", breed.Name);
            }
        }

        [Fact]
        public async void PostCreateParentBreedNegative()
        {
            using (var database = new TestDb())
            {
                var controller = new BreedsController(database.Context);

                IActionResult result = await controller.Create(new Breed { Name = "Test Breed with Negative Parent Breed", BreedId = -1 });
          
                CheckMessage(result, $"The Parent Breed ID cannot be negative or greater than {int.MaxValue}.");
            }
        }

        [Fact]
        public async void PostCreateParentBreedDoesntExist()
        {
            using (var database = new TestDb())
            {
                var controller = new BreedsController(database.Context);

                IActionResult result = await controller.Create(new Breed { Name = "Test Breed with Invalid Parent Breed", BreedId = 500 });

                CheckMessage(result, "The parent Breed selected could not be found.");
            }
        }

        [Fact]
        public async void PostCreateWithParentBreedSuccess()
        {
            using (var database = new TestDb())
            {
                var controller = new BreedsController(database.Context);

                IActionResult result = await controller.Create(new Breed { Name = "Test Breed with Parent Breed", BreedId = 1 });

                RedirectToActionResult redirect = Assert.IsType<RedirectToActionResult>(result);

                Assert.Equal("Index", redirect.ActionName);

                Breed breed = database.Context.Breeds.SingleOrDefault(b => b.Name == "Test Breed with Parent Breed");

                Assert.Equal("Test Breed with Parent Breed", breed.Name);
                Assert.Equal(1, breed.ParentBreed.Id);
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

                CheckMessage(result, "The Breed ID provided was invalid.");
            }
        }

        [Fact]
        public async void GetDetailsZeroId()
        {
            using (var database = new TestDb())
            {
                var controller = new BreedsController(database.Context);

                IActionResult result = await controller.Details(0);

                CheckMessage(result, $"The Breed ID must be between 1 and {int.MaxValue}.");
            }
        }

        [Fact]
        public async void GetDetailsNegativeId()
        {
            using (var database = new TestDb())
            {
                var controller = new BreedsController(database.Context);

                IActionResult result = await controller.Details(-1);

                CheckMessage(result, $"The Breed ID must be between 1 and {int.MaxValue}.");
            }
        }

        [Fact]
        public async void GetDetailsNotFound()
        {
            using (var database = new TestDb())
            {
                var controller = new BreedsController(database.Context);

                IActionResult result = await controller.Details(50);

                CheckMessage(result, "The selected Breed could not be found.");
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
        #endregion
        #endregion

        #region Edit 
        #region Get
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

                CheckMessage(result, "The Breed ID provided was invalid.");
            }
        }

        [Fact]
        public async void GetEditZeroId()
        {
            using (var database = new TestDb())
            {
                var controller = new BreedsController(database.Context);

                IActionResult result = await controller.Edit(0);

                CheckMessage(result, $"The Breed ID must be between 1 and {int.MaxValue}.");
            }
        }

        [Fact]
        public async void GetEditNegativeId()
        {
            using (var database = new TestDb())
            {
                var controller = new BreedsController(database.Context);

                IActionResult result = await controller.Edit(-1);

                CheckMessage(result, $"The Breed ID must be between 1 and {int.MaxValue}.");
            }
        }

        [Fact]
        public async void GetEditNotFound()
        {
            using (var database = new TestDb())
            {
                var controller = new BreedsController(database.Context);

                IActionResult result = await controller.Edit(50);

                CheckMessage(result, "The selected Breed could not be found.");
            }
        }
        #endregion

        #region Post 
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
        public async void PostEditParentBreedNull()
        {
            using (var database = new TestDb())
            {
                var controller = new BreedsController(database.Context);

                IActionResult result = await controller.Edit(10, new Breed { Id = 10, Name = "Test Breed with Null Parent Breed", BreedId = null });

                RedirectToActionResult redirect = Assert.IsType<RedirectToActionResult>(result);

                Assert.Equal("Index", redirect.ActionName);

                Breed breed = database.Context.Breeds.SingleOrDefault(b => b.Id == 10);

                Assert.Equal("Test Breed with Null Parent Breed", breed.Name);
            }
        }

        [Fact]
        public async void PostEditParentSetAsOwnParent()
        {
            using (var database = new TestDb())
            {
                var controller = new BreedsController(database.Context);

                IActionResult result = await controller.Edit(1, new Breed { Id = 1, Name = "Test Breed with Null Parent Breed", BreedId = 1 });
              
                CheckMessage(result, "A breed cannot be set as it's own Parent Breed.");
            }
        }

        [Fact]
        public async void PostEditParentBreedZero()
        {
            using (var database = new TestDb())
            {
                var controller = new BreedsController(database.Context);

                IActionResult result = await controller.Edit(1, new Breed { Id = 1, Name = "Test Modified Breed with Zero Parent Breed", BreedId = 0 });

                RedirectToActionResult redirect = Assert.IsType<RedirectToActionResult>(result);

                Assert.Equal("Index", redirect.ActionName);

                Breed breed = database.Context.Breeds.SingleOrDefault(b => b.Name == "Test Modified Breed with Zero Parent Breed");

                Assert.Equal(1, breed.Id);
            }
        }

        [Fact]
        public async void PostEditParentBreedNegative()
        {
            using (var database = new TestDb())
            {
                var controller = new BreedsController(database.Context);

                IActionResult result = await controller.Edit(1, new Breed { Id = 1, Name = "Test Breed with Negative Parent Breed", BreedId = -1 });

                CheckMessage(result, $"The Parent Breed ID cannot be negative or greater than {int.MaxValue}.");
            }
        }

        [Fact]
        public async void PostEditParentBreedDoesntExist()
        {
            using (var database = new TestDb())
            {
                var controller = new BreedsController(database.Context);

                IActionResult result = await controller.Edit(1, new Breed { Id = 1, Name = "Test Breed with Invalid Parent Breed", BreedId = 500 });

                BreedViewModel viewModel = CheckViewModel(result);

                CheckMessage(result, "The parent Breed selected could not be found.");
            }
        }

        [Fact]
        public async void PostEditWithParentBreedSuccess()
        {
            using (var database = new TestDb())
            {
                var controller = new BreedsController(database.Context);

                IActionResult result = await controller.Edit(7, new Breed { Id = 7, Name = "Test Modified Breed with Parent Breed", BreedId = 2 });

                RedirectToActionResult redirect = Assert.IsType<RedirectToActionResult>(result);

                Assert.Equal("Index", redirect.ActionName);

                Breed breed = database.Context.Breeds.SingleOrDefault(b => b.Name == "Test Modified Breed with Parent Breed");

                Assert.Equal(7, breed.Id);
                Assert.Equal(2, breed.BreedId);
            }
        }

        [Fact]
        public async void PostEditNoName()
        {
            using (var database = new TestDb())
            {
                var controller = new BreedsController(database.Context);

                IActionResult result = await controller.Edit(1, new Breed(1, string.Empty));

                CheckMessage(result, "A Breed must have a name.");
            }
        }

        [Fact]
        public async void PostEditZeroId()
        {
            using (var database = new TestDb())
            {
                var controller = new BreedsController(database.Context);

                IActionResult result = await controller.Edit(0, new Breed(0));

                CheckMessage(result, $"The Breed ID must be between 1 and {int.MaxValue}.");
            }
        }

        [Fact]
        public async void PostEditMismatchedIds()
        {
            using (var database = new TestDb())
            {
                var controller = new BreedsController(database.Context);

                IActionResult result = await controller.Edit(7, new Breed(6));

                CheckMessage(result, "The Breed ID provided doesn't match the Breed being updated.");
            }
        }

        [Fact]
        public async void PostEditNegativeId()
        {
            using (var database = new TestDb())
            {
                var controller = new BreedsController(database.Context);

                IActionResult result = await controller.Edit(-1, new Breed(-1, string.Empty));

                CheckMessage(result, $"The Breed ID must be between 1 and {int.MaxValue}.");
            }
        }

        [Fact]
        public async void PostEditNotFound()
        {
            using (var database = new TestDb())
            {
                var controller = new BreedsController(database.Context);

                IActionResult result = await controller.Edit(50, new Breed(50, "Test Breed 6 Modified"));

                CheckMessage(result, "The selected Breed could not be found.");
            }
        }
        #endregion
        #endregion

        #region Delete
        #region Get
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

                CheckMessage(result, "The selected Breed could not be found.");
            }
        }

        [Fact]
        public async void GetDeleteNullId()
        {
            using (var database = new TestDb())
            {
                var controller = new BreedsController(database.Context);

                IActionResult result = await controller.Delete(null);

                CheckMessage(result, "The Breed ID provided was invalid.");
            }
        }

        [Fact]
        public async void GetDeleteZeroId()
        {
            using (var database = new TestDb())
            {
                var controller = new BreedsController(database.Context);

                IActionResult result = await controller.Delete(0);

                CheckMessage(result, $"The Breed ID must be between 1 and {int.MaxValue}.");
            }
        }

        [Fact]
        public async void GetDeleteNegativeId()
        {
            using (var database = new TestDb())
            {
                var controller = new BreedsController(database.Context);

                IActionResult result = await controller.Delete(-1);

                CheckMessage(result, $"The Breed ID must be between 1 and {int.MaxValue}.");
            }
        }
        #endregion

        #region Post 
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
        public async void PostDeleteConfirmNegativeId()
        {
            using (var database = new TestDb())
            {

                var controller = new BreedsController(database.Context);

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
                Assert.True(database.Context.Breeds.Any(i => i.Id == 9));
                Assert.False(database.Context.Breeds.Any(b => b.BreedId == 9));
                Assert.False(database.Context.Individuals.Any(b => b.BreedId == 9));

                var controller = new BreedsController(database.Context);

                IActionResult result = await controller.DeleteConfirmed(9);

                RedirectToActionResult redirect = Assert.IsType<RedirectToActionResult>(result);

                Assert.Equal("Index", redirect.ActionName);

                Assert.False(database.Context.Breeds.Any(i => i.Id == 9));
            }
        }
        #endregion
        #endregion
    }
}