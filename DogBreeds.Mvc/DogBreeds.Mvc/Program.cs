using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using DogBreeds.Mvc.Dal;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace DogBreeds.Mvc
{
    public class Program
    {
        public static void Main(string[] args)
        {
            IWebHost host = BuildWebHost(args);

            using (IServiceScope scope = host.Services.CreateScope())
            {
                IServiceProvider services = scope.ServiceProvider;

                try
                {
                    DogBreedsContext context = services.GetRequiredService<DogBreedsContext>();
                    var dataLoader = new DataLoader(context);

                    dataLoader.ImportBreeds();
                }
                catch (Exception ex)
                {
                }
            }

            host.Run();
        }

        public static IWebHost BuildWebHost(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>()
                .Build();
    }
}
