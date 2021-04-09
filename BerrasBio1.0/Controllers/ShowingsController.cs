using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BerrasBio1._0.Data;
using BerrasBio1._0.Models;
using BerrasBio1._0.Models.ViewModels;

namespace BerrasBio1._0.Controllers
{
    public class ShowingsController : Controller
    {
        private readonly CinemaContext _context;

        public ShowingsController(CinemaContext context)
        {
            _context = context;
        }

        // GET: Showings
        public async Task<IActionResult> Index(string sortOrder)
        {
            //Sortingorders depending on what string gets sent to the action
            ViewBag.StartTime = String.IsNullOrEmpty(sortOrder) ? "StartTime" : "";
            ViewBag.StartTime = sortOrder == "StartTime" ? "start_time_desc" : "StartTime";
            ViewBag.EndTime = sortOrder == "EndTime" ? "end_time_desc" : "EndTime";
            ViewBag.MovieTitle = sortOrder == "MovieTitle" ? "movie_title_desc" : "MovieTitle";
            ViewBag.Auditorium = sortOrder == "Auditorium" ? "auditorium_desc" : "Auditorium";
            ViewBag.Seats = sortOrder == "Seats" ? "seats_desc" : "Seats"; 

            var cinemaContext = _context.Showings.Include(s => s.Auditorium).Include(s => s.Movie).Include(s => s.Seats).ToList();

            //Order the context/showinglist according to sortOrder
            switch (sortOrder)
            {
                case "StartTime":
                    cinemaContext = cinemaContext.OrderBy(s=>s.StartTime).ToList();
                    break;
                case "start_time_desc":
                    cinemaContext = cinemaContext.OrderByDescending(s => s.StartTime).ToList();
                    break;
                case "EndTime":
                    cinemaContext = cinemaContext.OrderBy(s => s.EndTime).ToList();
                    break;
                case "end_time_desc":
                    cinemaContext = cinemaContext.OrderByDescending(s => s.EndTime).ToList();
                    break;
                case "MovieTitle":
                    cinemaContext = cinemaContext.OrderBy(s => s.Movie.Name).ToList();
                    break;
                case "movie_title_desc":
                    cinemaContext = cinemaContext.OrderByDescending(s => s.Movie.Name).ToList();
                    break;
                case "Auditorium":
                    cinemaContext = cinemaContext.OrderBy(s => s.Auditorium.Name).ToList();
                    break;
                case "auditorium_desc":
                    cinemaContext = cinemaContext.OrderByDescending(s => s.Auditorium.Name).ToList();
                    break;
                case "Seats":
                    cinemaContext = cinemaContext.OrderBy(s => s.Seats.Where(s => s.Booked == false).Count()).ToList();
                    break;
                case "seats_desc":
                    cinemaContext = cinemaContext.OrderByDescending(s => s.Seats.Where(s => s.Booked == false).Count()).ToList();
                    break;
            }
            return View(cinemaContext);
        }

        // GET: Showings/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            //get the showing
            var showing = await _context.Showings
                .Include(s => s.Auditorium)
                .Include(s => s.Movie)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (showing == null)
            {
                return NotFound();
            }

