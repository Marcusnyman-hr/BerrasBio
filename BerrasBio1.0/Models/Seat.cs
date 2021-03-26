using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace BerrasBio1._0.Models
{
    public class Seat
    {
        public int Id { get; set; }
        public string Row { get; set; }
        public int Number { get; set; }
        public bool Booked { get; set; }
        public int ShowingId { get; set; }
        [ForeignKey("ShowingId")]
        public virtual Showing Showing { get; set; }

    }
}
