﻿using DogBreeds.Mvc.Dal;
using DogBreeds.Mvc.Dal.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DogBreeds.Test
{
    public class TestDb : IDisposable
    {
        public TestDb() => CreateTestContext();

        internal DogBreedsContext Context { get; private set; }

        internal void CreateTestContext()
        {
            DbContextOptionsBuilder<DogBreedsContext> builder = new DbContextOptionsBuilder<DogBreedsContext>().UseInMemoryDatabase();

            builder.EnableSensitiveDataLogging();

            var context = new DogBreedsContext(builder.Options);

            Context = context;

            AddBreeds();
            AddIndividuals();       
        }

        internal void AddBreeds()
        {
            if (Context.Breeds.Count() == 0)
            {
                IEnumerable<Breed> breeds = Enumerable.Range(1, 10).Select(i => new Breed { Name = $"Test Breed {i}" });
                Context.Breeds.AddRange(breeds);

                Context.SaveChanges();
            }          
        }

        internal void AddIndividuals()
        {
            if (Context.Individuals.Count() == 0)
            {
                IEnumerable<Individual> individuals = Enumerable.Range(1, 8).Select(i => new Individual ($"Test Individual {i}", i));

                Context.Individuals.AddRange(individuals);

                Context.SaveChanges();

                Context.Individuals.Add(new Individual ("Fido", 2));
                Context.Individuals.Add(new Individual ("Rex", 8));

                Context.SaveChanges();
            }
        }
        
        public void Dispose() => Context.Dispose();
    }
}
