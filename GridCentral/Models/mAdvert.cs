using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GridCentral.Models
{
    public class mAdvert
    {
        [JsonProperty("Image")]
        public string Image { get; set; }

        [JsonProperty("Name")]
        public string Name { get; set; }

        [JsonProperty("Owner")]
        public string Owner { get; set; }

        [JsonProperty("Type")]
        public string Type { get; set; }

        [JsonProperty("Show")]
        public string Show { get; set; }

        [JsonProperty("AddedOn")]
        public string AddedOn { get; set; }

        [JsonProperty("Parent")]
        public string Parent { get; set; }

        [JsonProperty("Description")]
        public string Description { get; set; }
    }
}
