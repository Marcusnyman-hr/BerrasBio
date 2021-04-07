using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BerrasBio1._0.Models.ViewModels
{
    public class BuyTicketVM
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public int ShowingId { get; set; }
        public int SeatId { get; set; }
        public List<Seat> Seats { get; set; }
        public int AmountOfTickets { get; set; }
        public string SelectedSeats { get; set; }
    }
}
