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

        [Display(Name = "Name")]
        [Required(ErrorMessage = "{0} is required.")]
        public string Name { get; set; }

        [Display(Name = "Breed")]
        [Required(ErrorMessage = "{0} is required.")]
        public int BreedId { get; set; }

        public IEnumerable<Breed> Breeds { get; set; } = new List<Breed>();

        [Display(Name = "Pictures")]
        public IEnumerable<Picture> Pictures { get; set; } = new List<Picture>();
    }
}
