using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GridCentral.Models
{
    public class mQuestion
    {

        [JsonProperty("_id")]
        public string Id { get; set; }

        [JsonProperty("Question")]
        public string Question { get; set; }

        [JsonProperty("Answer")]
        public string Answer { get; set; }

        [JsonProperty("Answer_By")]
        public string Answer_By { get; set; }

        [JsonProperty("Asked_By")]
        public string Asked_By { get; set; }

        [JsonProperty("Displayname")]
        public string Displayname { get; set; }

        [JsonProperty("ProductId")]
        public string ProductId { get; set; }

        [JsonProperty("Asked_At")]
        public string Asked_At { get; set; }

        [JsonProperty("Timeago")]
        public string Timeago { get; set; }

        [JsonProperty("IsItem")]
        public string IsItem { get; set; }
    }
}
