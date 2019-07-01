using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
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

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required(ErrorMessage = "You must enter a name for this Breed.")]
        [StringLength(255, MinimumLength = 1, ErrorMessage = "You must enter a name for this Breed.")]
        public string Name { get; set; }
    }
}
