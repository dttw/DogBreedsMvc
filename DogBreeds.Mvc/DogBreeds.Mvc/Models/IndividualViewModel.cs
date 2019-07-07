using DogBreeds.Mvc.Dal.Models;
using System.Collections.Generic;

namespace DogBreeds.Mvc.Models
{
    public class IndividualViewModel
    {
        public IndividualViewModel() { }

        public IndividualViewModel(Individual individual) => Individual = individual;

        public IndividualViewModel(string name, int breedId) => Individual = new Individual(name, breedId);

        public IndividualViewModel(int id, string name, int breedId) => Individual = new Individual(id, name, breedId);

        public Individual Individual { get; private set; } = new Individual();

        public IEnumerable<Breed> Breeds { get; set; } = new List<Breed>();

        public IList<string> Messages { get; set; } = new List<string>();
    }
}
