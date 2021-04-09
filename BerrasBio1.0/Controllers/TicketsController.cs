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
    }
}
