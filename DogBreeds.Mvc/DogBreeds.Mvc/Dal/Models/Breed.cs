using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace DogBreeds.Mvc.Dal.Models
{
    public class Breed
    {
        public Breed() { }

        public Breed(string name) => Name = name;

        public Breed(int id) => Id = id;

        public Breed(int id, string name) : this(id) => Name = name;

        public int Id { get; set; }

        [Required]
        public string Name { get; set; }
    }
}
