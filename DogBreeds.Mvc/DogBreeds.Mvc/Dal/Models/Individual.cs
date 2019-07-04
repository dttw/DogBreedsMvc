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

        public Individual(string name, int breedId)
        {
            Name = name;
            BreedId = breedId;
        }

        public Individual(int id, string name, int breedId)
        {
            Id = id;
            Name = name;
            BreedId = breedId;
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Display(Name = "Name")]
        [Required(ErrorMessage = "You must enter a name for this Individual.")]
        [StringLength(255, MinimumLength = 1, ErrorMessage = "You must enter a name for this Individual.")]
        public string Name { get; set; }

        [Display(Name = "Breed")]
        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "You must select a Breed for this Individual.")]
        public int BreedId { get; set; }

        public Breed Breed { get; set; } = new Breed();
    }
}
