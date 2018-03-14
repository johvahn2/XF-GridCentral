using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GridCentral.Models
{
    public class mOrder
    {

        [JsonProperty("_id")]
        public string OrderId { get; set; }

        [JsonProperty("OrderNumber")]
        public string OrderNumber { get; set; }

        [JsonProperty("OwnerEmail")]
        public string OwnerEmail { get; set; }

        [JsonProperty("Status")]
        public string Status { get; set; }

        [JsonProperty("DeliveryTime")]
        public string DeliveryTime { get; set; }

        [JsonProperty("Address1")]
        public string Address1 { get; set; }

        [JsonProperty("Address2")]
        public string Address2 { get; set; }


        [JsonProperty("Name")]
        public string Name { get; set; }

        [JsonProperty("PhoneNumber")]
        public string PhoneNumber { get; set; }

        [JsonProperty("ProfileNumber")]
        public string ProfileNumber { get; set; }

        [JsonProperty("Order_At")]
        public string Order_At { get; set; }

        [JsonProperty("GrandPrice")]
        public string GrandPrice { get; set; }

        [JsonProperty("ShippingPrice")]
        public string ShippingPrice { get; set; }

        [JsonProperty("TaxPrice")]
        public string TaxPrice { get; set; }

        [JsonProperty("ItemTotal")]
        public string ItemTotal { get; set; }

        [JsonProperty("Items")]
        public ObservableCollection<mOrderItems> Items { get; set; }

        [JsonProperty("OrderMessage")]
        public string OrderMessage { get; set; } = null;
    }


     public class mOrderItems
    {
        [JsonProperty("ItemName")]
        public string ItemName { get; set; }

        [JsonProperty("ItemNameSub")]
        public string ItemNameSub { get; set; }

        [JsonProperty("ItemId")]
        public string ItemId { get; set; }

        [JsonProperty("Price")]
        public string Price { get; set; }

        [JsonProperty("Seller")]
        public string Seller { get; set; }

        [JsonProperty("Quantity")]
        public string Quantity { get; set; }

        [JsonProperty("Thumbnail")]
        public string Thumbnail { get; set; }


    }
}
