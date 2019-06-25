using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace DogBreeds.Mvc.Dal.Models
{
    public class Individual
    {
        public Individual() { }

        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        public int BreedId { get; set; }

        public Breed Breed { get; set; }
    }
}
