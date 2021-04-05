using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace BerrasBio1._0.Models
{
    public class Ticket
    {
        public Ticket()
        {
            Guid id = Guid.NewGuid();
            this.Id = id.ToString();
        }
        public string Id { get; set; }
        public int? ShowingId { get; set; }
        [ForeignKey("ShowingId")]
        public virtual Showing Showing { get; set; }
        public int SeatId { get; set; }
        [ForeignKey("SeatId")]
        public virtual Seat Seat { get; set; }
        public decimal Price { get; set; }

    }
}
