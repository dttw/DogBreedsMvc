using DogBreeds.Mvc.Controllers;
using DogBreeds.Mvc.Dal.Models;
using DogBreeds.Mvc.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using Xunit;
using System.Linq;
using DogBreeds.Mvc.Dal;

namespace DogBreeds.Test
{
    public class BreedControllerTests : IClassFixture<TestDb>
    { 
        TestDb database;

        public BreedControllerTests(TestDb database) => this.database = database;

        [Fact]
        public async void DetailsFound()
        {            
            var controller = new BreedsController(database.Context);

            IActionResult result = await controller.Details(8);

            ViewResult viewResult = Assert.IsType<ViewResult>(result);
            BreedViewModel breed = Assert.IsAssignableFrom<BreedViewModel>(viewResult.ViewData.Model);

            Assert.Equal(8, breed.Id);
            Assert.Equal("Test Breed 8", breed.Name);
        }

        [Fact]
        public async void DetailsNullId()
        {
            var controller = new BreedsController(database.Context);

            IActionResult result = await controller.Details(null);

            NotFoundResult viewResult = Assert.IsType<NotFoundResult>(result);

            Assert.Equal(404, viewResult.StatusCode);
        }

        [Fact]
        public async void DetailsNotFound()
        {
            var controller = new BreedsController(database.Context);

            IActionResult result = await controller.Details(null);

            NotFoundResult viewResult = Assert.IsType<NotFoundResult>(result);

            Assert.Equal(404, viewResult.StatusCode);
        }

        [Fact]
        public  void GetCreate()
        {
            var controller = new BreedsController(database.Context);

            IActionResult result = controller.Create();

            ViewResult viewResult = Assert.IsType<ViewResult>(result);
            BreedViewModel breed = Assert.IsAssignableFrom<BreedViewModel>(viewResult.ViewData.Model);

            Assert.Equal(0, breed.Id);
            Assert.True(string.IsNullOrEmpty(breed.Name));
        }

        [Fact]
        public async void PostCreateOk()
        {
            var controller = new BreedsController(database.Context);

            IActionResult result = await controller.Create(new DogBreeds.Mvc.Dal.Models.Breed("Test Breed 11"));

            RedirectToActionResult redirect = Assert.IsType<RedirectToActionResult>(result);

            Assert.Equal("Index", redirect.ActionName);

            Breed breed = database.Context.Breeds.LastOrDefault();

            Assert.Equal("Test Breed 11", breed.Name);
            Assert.Equal(11, breed.Id);
        }

        [Fact]
        public async void CreateNoName()
        {
            var controller = new BreedsController(database.Context);

            IActionResult result = await controller.Create(new Breed());

            ViewResult viewResult = Assert.IsType<ViewResult>(result);
            BreedViewModel breed = Assert.IsAssignableFrom<BreedViewModel>(viewResult.ViewData.Model);

            Assert.Equal(0, breed.Id);
            Assert.True(string.IsNullOrEmpty(breed.Name));

            Assert.Equal(1, breed.Messages.Count);
            Assert.Equal("You cannot add a Breed without a name.", breed.Messages.FirstOrDefault());
        }

        [Fact]
        public async void GetBreedWithIndividuals()
        {
            var controller = new BreedsController(database.Context);

            IActionResult result = await controller.Details(8);

            ViewResult viewResult = Assert.IsType<ViewResult>(result);
            BreedViewModel breed = Assert.IsAssignableFrom<BreedViewModel>(viewResult.ViewData.Model);

            Assert.Equal(8, breed.Id);
            Assert.Equal("Test Breed 8", breed.Name);
            Assert.Single(breed.Individuals);
        }
        
        [Fact]
        public async void GetEditFound()
        {
            var controller = new BreedsController(database.Context);

            IActionResult result = await controller.Edit(2);

            ViewResult viewResult = Assert.IsType<ViewResult>(result);
            BreedViewModel breed = Assert.IsAssignableFrom<BreedViewModel>(viewResult.ViewData.Model);

            Assert.Equal(2, breed.Id);
            Assert.Equal("Test Breed 2", breed.Name);
        }

        [Fact]
        public async void GetEditNullId()
        {
            var controller = new BreedsController(database.Context);

            IActionResult result = await controller.Edit(null);

            NotFoundResult viewResult = Assert.IsType<NotFoundResult>(result);

            Assert.Equal(404, viewResult.StatusCode);
        }

