using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GridCentral.Models
{
    class mProductFee
    {
        [JsonProperty("Tax")]
        public string Tax
        {
            get;
            set;
        }

        [JsonProperty("Shipping")]
        public string Shipping
        {
            get;
            set;
        }
    }
}
