﻿using System;
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
        public async Task<IActionResult> Index()
        {
            
            var cinemaContext = _context.Showings.Include(s => s.Auditorium).Include(s => s.Movie).Include(s=> s.Seats);
            return View(await cinemaContext.ToListAsync());
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
        public async Task<IActionResult> Movie(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var showings = _context.Showings
                .Where(s => s.MovieId == id)
                .Include(s => s.Auditorium)
                .Include(s => s.Movie)
                .Include(s => s.Seats);

            if (showings == null)
            {
                return NotFound();
            }

            return View(await showings.ToListAsync());
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


