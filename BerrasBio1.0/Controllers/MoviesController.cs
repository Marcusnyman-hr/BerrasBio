using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BerrasBio1._0.Data;
using BerrasBio1._0.Models;
using System.Collections;

namespace BerrasBio1._0.Controllers
{
    public class MoviesController : Controller
    {
        private readonly CinemaContext _context;

        public MoviesController(CinemaContext context)
        {
            _context = context;
        }

        // GET: Movies
        public async Task<IActionResult> Index()
        {
            return View(await _context.Movies.Include(m=>m.Genres).ToListAsync());
        }

        // GET: Movies/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            //include all relationals
            var movie = await _context.Movies
                .Include(m=>m.Genres)
                .Include(m=>m.Actors)
                .Include(m=>m.Director)
                .Include(m=>m.Producer)
                .Include(m=>m.Showings)
                .FirstOrDefaultAsync(m => m.Id == id);
            //Reformat the trailer-url to be a youtube-embeddable url.
            string trailerUrl = movie.TrailerUrl;
            string[] splittedUrl = trailerUrl.Split('/');
            string embedUrl = $"https://www.youtube.com/embed/{splittedUrl.Last()}";
            movie.TrailerUrl = embedUrl;

            if (movie == null)
            {
                return NotFound();
            }

            return View(movie);
        }   
    }
}
