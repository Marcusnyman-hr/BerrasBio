using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BerrasBio1._0.Data;
using BerrasBio1._0.Models;
using BerrasBio1._0.Models.ViewModels;

namespace BerrasBio1._0.Controllers
{
    public class TicketsController : Controller
    {
        private readonly CinemaContext _context;

        public TicketsController(CinemaContext context)
        {
            _context = context;
        }
        //Gets the customers tickets, both old and new and returns with includes for relations to create the tickets.
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
        //Buy ticket
        public IActionResult BuyTicket(int id)
        {
            if (id == null)
            {
                return NotFound();
            }
            //Get the showing
            var showing = _context.Showings
                .Include(s => s.Seats)
                .Include(s => s.Auditorium)
                .Include(s => s.Movie)
                .FirstOrDefault(s => s.Id == id);

            if (showing == null)
            {
                return NotFound();
            }

            var seats = showing.Seats.OrderBy(s => s.Row).ThenBy(s => s.Number).ToList();
            //Create a new viewmodel for the view
            var model = new BuyTicketVM();
            model.Seats = seats;

            //Set some viewbag props like moviename, startingtime and backdrop url.
            ViewBag.ShowingId = id;
            ViewBag.MovieTitle = showing.Movie.Name;
            ViewBag.MovieDate = showing.StartTime.ToString("yyyy-MM-dd HH:mm");
            ViewBag.MovieBackdrop = showing.Movie.BackdropUrl;
            ViewBag.ShowingPrice = showing.Price;
            return View(model);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> BuyTicket([Bind("FirstName,LastName,Email,ShowingId, AmountOfTickets, SelectedSeats")] BuyTicketVM buyTicketVM)
        {
            if (ModelState.IsValid)
            {
                //create a list for tickets
                List<Ticket> tickets = new List<Ticket>();
                //Get the seats from that is not booked from the showing
                List<Seat> freeSeats = _context.Seats.Where(s => s.ShowingId == buyTicketVM.ShowingId && s.Booked == false).ToList();
                //split the string of the selected-seats that gets passed to the action
                var selectedSeats = buyTicketVM.SelectedSeats.Split(',').Select(Int32.Parse).ToList();
                //For "amount of tickets" get the seat, create the ticket, set the seat to booked
                for (int i = 0; i < buyTicketVM.AmountOfTickets; i++)
                {
                    var freeSeat = _context.Seats.Find(selectedSeats[i]);
                    Ticket ticket = new Ticket { ShowingId = buyTicketVM.ShowingId, SeatId = selectedSeats[i], Price = _context.Showings.Where(s => s.Id == buyTicketVM.ShowingId).FirstOrDefault().Price };
                    tickets.Add(ticket);
                    freeSeat.Booked = true;
                }
                //Creates the customer, but first checks if the email already exists
                Customer customer = _context.Customers.Where(c => c.Email == buyTicketVM.Email).FirstOrDefault();
                //If customer dont exists, create a new customer
                if (customer == null)
                {
                    customer = new Customer() { FirstName = buyTicketVM.FirstName, LastName = buyTicketVM.LastName, Email = buyTicketVM.Email, Tickets = tickets };
                    _context.Customers.Add(customer);
                }
                else
                //If customer exists and already bought tickets before, merge all new and old tickets
                {
                    if (customer.Tickets != null)
                    {
                        List<Ticket> oldTickets = customer.Tickets.ToList();
                        oldTickets.AddRange(tickets);
                        customer.Tickets = oldTickets;
                    }
                    else
                    //else set the new tickets for the customer
                    {
                        customer.Tickets = tickets;
                    }
                }
                _context.SaveChanges();
                return RedirectToAction("TicketConfirmation", "Tickets", new { id = customer.Id });

            }

            //if modelstate is invalid ,return the view with viewbags for already selected seats etc.
            var showing = _context.Showings
                .Include(s => s.Seats)
                .Include(s => s.Auditorium)
                .Include(s => s.Movie)
                .FirstOrDefault(s => s.Id == buyTicketVM.ShowingId);

            var seats = showing.Seats.OrderBy(s => s.Row).ThenBy(s => s.Number).ToList();
            var model = new BuyTicketVM();
            model.Seats = seats;
            model.SelectedSeats = buyTicketVM.SelectedSeats;
            model.AmountOfTickets = buyTicketVM.AmountOfTickets;

            ViewBag.ShowingId = buyTicketVM.ShowingId;
            ViewBag.ShowingPrice = showing.Price;
            return View(model);
        }

    }
}
