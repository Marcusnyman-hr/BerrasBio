using BerrasBio1._0.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BerrasBio1._0.Services
{
    public interface IMovieService
    {
        public List<Movie> GetAllMoviesFromDb();
        public void FetchUpcomingMoviesFromApi(int amountOfPagesToFetch);
        public void SeedFromSqlFile();
        public List<Movie> GetMoviesByAmount(int amount);
    }
}
