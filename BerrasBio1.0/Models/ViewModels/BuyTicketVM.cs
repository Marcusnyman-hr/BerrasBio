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
        [Required]
        [StringLength(30)]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }
        [Required]
        [StringLength(50)]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }
        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }
        [Required]
        public int ShowingId { get; set; }
        public int SeatId { get; set; }
        public List<Seat> Seats { get; set; }
        [Required]
        [Range(1, 12)]
        public int AmountOfTickets { get; set; }
        [Required]
        public string SelectedSeats { get; set; }
    }
}
