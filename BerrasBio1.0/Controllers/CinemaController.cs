using BerrasBio1._0.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BerrasBio1._0.Controllers
{
    public class CinemaController : Controller
    {
        private readonly IMovieService _movieService;
        public CinemaController(IMovieService movieService)
        {
            _movieService = movieService;
        }
        public IActionResult Index()
        {
            //if(_movieService.GetAllMoviesFromDb().Count < 0 || _movieService.GetAllMoviesFromDb() == null)
            //{
            //_movieService.FetchUpcomingMoviesFromApi(10);
            //}
            return View();
        }
    }
}
