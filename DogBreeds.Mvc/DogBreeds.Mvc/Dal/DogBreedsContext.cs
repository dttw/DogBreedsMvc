using DogBreeds.Mvc.Dal.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DogBreeds.Mvc.Dal
{
    public class DogBreedsContext : DbContext
    {
        public DogBreedsContext(DbContextOptions<DogBreedsContext> options)
             : base(options) { }

        public DbSet<Breed> Breeds { get; set; }

        public DbSet<Individual> Individuals { get; set; }
    }
}
