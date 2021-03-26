using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace BerrasBio1._0.Models
{
    public class Showing
    {
        public int Id { get; set; }
        public int AmountOfSeats { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public int MovieId { get; set; }
        [ForeignKey("MovieId")]
        public virtual Movie Movie { get; set; }
        public int AuditoriumId { get; set; }
        [ForeignKey("AuditoriumId")]
        public virtual Auditorium Auditorium { get; set; }
        public ICollection<Seat> Seats { get; set; }
    }
}
