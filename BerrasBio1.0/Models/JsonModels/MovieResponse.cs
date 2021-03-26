using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BerrasBio1._0.Models.JsonModels
{
    public class MovieResponse
    {
        [JsonProperty(PropertyName = "title")]
        public string Title { get; set; }
        [JsonProperty(PropertyName = "release_date")]
        public string ReleaseDate { get; set; }
        [JsonProperty(PropertyName = "backdrop_path")]
        public string BackdropPath { get; set; }
        [JsonProperty(PropertyName = "poster_path")]
        public string PosterPath { get; set; }
        [JsonProperty(PropertyName = "overview")]
        public string Overview { get; set; }
        [JsonProperty(PropertyName = "runtime")]
        public int Runtime { get; set; }
        [JsonProperty(PropertyName = "genres")]
        public List<Genre> Genres { get; set; }
    }
}
