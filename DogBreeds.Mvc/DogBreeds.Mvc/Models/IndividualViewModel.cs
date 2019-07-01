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

        public IndividualViewModel(Individual individual)
            : this(individual.Id, individual.Name, individual.BreedId)
        {
        }

        public IndividualViewModel(string name, int breedId)
        {
            Name = name;
            BreedId = breedId;
        }

        public IndividualViewModel(int id, string name, int breedId)
        {
            Id = id;
            Name = name;
            BreedId = breedId;
        }

        public int Id { get; set; }

        [Required(ErrorMessage = "You must enter a name for this Breed.")]
        [StringLength(255, MinimumLength = 1, ErrorMessage = "You must enter a name for this Individual.")]
        public string Name { get; set; }

        [Display(Name = "Breed")]
        [Required(ErrorMessage = "You must select a Breed for this Individual.")]
        public int BreedId { get; set; }

        public IEnumerable<Breed> Breeds { get; set; } = new List<Breed>();

        public IList<string> Messages { get; set; } = new List<string>();
    }
}
