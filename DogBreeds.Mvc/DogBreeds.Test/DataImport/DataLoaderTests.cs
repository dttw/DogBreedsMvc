using DogBreeds.Mvc.Dal;
using DogBreeds.Mvc.Dal.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;

namespace DogBreeds.Test.DataImport
{
    public class DataLoaderTests
    {
        [Fact]
        public void ImportBreedsValid()
        {
            using (var database = new TestDb())
            {
                var loader = new DataLoader(database.Context);

                Assert.False(database.Context.Breeds.Any(b => b.Name == "Affenpinscher"));
                Assert.False(database.Context.Breeds.Any(b => b.Name == "African"));

                loader.ImportBreeds(Encoding.ASCII.GetBytes(@"{""affenpinscher"": [], ""african"": [] }"));

                Assert.True(database.Context.Breeds.Any(b => b.Name == "Affenpinscher"));
                Assert.True(database.Context.Breeds.Any(b => b.Name == "African"));
            }
        }

        [Fact]
        public void ImportCreatedWithParentBreed()
        {
            using (var database = new TestDb())
            {
                var loader = new DataLoader(database.Context);

                loader.ImportBreeds(Encoding.ASCII.GetBytes(@"{""bulldog"": [""boston"", ""french""]}"));

                Breed bulldog = database.Context.Breeds.SingleOrDefault(b => b.Name == "Bulldog");

                Assert.NotNull(bulldog);

                Breed boston = database.Context.Breeds.SingleOrDefault(b => b.Name == "Boston");

                Assert.NotNull(boston);

                Assert.Equal(boston.BreedId, bulldog.Id);
            }
        }

        [Fact]
        public void ImportBreedsIgnoreDuplicate()
        {
            string json = @"{""bullterrier"": [], ""bullterrier"": [], ""bullterrier"": [], ""bullterrier"": [] }";

            using (var database = new TestDb())
            {
                var loader = new DataLoader(database.Context);

                loader.ImportBreeds(Encoding.ASCII.GetBytes(json));

                Assert.Equal(1, database.Context.Breeds.Count(b => b.Name == "Bullterrier"));
            }
        }

        [Fact]
        public void ImportBreedsEmptyFile()
        {
            using (var database = new TestDb())
            {
                var loader = new DataLoader(database.Context);

                Exception ex = Assert.Throws<ArgumentException>(() => loader.ImportBreeds(Array.Empty<byte>()));

                Assert.Equal("The file containing the initial Breeds is empty.", ex.Message);
            }
        }

        [Fact]
        public void ImportBreedsInvalidJson()
        {
            using (var database = new TestDb())
            {
                var loader = new DataLoader(database.Context);

                Exception ex = Assert.Throws<ArgumentException>(() => loader.ImportBreeds(Encoding.ASCII.GetBytes(@"{""affenpinscher"": [], ""african"": [],")));
                    
                Assert.Equal("The JSON provided does not match the required format.", ex.Message);
            }
        }
    }
}
