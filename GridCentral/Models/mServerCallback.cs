using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GridCentral.Models
{
    public class mServerCallback
    {
        [JsonProperty("status")]
        public string Status { get; set; }

        [JsonProperty("mess")]
        public string Mess { get; set; }

        [JsonProperty("data")]
        public object Data { get; set; }

        [JsonProperty("data2")]
        public object Data2 { get; set; }

    }
}
