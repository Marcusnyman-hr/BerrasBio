using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BerrasBio1._0.Data;
using BerrasBio1._0.Models;

namespace BerrasBio1._0.Controllers
{
    public class TicketsController : Controller
    {
        private readonly CinemaContext _context;

        public TicketsController(CinemaContext context)
        {
            _context = context;
        }

        // GET: Tickets
        public async Task<IActionResult> Index()
        {
            var cinemaContext = _context.Tickets.Include(t => t.Seat).Include(t => t.Showing);
            return View(await cinemaContext.ToListAsync());
        }
        public IActionResult TicketConfirmation(int id)
        {

            var customer = _context.Customers
                .Where(c=>c.Id == id)
                .Include(customer => customer.Tickets)
                .ThenInclude(ticket => ticket.Showing)
                .ThenInclude(showing=>showing.Movie)
                .Include(customer => customer.Tickets)
                .ThenInclude(ticket=>ticket.Seat)
                .Include(customer => customer.Tickets)
                .ThenInclude(ticket => ticket.Showing)
                .ThenInclude(showing => showing.Auditorium).FirstOrDefault();
            ViewBag.CustomerName = $"{customer.FirstName} {customer.LastName}";
            if (customer == null)
            {
                return NotFound();
            }

            return View(customer.Tickets);
        }

        // GET: Tickets/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var ticket = await _context.Tickets
                .Include(t => t.Seat)
                .Include(t => t.Showing)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (ticket == null)
            {
                return NotFound();
            }

            return View(ticket);
        }

        // GET: Tickets/Create
        public IActionResult Create()
        {
            ViewData["SeatId"] = new SelectList(_context.Seats.OrderBy(s=>s.Row), "Id", "SeatName");
            ViewData["ShowingId"] = new SelectList(_context.Showings, "Id", "Id");
            return View();
        }

        // POST: Tickets/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ShowingId,SeatId")] Ticket ticket)
        {
            if (ModelState.IsValid)
            {
                Seat seat = _context.Seats.Where(s => s.Id == ticket.SeatId).FirstOrDefault();
                seat.Booked = true;
                _context.Add(ticket);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["SeatId"] = new SelectList(_context.Seats, "Id", "Id", ticket.SeatId);
            ViewData["ShowingId"] = new SelectList(_context.Showings, "Id", "Id", ticket.ShowingId);
            return View(ticket);
        }

        // GET: Tickets/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var ticket = await _context.Tickets.FindAsync(id);
            if (ticket == null)
            {
                return NotFound();
            }
            ViewData["SeatId"] = new SelectList(_context.Seats, "Id", "Id", ticket.SeatId);
            ViewData["ShowingId"] = new SelectList(_context.Showings, "Id", "Id", ticket.ShowingId);
            return View(ticket);
        }

        // POST: Tickets/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("Id,ShowingId,SeatId")] Ticket ticket)
        {
            if (id != ticket.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(ticket);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TicketExists(ticket.Id))
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
            ViewData["SeatId"] = new SelectList(_context.Seats, "Id", "Id", ticket.SeatId);
            ViewData["ShowingId"] = new SelectList(_context.Showings, "Id", "Id", ticket.ShowingId);
            return View(ticket);
        }

        // GET: Tickets/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var ticket = await _context.Tickets
                .Include(t => t.Seat)
                .Include(t => t.Showing)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (ticket == null)
            {
                return NotFound();
            }

            return View(ticket);
        }

        // POST: Tickets/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var ticket = await _context.Tickets.FindAsync(id);
            _context.Tickets.Remove(ticket);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TicketExists(string id)
        {
            return _context.Tickets.Any(e => e.Id == id);
        }
    }
}
