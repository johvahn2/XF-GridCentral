using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GridCentral.Models
{
    public class mAccount
    {
        [JsonProperty("Email")]
        public string Email { get; set; }

        [JsonProperty("Displayname")]
        public string Displayname { get; set; }

        [JsonProperty("Password")]
        public string Password { get; set; }


        [JsonProperty("NewPassword")]
        public string NewPassword { get; set; }


        [JsonProperty("nToken")]
        public string nToken { get; set; }

        [JsonProperty("_id")]
        public string UserId { get; set; }

        [JsonProperty("FirstName")]
        public string FirstName { get; set; }

        [JsonProperty("LastName")]
        public string LastName { get; set; }

        [JsonProperty("PhoneNumber")]
        public string PhoneNumber { get; set; }

        [JsonProperty("Gender")]
        public string Gender { get; set; }

        [JsonProperty("bDay")]
        public DateTime bDay { get; set; }

        [JsonProperty("Address")]
        public string Address { get; set; }

        [JsonProperty("AddressDescription")]
        public string AddressDescription { get; set; }

        [JsonProperty("Rating")]
        public string Rating { get; set; }

        [JsonProperty("Rank")]
        public string Rank { get; set; }

        [JsonProperty("Notify")]
        public bool Notify { get; set; }

        [JsonProperty("ProfileImage")]
        public string ProfileImage { get; set; }

        [JsonProperty("bProfileImage")]
        public byte[] bProfileImage { get; set; }

        [JsonProperty("Interests")]
        public List<string> Interests { get; set; }

        [JsonProperty("IsAvailable")]
        public bool IsAvailable { get; set; }


        [JsonProperty("CreatedAt")]
        public string createdAt { get; set; }
    }
}
