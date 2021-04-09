using BerrasBio1._0.Data;
using BerrasBio1._0.Services;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BerrasBio1._0
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var host = CreateHostBuilder(args).Build();
            
            using (var scope = host.Services.CreateScope())
            {
                CinemaContext dbContext = scope.ServiceProvider.GetService<CinemaContext>();
                IMovieService movieService = scope.ServiceProvider.GetService<IMovieService>();
                //If database doesent exist, run the seeder from movieservice
                bool DatabaseDoesNotExist = dbContext.Database.EnsureCreated();
                if (DatabaseDoesNotExist)
                {
                    movieService.SeedFromSqlFile();
                    //movieService.FetchUpcomingMoviesFromApi(8);
                };
                //movieService.CreateShowings();
            }
            host.Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
