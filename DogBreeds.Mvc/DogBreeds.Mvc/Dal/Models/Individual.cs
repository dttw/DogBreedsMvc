using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DogBreeds.Mvc.Dal.Models
{
    public class Individual
    {
        public Individual() { }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [MinLength(1, ErrorMessage = "You must enter a Name for this Individual")]
        public string Name { get; set; }

        [Required]
        public int BreedId { get; set; }

        public Breed Breed { get; set; }
    }
}
