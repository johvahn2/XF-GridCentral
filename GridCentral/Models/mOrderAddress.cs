using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace GridCentral.Models
{
    public class mOrderAddress
    {
        [JsonIgnore]
        public ICommand UseCommand { get;set; }
        [JsonIgnore]
        public ICommand ModifyCommand { get;set; }
        [JsonIgnore]
        public ICommand RemoveCommand { get; set; }

        [JsonProperty("Address1")]
        public string Address1 { get; set; }

        [JsonProperty("Address2")]
        public string Address2 { get; set; }

        [JsonProperty("PhoneNumber")]
        public string PhoneNumber { get; set; }

        [JsonProperty("Owner")]
        public string Owner { get; set; }

        [JsonProperty("_id")]
        public string _id { get; set; }
    }
}
