using DogBreeds.Mvc.Dal.Models;
using Microsoft.EntityFrameworkCore;

namespace DogBreeds.Mvc.Dal
{
    public class DogBreedsContext : DbContext
    {
        public DogBreedsContext(DbContextOptions<DogBreedsContext> options)
             : base(options) { }

        public DbSet<Breed> Breeds { get; set; }

        public DbSet<Individual> Individuals { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Individual>().HasOne(s => s.Breed).WithMany(g => g.Individuals).HasForeignKey(s => s.BreedId);
        }
    }
}
