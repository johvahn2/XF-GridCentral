using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GridCentral.Models
{
    public class mNotify
    {
        [JsonIgnore]
        public string IconBg
        {
            get;
            set;
        }

        [JsonIgnore]
        public string Icon
        {
            get;
            set;
        }

        [JsonProperty("Header")]
        public string Header
        {
            get;
            set;
        }

        [JsonProperty("Message")]
        public string Message
        {
            get;
            set;
        }

        [JsonProperty("From")]
        public string From
        {
            get;
            set;
        }


        [JsonProperty("To")]
        public string To
        {
            get;
            set;
        }

        [JsonProperty("Objecter")]
        public string Objecter
        {
            get;
            set;
        }

        [JsonProperty("Type")]
        public string Type
        {
            get;
            set;
        }

        [JsonProperty("Why")]
        public string Why
        {
            get;
            set;
        }

        [JsonProperty("State")]
        public string State
        {
            get;
            set;
        }

        [JsonProperty("Created_at")]
        public string Created_at
        {
            get;
            set;
        }
    }
}
