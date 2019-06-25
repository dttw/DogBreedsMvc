using DogBreeds.Mvc.Dal.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace DogBreeds.Mvc.Models
{
    public class BreedViewModel
    {
        public BreedViewModel() { }

        public BreedViewModel(Breed breed) : this(breed.Id, breed.Name)
        {
        }

        public BreedViewModel(string name) => Name = name;

        public BreedViewModel(int id, string name) : this(name) => Id = id;

        public int Id { get; set; }

        [Display(Name = "Name")]
        [Required(ErrorMessage = "{0} is required.")]
        public string Name { get; set; }

        public IEnumerable<Individual> Individuals { get; set; } = new List<Individual>();

        public IEnumerable<Picture> Pictures { get; set; } = new List<Picture>();
    }
}
