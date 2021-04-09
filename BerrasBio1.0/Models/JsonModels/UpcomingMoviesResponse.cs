using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BerrasBio1._0.Models.JsonModels
{
    //Model to handle the api-calls to get the ids of upcoming movies from the movie database-api
    public class UpcomingMoviesResponse
    {
        [JsonProperty(PropertyName = "results")]
        public List<Result> Results { get; set; }

        public class Result
        {
            [JsonProperty(PropertyName = "id")]
            public int Id { get; set; }
        }
    }
}
