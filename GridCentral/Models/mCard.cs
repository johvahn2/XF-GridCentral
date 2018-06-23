using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GridCentral.Models
{
    public class mCard
    {
        [JsonProperty("Firstname")]
        public string Name { get; set; }

        [JsonProperty("Lastname")]
        public string Lastname { get; set; }

        [JsonProperty("Country")]
        public string Country { get; set; }

        [JsonProperty("Cardnumber")]
        public string Cardnumber { get; set; }

        [JsonProperty("Cvv")]
        public string Cvv { get; set; }

        [JsonProperty("Expiredate")]
        public DateTime Expiredate { get; set; }

        [JsonProperty("Address1")]
        public string Address1 { get; set; }

        [JsonProperty("Address2")]
        public string Address2 { get; set; }

        [JsonProperty("City")]
        public string City { get; set; }


        [JsonProperty("Region")]
        public string Region { get; set; }

        [JsonProperty("ZipCode")]
        public string ZipCode { get; set; }

    }
}