            return View(showing);
        }
        //Get showings for a particular movie based on the ID
        public async Task<IActionResult> Movie(int? id, string sortOrder)
        {
            if (id == null)
            {
                return NotFound();
            }
            ViewBag.StartTime = String.IsNullOrEmpty(sortOrder) ? "StartTime" : "";
            ViewBag.StartTime = sortOrder == "StartTime" ? "start_time_desc" : "StartTime";
            ViewBag.EndTime = sortOrder == "EndTime" ? "end_time_desc" : "EndTime";
            ViewBag.MovieTitle = sortOrder == "MovieTitle" ? "movie_title_desc" : "MovieTitle";
            ViewBag.Auditorium = sortOrder == "Auditorium" ? "auditorium_desc" : "Auditorium";
            ViewBag.Seats = sortOrder == "Seats" ? "seats_desc" : "Seats";

            //Get showing where showing.movieID equals the id that is sent to the action
            var showings = _context.Showings
                .Where(s => s.MovieId == id)
                .Include(s => s.Auditorium)
                .Include(s => s.Movie)
                .Include(s => s.Seats).ToList();

            if (showings == null)
            {
                return NotFound();
            }

            //sort the showings
            switch (sortOrder)
            {
                case "StartTime":
                    showings = showings.OrderBy(s => s.StartTime).ToList();
                    break;
                case "start_time_desc":
                    showings = showings.OrderByDescending(s => s.StartTime).ToList();
                    break;
                case "EndTime":
                    showings = showings.OrderBy(s => s.EndTime).ToList();
                    break;
                case "end_time_desc":
                    showings = showings.OrderByDescending(s => s.EndTime).ToList();
                    break;
                case "MovieTitle":
                    showings = showings.OrderBy(s => s.Movie.Name).ToList();
                    break;
                case "movie_title_desc":
                    showings = showings.OrderByDescending(s => s.Movie.Name).ToList();
                    break;
                case "Auditorium":
                    showings = showings.OrderBy(s => s.Auditorium.Name).ToList();
                    break;
                case "auditorium_desc":
                    showings = showings.OrderByDescending(s => s.Auditorium.Name).ToList();
                    break;
                case "Seats":
                    showings = showings.OrderBy(s => s.Seats.Where(s => s.Booked == false).Count()).ToList();
                    break;
                case "seats_desc":
                    showings = showings.OrderByDescending(s => s.Seats.Where(s => s.Booked == false).Count()).ToList();
                    break;
            }
            return View(showings);
        }

        //Get the showings for TODAY - DATETIME.NOW
        public IActionResult Today(string sortOrder)
        {
            ViewBag.StartTime = String.IsNullOrEmpty(sortOrder) ? "StartTime" : "";
            ViewBag.StartTime = sortOrder == "StartTime" ? "start_time_desc" : "StartTime";
            ViewBag.EndTime = sortOrder == "EndTime" ? "end_time_desc" : "EndTime";
            ViewBag.MovieTitle = sortOrder == "MovieTitle" ? "movie_title_desc" : "MovieTitle";
            ViewBag.Auditorium = sortOrder == "Auditorium" ? "auditorium_desc" : "Auditorium";
            ViewBag.Seats = sortOrder == "Seats" ? "seats_desc" : "Seats";
            DateTime dateToday = DateTime.Today;

            var showings = _context.Showings
                .Where(s => s.StartTime.Date == dateToday.Date)
                .Include(s => s.Auditorium)
                .Include(s => s.Movie)
                .Include(s => s.Seats).ToList();

            if (showings == null)
            {
                return NotFound();
            }
            switch (sortOrder)
            {
                case "StartTime":
                    showings = showings.OrderBy(s => s.StartTime).ToList();
                    break;
                case "start_time_desc":
                    showings = showings.OrderByDescending(s => s.StartTime).ToList();
                    break;
                case "EndTime":
                    showings = showings.OrderBy(s => s.EndTime).ToList();
                    break;
                case "end_time_desc":
                    showings = showings.OrderByDescending(s => s.EndTime).ToList();
                    break;
                case "MovieTitle":
                    showings = showings.OrderBy(s => s.Movie.Name).ToList();
                    break;
                case "movie_title_desc":
                    showings = showings.OrderByDescending(s => s.Movie.Name).ToList();
                    break;
                case "Auditorium":
                    showings = showings.OrderBy(s => s.Auditorium.Name).ToList();
                    break;
                case "auditorium_desc":
                    showings = showings.OrderByDescending(s => s.Auditorium.Name).ToList();
                    break;
                case "Seats":
                    showings = showings.OrderBy(s => s.Seats.Where(s => s.Booked == false).Count()).ToList();
                    break;
                case "seats_desc":
                    showings = showings.OrderByDescending(s => s.Seats.Where(s => s.Booked == false).Count()).ToList();
                    break;
            }
            return View(showings);
        }

        //Create/edit/delete showing (Would be protected in a real world project)
        // GET: Showings/Create
        public IActionResult Create()
        {
            ViewData["AuditoriumId"] = new SelectList(_context.Auditoriums, "Id", "Name");
            ViewData["MovieId"] = new SelectList(_context.Movies, "Id", "Name");
            return View();
        }

