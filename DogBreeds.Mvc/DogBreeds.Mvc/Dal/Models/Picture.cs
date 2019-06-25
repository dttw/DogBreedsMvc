using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DogBreeds.Mvc.Dal.Models
{
    public class Picture
    {
        public Picture() { }

        public int Id { get; set; }

        public string Uri { get; set; }

        public int? IndividualId { get; set; }

        public Individual Individual { get; set; }

        public int? BreedId { get; set; }

        public Breed Breed { get; set; }
    }
}
