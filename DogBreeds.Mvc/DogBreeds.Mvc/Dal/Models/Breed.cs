using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

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

        [Display(Name = "Name")]
        [Required(ErrorMessage = "You must enter a name for this Breed.")]
        [StringLength(255, MinimumLength = 1, ErrorMessage = "You must enter a name for this Breed.")]
        public string Name { get; set; }

        public ICollection<Individual> Individuals { get; set; }

        public ICollection<Breed> Breeds { get; set; }

        [NotMapped]
        public string FullName => GetFullName();

        [Display(Name = "Type of Breed")]
        public int? BreedId { get; set; }

        [Display(Name = "Type of Breed")]
        public Breed ParentBreed { get; set; }

        private string GetFullName()
        {
            if (ParentBreed is null)
            {
                return Name;
            }

            return $"{Name} {ParentBreed.Name}";
        }
    }
}
