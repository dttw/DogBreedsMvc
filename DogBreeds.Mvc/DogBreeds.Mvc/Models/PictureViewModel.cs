using DogBreeds.Mvc.Dal.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace DogBreeds.Mvc.Models
{
    public class PictureViewModel
    {
        public PictureViewModel() { }

        public PictureViewModel(Picture picture) : this(picture.Id, picture.Uri, picture.IndividualId, picture.BreedId)
        {
        }

        public PictureViewModel(int id, string uri, int? individualId, int? breedId)
        {
            Id = id;
            Uri = uri;
            IndividualId = individualId;
            BreedId = breedId;
        }

        public int Id { get; set; }

        [Display(Name = "Location")]
        [Required(ErrorMessage = "{0} is required.")]
        public string Uri { get; set; }

        public int? IndividualId { get; set; }

        public Individual Individual { get; set; }

        public int? BreedId { get; set; }

        public Breed Breed { get; set; }
    }
}
