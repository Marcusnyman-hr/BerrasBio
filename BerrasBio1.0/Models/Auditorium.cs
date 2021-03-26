using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BerrasBio1._0.Models
{
    public class Auditorium
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Seats { get; set; }
        public ICollection<Showing> Showings { get; set; }
    }
}
