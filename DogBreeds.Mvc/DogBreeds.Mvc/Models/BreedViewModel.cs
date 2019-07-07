using DogBreeds.Mvc.Dal.Models;
using System.Collections.Generic;

namespace DogBreeds.Mvc.Models
{
    public class BreedViewModel
    {
        public BreedViewModel() { }

        public BreedViewModel(Breed breed) => Breed = breed;

        public BreedViewModel(string name) => Breed = new Breed(name);

        public BreedViewModel(int id, string name) : this(name) => Breed = new Breed(id, name);

        public Breed Breed { get; private set; } = new Breed();

        public IEnumerable<Individual> Individuals { get; set; } = new List<Individual>();

        public IEnumerable<Breed> ChildBreeds { get; set; } = new List<Breed>();

        public IEnumerable<Breed> Breeds { get; set; } = new List<Breed>();
        
        public IList<string> Messages { get; set; } = new List<string>();
    }
}