        [Fact]
        public async void GetEditNotFound()
        {
            var controller = new BreedsController(database.Context);

            IActionResult result = await controller.Edit(null);

            NotFoundResult viewResult = Assert.IsType<NotFoundResult>(result);

            Assert.Equal(404, viewResult.StatusCode);
        }

        [Fact]
        public async void PostEditOk()
        {
            var controller = new BreedsController(database.Context);

            IActionResult result = await controller.Edit(6);

            ViewResult viewResult = Assert.IsType<ViewResult>(result);
            BreedViewModel breed = Assert.IsAssignableFrom<BreedViewModel>(viewResult.ViewData.Model);

            Assert.Equal(6, breed.Id);
            Assert.Equal("Test Breed 6", breed.Name);
        }

        [Fact]
        public async void PostEditNullId()
        {
            var controller = new BreedsController(database.Context);

            IActionResult result = await controller.Edit(null);

            NotFoundResult viewResult = Assert.IsType<NotFoundResult>(result);

            Assert.Equal(404, viewResult.StatusCode);
        }

        [Fact]
        public async void PostEditInvalidModel()
        {
            var controller = new BreedsController(database.Context);

            IActionResult result = await controller.Edit(6, new Breed(6));

            ViewResult viewResult = Assert.IsType<ViewResult>(result);
            BreedViewModel breed = Assert.IsAssignableFrom<BreedViewModel>(viewResult.ViewData.Model);

            Assert.Equal(1, breed.Messages.Count);
            Assert.Equal("You cannot edit a Breed without entering a name.", breed.Messages.FirstOrDefault());
        }
        
        [Fact]
        public async void PostEditNotFound()
        {
            var controller = new BreedsController(database.Context);

            IActionResult result = await controller.Edit(6, new Breed(7, "Test Breed 6 Modified"));

            NotFoundResult viewResult = Assert.IsType<NotFoundResult>(result);

            Assert.Equal(404, viewResult.StatusCode);
        }

        [Fact]
        public async void GetBreeds()
        {
            var controller = new BreedsController(database.Context);

            IActionResult result = await controller.Index();

            ViewResult viewResult = Assert.IsType<ViewResult>(result);
            IEnumerable<Breed> breeds = Assert.IsAssignableFrom<IEnumerable<Breed>>(viewResult.ViewData.Model);

            Assert.Equal(10, breeds.Count());
        }
  
        [Fact]
        public async void GetDeleteFound()
        {
            var controller = new BreedsController(database.Context);

            IActionResult result = await controller.Delete(3);

            ViewResult viewResult = Assert.IsType<ViewResult>(result);
            BreedViewModel breed = Assert.IsAssignableFrom<BreedViewModel>(viewResult.ViewData.Model);

            Assert.Equal(3, breed.Id);
            Assert.Equal("Test Breed 3", breed.Name);
        }

        [Fact]
        public async void GetDeleteNotFound()
        {
            var controller = new BreedsController(database.Context);

            IActionResult result = await controller.Delete(50);

            NotFoundResult viewResult = Assert.IsType<NotFoundResult>(result);

            Assert.Equal(404, viewResult.StatusCode);
        }

        [Fact]
        public async void GetDeleteNullId()
        {
            var controller = new BreedsController(database.Context);

            IActionResult result = await controller.Delete(null);

            NotFoundResult viewResult = Assert.IsType<NotFoundResult>(result);

            Assert.Equal(404, viewResult.StatusCode);
        }

        [Fact]
        public async void PostDeleteConfirmNotFound()
        {
            var controller = new BreedsController(database.Context);

            IActionResult result = await controller.Delete(50);
            
            NotFoundResult viewResult = Assert.IsType<NotFoundResult>(result);

            Assert.Equal(404, viewResult.StatusCode);
        }

        [Fact]
        public async void PostDeleteConfirmNullId()
        {
            var controller = new BreedsController(database.Context);

            IActionResult result = await controller.Delete(null);

            NotFoundResult viewResult = Assert.IsType<NotFoundResult>(result);

            Assert.Equal(404, viewResult.StatusCode);
        }

        [Fact]
        public async void PostDeleteConfirmSuccess()
        {
            int currentBreedCount = database.Context.Breeds.Count();

            var controller = new BreedsController(database.Context);

            IActionResult result = await controller.DeleteConfirmed(6);

            RedirectToActionResult redirect = Assert.IsType<RedirectToActionResult>(result);

            Assert.Equal("Index", redirect.ActionName);

            Assert.Equal((currentBreedCount -1), database.Context.Breeds.Count());
        }
    }
}