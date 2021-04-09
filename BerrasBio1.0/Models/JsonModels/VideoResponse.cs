using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BerrasBio1._0.Models.JsonModels
{
    //Model to handle the api-calls for the video-urls from the movie database
    public class VideoResponse
    {
        [JsonProperty(PropertyName = "results")]
        public List<Result> Results { get; set; }

        public class Result
        {
            [JsonProperty(PropertyName = "site")]
            public string Site { get; set; }

            [JsonProperty(PropertyName = "key")]
            public string Key { get; set; }
            [JsonProperty(PropertyName = "type")]
            public string Type { get; set; }
        }
    }
}
