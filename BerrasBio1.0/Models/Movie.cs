using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BerrasBio1._0.Models
{
    public class Movie
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime RelaseDate { get; set; }
        public string Plot { get; set; }
        public int Duration { get; set; }
        public string ImageUrl { get; set; }
        public string BackdropUrl { get; set; }
        public string TrailerUrl { get; set; }
        public Director Director { get; set; }
        public ICollection<Actor> Actors { get; set; }
        public Writer Writer { get; set; }
        public Producer Producer { get; set; }
        public ICollection<Genre> Genres { get; set; }
        public ICollection<Showing> Showings { get; set; }
    }
}
