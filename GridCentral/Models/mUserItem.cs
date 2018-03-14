using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace GridCentral.Models
{
    public class mUserItem
    {
        [JsonIgnore]
        public ICommand DeleteItemCommand { get; set; }

        [JsonProperty("_id")]
        public string Id
        {
            get;
            set;
        }
        [JsonProperty("Name")]
        public string Name
        {
            get;
            set;
        }

        [JsonIgnore]
        public string SummaryName
        {
            get;
            set;
        }

        [JsonProperty("Description")]
        public string Description
        {
            get;
            set;
        }

        [JsonIgnore]
        public string SummaryDesc
        {
            get;
            set;
        }

        [JsonProperty("Price")]
        public string Price
        {
            get;
            set;
        }

        [JsonProperty("Images")]
        public List<string> Images
        {
            get;
            set;
        }

        [JsonProperty("bImages")]
        public List<byte[]> bImages
        {
            get;
            set;
        }

        [JsonProperty("Quantity")]
        public string Quantity
        {
            get;
            set;
        }

        [JsonProperty("Status")]
        public string Status
        {
            get;
            set;
        }

        [JsonProperty("Visable")]
        public string Visable
        {
            get;
            set;
        }

        [JsonIgnore]
        public string StatusColor
        {
            get;
            set;
        }

        [JsonProperty("PurchaseType")]
        public string PurchaseType
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

        [JsonIgnore]
        public string StateColor
        {
            get;
            set;
        }

        [JsonProperty("Category")]
        public string Category
        {
            get;
            set;
        }

        [JsonProperty("CreatedBy")]
        public string Manufacturer
        {
            get;
            set;
        }

        [JsonProperty("Displayname")]
        public string Displayname
        {
            get;
            set;
        }

        [JsonProperty("CreatedDate")]
        public string CreatedDate
        {
            get;
            set;
        }


        [JsonProperty("Timeago")]
        public string Timeago { get; set; }

        [JsonIgnore]
        public string Thumbnail
        {
            get;
            set;
        }

        [JsonIgnore]
        public string ThumbnailHeight
        {
            get;
            set;
        }

        public mUserItem()
        {               
        }
    }
}
