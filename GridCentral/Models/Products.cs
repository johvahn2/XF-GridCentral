using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GridCentral.Models
{
    public class Product
    {
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

        [JsonProperty("Addon")]
        public string Addon
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

        [JsonProperty("Status")]
        public string Status
        {
            get;
            set;
        }

        [JsonProperty("Weight")]
        public string Weight
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

        [JsonProperty("bPrice")]
        public string bPrice
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

        [JsonProperty("Quantity")]
        public string Quantity
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

        [JsonProperty("PRate")]
        public string PRating
        {
            get;
            set;
        } = null;


        [JsonProperty("Rating")]
        public ObservableCollection<mRate> Ratings
        {
            get;
            set;
        }


        [JsonProperty("IsDeal")]
        public string IsDeal
        {
            get;
            set;
        }

        [JsonProperty("DealPrice")]
        public string DealPrice
        {
            get;
            set;
        }

        [JsonProperty("DealPercentage")]
        public string DealPercentage
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
        [JsonIgnore]
        public double RatingValue { get; set; }
        [JsonIgnore]
        public double RatingMax { get; set; }

        public Product()
        {
        }
    }

    public class mRating
    {
        public string rate
        {
            get;
            set;
        }
        public string by
        {
            get;
            set;
        }
    }
}
