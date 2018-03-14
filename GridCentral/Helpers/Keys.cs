using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GridCentral.Helpers
{
    public class Keys
    {
        public static string Url_Main = "https://api.gridcentral.sharefold.com/";//https://gridshop1.herokuapp.com/


        public static readonly string HockeyId_Android = "765330ab36954d27b876eebbe5826484";

        public static readonly string HockeyId_IOS = "";

        public static string TAG = "GRID--";

        public static List<string> ItemCategories
        {
            get
            {
                return new List<string>()
                {
                    "Appliances","Art","Baby","Books","Cars","Clothing","Electronics","Furniture","Hair","Home Supplies",
                    "Personal Care", "Makeup Beauty", "Jewelry","Other","Toys Games"
                };
            }
        }

        //public static List<string> ItemCategories
        //{
        //    get
        //    {
        //        return new List<string>()
        //        {
        //           "Art","Books","Cars","Clothing","Electronics","Furniture",
        //           "Jewelry","Other","Toys_Games" , "Personal_Care" , "Makeup_Beauty"
        //        };
        //    }
        //}

        public static Dictionary<string, string> CatItems = new Dictionary<string, string>()
        {
            {"All","All"},{ "Appliances","Appliances"},{"Art","Art"},{"Baby","Baby"},{"Books","Books"},{"Cars","Cars"},{"Clothing","Clothing"},{"Electronics","Electronics"},{"Furniture","Furniture"},
            {"Hair","Hair"},{ "Home Supplies","Home_Supplies"},{"Personal Care","Personal_Care"},{ "Makeup Beauty","Makeup_Beauty"},{ "Jewelry","Jewelry"},{ "Other","Other"},{ "Toys Games","Toys_Games"}
        };

        public static Dictionary<string, string> CatItemsRev = new Dictionary<string, string>()
        {
            {"All","All"},{ "Appliances","Appliances"},{"Art","Art"},{"Baby","Baby"},{"Books","Books"},{"Cars","Cars"},{"Clothing","Clothing"},{"Electronics","Electronics"},{"Furniture","Furniture"},
            {"Hair","Hair"},{ "Home_Supplies","Home Supplies"},{"Personal_Care","Personal Care"},{ "Makeup_Beauty","Makeup Beauty"},{ "Jewelry","Jewelry"},{ "Other","Other"},{ "Toys_Games","Toys Games"}
        };

        public static List<string> NotifyWhys
        {
            get
            {
                return new List<string>()
                {
                    "item-requested","new-question","answer-question",
                    "Approved","Canceled","Pending","Transit","Delivered","order-update","rate"
                };
            }
        }

        public static List<string> OrderStatus
        {
            get
            {
                return new List<string>()
                {
                    "Pending","Approved","Preparing","Delay","Transit","Delivered","Canceled"
                };
            }
        }

        public static List<string> NotifyTypes
        {
            get
            {
                return new List<string>()
                {
                    "item","question","order"
                };
            }
        }

    }
}