        // POST: Showings/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,StartTime,MovieId,AuditoriumId,Price")] Showing showing)
        {
            if (ModelState.IsValid)
            {
                //Get the amount of seats that the Auditorium in question have
                int amountOfSeats = _context.Auditoriums.Where(a => a.Id == showing.AuditoriumId).FirstOrDefault().Seats;
                //Get the movie from db
                Movie movie = _context.Movies.Where(m => m.Id == showing.MovieId).FirstOrDefault();
                //Set the duration
                int duration = movie.Duration;
                //Set the endtime (Basically starttime + duration)
                DateTime endTime = showing.StartTime.AddMinutes(duration);
                showing.EndTime = endTime;
                //Create the seats
                List<Seat> seats = new List<Seat>();
                string letters = "ABCDEFGHIJKLMNOPQRSTUVXYZ";
                for (int i = 0; i < amountOfSeats / 10; i++)
                {
                    for (int y = 0; y < 10; y++)
                    {
                        seats.Add(new Seat { Booked = false, Row = letters[i].ToString(), Number = y });
                    }
                }
                //Set the price on the showing according to releasedate (newer then 30 days) and showing starttime (afternoonshow or eveningshow)
                if ((movie.RelaseDate - DateTime.Now).TotalDays < 30)
                {
                    showing.Price += _context.Prices.Where(p => p.Name == "NewMovie").FirstOrDefault().Amount;
                }
                if(showing.StartTime.Hour > 10 && showing.StartTime.Hour < 19)
                {
                    showing.Price += _context.Prices.Where(p => p.Name == "DayShow").FirstOrDefault().Amount;
                }
                if(showing.StartTime.Hour > 18 && showing.StartTime.Hour <= 23)
                {
                    showing.Price += _context.Prices.Where(p => p.Name == "EveningShow").FirstOrDefault().Amount;
                }
                showing.Seats = seats;
                _context.Showings.Add(showing);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["AuditoriumId"] = new SelectList(_context.Auditoriums, "Id", "Id", showing.AuditoriumId);
            ViewData["MovieId"] = new SelectList(_context.Movies, "Id", "Id", showing.MovieId);
            return View(showing);
        }


        // GET: Showings/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var showing = await _context.Showings.FindAsync(id);
            if (showing == null)
            {
                return NotFound();
            }
            ViewData["AuditoriumId"] = new SelectList(_context.Auditoriums, "Id", "Id", showing.AuditoriumId);
            ViewData["MovieId"] = new SelectList(_context.Movies, "Id", "Id", showing.MovieId);
            return View(showing);
        }

        // POST: Showings/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,StartTime,EndTime,MovieId,AuditoriumId")] Showing showing)
        {
            if (id != showing.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(showing);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ShowingExists(showing.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["AuditoriumId"] = new SelectList(_context.Auditoriums, "Id", "Id", showing.AuditoriumId);
            ViewData["MovieId"] = new SelectList(_context.Movies, "Id", "Id", showing.MovieId);
            return View(showing);
        }

        // GET: Showings/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var showing = await _context.Showings
                .Include(s => s.Auditorium)
                .Include(s => s.Movie)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (showing == null)
            {
                return NotFound();
            }

            return View(showing);
        }

        // POST: Showings/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var showing = await _context.Showings.FindAsync(id);
            //var seats = await _context.Seats.Where(x=>x.)
            _context.Showings.Remove(showing);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ShowingExists(int id)
        {
            return _context.Showings.Any(e => e.Id == id);
        }

       
        //Get the prices from the db and send them as viewbag to the view
        public IActionResult PriceList()
        {
            List<Price> prices = _context.Prices.ToList();
            ViewBag.BasePrice = prices.Where(p => p.Name == "BasePrice").FirstOrDefault().Amount;
            ViewBag.NewMovie = prices.Where(p => p.Name == "NewMovie").FirstOrDefault().Amount;
            ViewBag.DayShow = prices.Where(p => p.Name == "DayShow").FirstOrDefault().Amount;
            ViewBag.EveningShow = prices.Where(p => p.Name == "EveningShow").FirstOrDefault().Amount;
            return View();
        }
    }
}


