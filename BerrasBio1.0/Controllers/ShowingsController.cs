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
            ViewBag.StartTime = String.IsNullOrEmpty(sortOrder) ? "StartTime" : "";
            ViewBag.StartTime = sortOrder == "StartTime" ? "start_time_desc" : "StartTime";
            ViewBag.EndTime = sortOrder == "EndTime" ? "end_time_desc" : "EndTime";
            ViewBag.MovieTitle = sortOrder == "MovieTitle" ? "movie_title_desc" : "MovieTitle";
            ViewBag.Auditorium = sortOrder == "Auditorium" ? "auditorium_desc" : "Auditorium";
            ViewBag.Seats = sortOrder == "Seats" ? "seats_desc" : "Seats"; 

            var cinemaContext = _context.Showings.Include(s => s.Auditorium).Include(s => s.Movie).Include(s => s.Seats).ToList();

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

            var showings = _context.Showings
                .Where(s => s.MovieId == id)
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
        public async Task<IActionResult> Create([Bind("Id,StartTime,MovieId,AuditoriumId")] Showing showing)
        {
            if (ModelState.IsValid)
            {
                int amountOfSeats = _context.Auditoriums.Where(a => a.Id == showing.AuditoriumId).FirstOrDefault().Seats;
                int duration = _context.Movies.Where(m => m.Id == showing.MovieId).FirstOrDefault().Duration;
                DateTime endTime = showing.StartTime.AddMinutes(duration);
                showing.EndTime = endTime;
                List<Seat> seats = new List<Seat>();
                string letters = "ABCDEFGHIJKLMNOPQRSTUVXYZ";
                for (int i = 0; i < amountOfSeats / 10; i++)
                {
                    for (int y = 0; y < 10; y++)
                    {
                        seats.Add(new Seat { Booked = false, Row = letters[i].ToString(), Number = y });
                    }
                }
                showing.Seats = seats;
                _context.Add(showing);
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
        public IActionResult BuyTicket(int id)
        {
            //För VG
            //ViewData["SeatId"] = new SelectList(_context.Seats.Where(s=>s.Booked == false).OrderBy(s => s.Row), "Id", "SeatName");
            ViewBag.ShowingId = id;
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> BuyTicket([Bind("FirstName,LastName,Email,ShowingId, AmountOfTickets")] BuyTicketVM buyTicketVM)
        {
            if (ModelState.IsValid)
            {
                List<Ticket> tickets = new List<Ticket>();
                List<Seat> freeSeats = _context.Seats.Where(s => s.ShowingId == buyTicketVM.ShowingId && s.Booked == false).ToList();
                for (int i = 0; i < buyTicketVM.AmountOfTickets; i++)
                {
                    Seat freeSeat = freeSeats[i];
                    Ticket ticket = new Ticket { ShowingId = buyTicketVM.ShowingId, SeatId = freeSeat.Id };
                    tickets.Add(ticket);
                    freeSeat.Booked = true;
                }
                Customer customer = new Customer { FirstName = buyTicketVM.FirstName, LastName = buyTicketVM.LastName, Email = buyTicketVM.Email, Tickets = tickets };
                _context.Add(customer);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View();
        }
    }
}


