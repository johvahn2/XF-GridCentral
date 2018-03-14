using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GridCentral.Models
{
    public class mRate
    {
        [JsonProperty("Rate")]
        public string Rate { get; set; }

        [JsonProperty("By")]
        public string By { get; set; }

        [JsonProperty("Itemid")]
        public string Itemid { get; set; }
    }
}
