using DogBreeds.Mvc.Dal.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

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
