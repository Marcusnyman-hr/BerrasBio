using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BerrasBio1._0.Models.JsonModels
{
    //Model to handle the api-calls for cast and crew from the movie database-api
    public class CrewResponse
    {
        [JsonProperty(PropertyName = "cast")]
        public List<Actor> Cast { get; set; }
        [JsonProperty(PropertyName = "crew")]
        public List<CrewMember> Crew { get; set; }

        public class CrewMember
        {
            [JsonProperty(PropertyName = "name")]
            public string Name { get; set; }
            [JsonProperty(PropertyName = "job")]
            public string Job { get; set; }
        }
    }
}
