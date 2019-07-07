using DogBreeds.Mvc.Controllers;
using DogBreeds.Mvc.Dal.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DogBreeds.Mvc.Dal
{
    public class DataLoader
    {
        private readonly DogBreedsContext context;
        private TextInfo textInfo = new CultureInfo("en-GB", false).TextInfo;

        public DataLoader(DogBreedsContext context) => this.context = context;

        public void ImportBreeds()
        {
            byte[] fileContents = Json.Breeds.InitialBreeds;

            ImportBreeds(fileContents);
        }

        public void ImportBreeds(byte[] fileContents)
        {
            if (fileContents.Length == 0)
            {
                throw new ArgumentException("The file containing the initial Breeds is empty.");
            }

            string json = GetJson(fileContents);

            try
            {
                Dictionary<string, string[]> breeds = GetBreedsFromJson(json);

                if (breeds != null && breeds.Count() > 0)
                {
                    AddBreeds(breeds);
                }
            }
            catch(Exception ex)
            {
                throw new ArgumentException("The JSON provided does not match the required format.");
            }            
        }

        private string GetJson(byte[] fileContents) => Encoding.UTF8.GetString(fileContents, 0, fileContents.Length);

        private Dictionary<string, string[]> GetBreedsFromJson(string json) => JsonConvert.DeserializeObject<Dictionary<string, string[]>>(json);

        private void AddBreeds(Dictionary<string, string[]> breeds)
        {          
            foreach (KeyValuePair<string, string[]> breed in breeds)
            {
                if (!string.IsNullOrEmpty(breed.Key))
                {
                    string properName = textInfo.ToTitleCase(breed.Key);

                    AddBreed(new Breed(properName));

                    if (breed.Value.Count() > 0)
                    {
                        AddChildBreeds(properName, breed.Value);
                    }                   
                }
            }
        }

        private void AddBreed(Breed breed)
        {
            if (!BreedExists(breed.Name))
            {
                context.Breeds.Add(breed);
                context.SaveChanges();
            }           
        }

        private void AddChildBreeds(string parentBreedName, string[] childBreeds)
        {
            Breed parentBreed = context.Breeds.SingleOrDefault(b => b.Name == parentBreedName);

            if (parentBreed != null)
            {
                AddChildBreeds(parentBreed, childBreeds);
            }      
        }

        private void AddChildBreeds(Breed parentBreed, string[] childBreeds)
        {
            foreach (string breed in childBreeds)
            {
                if (!string.IsNullOrEmpty(breed))
                {
                    AddBreed(new Breed(textInfo.ToTitleCase(breed)) { BreedId = parentBreed.Id });
                }                
            }
        }

        private bool BreedExists(string name) => context.Breeds.Any(b => b.Name == name);
    }
}