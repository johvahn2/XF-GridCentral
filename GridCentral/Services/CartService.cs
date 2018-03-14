using GridCentral.Helpers;
using GridCentral.Models;
using GridCentral.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace GridCentral.Services
{
    public class CartService
    {

        private static CartService instance;

        public static CartService Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new CartService();
                }

                return instance;
            }
        }


        public async Task<CartType> FetchUserCart(string email)
        {
            try
            {
                var httpClient = new HttpClient();

                var response = await httpClient.GetAsync(Keys.Url_Main + "cart/get/" + email);

                response.EnsureSuccessStatusCode();

                string content = await response.Content.ReadAsStringAsync();

                mServerCallback callback = Newtonsoft.Json.JsonConvert.DeserializeObject<mServerCallback>(content);

                if (callback.Status == "true")
                {
                    ObservableCollection<Product> newitems = new ObservableCollection<Product>();
                    ObservableCollection<string> quantity = new ObservableCollection<string>();

                    newitems = Newtonsoft.Json.JsonConvert.DeserializeObject<ObservableCollection<Product>>(callback.Data.ToString());
                    quantity = Newtonsoft.Json.JsonConvert.DeserializeObject<ObservableCollection<string>>(callback.Data2.ToString());



                    if (newitems.Count < 1) return null;


                    return new CartType() { Prop1 = newitems,Prop2=quantity};
                }
                else
                {

                    DialogService.ShowError(Strings.ServerFailed);
                    return null;
                }
            }
            catch (Exception ex)
            {
                DialogService.ShowErrorToast(Strings.HttpFailed);
                Debug.WriteLine(Keys.TAG + ex);
                return null;
            }
        }

        public async Task<string> Additem(mCartS item)
        {
            try
            {
                var Itemcreds = Newtonsoft.Json.JsonConvert.SerializeObject(item);

                HttpContent ItemContent = new StringContent(Itemcreds, Encoding.UTF8, "application/json");

                using (var client = new HttpClient())
                {
                    HttpResponseMessage response = await client.PostAsync(Keys.Url_Main + "cart/add", ItemContent);

                    using (HttpContent spawn = response.Content)
                    {
                        string content = await spawn.ReadAsStringAsync();

                        mServerCallback callback = Newtonsoft.Json.JsonConvert.DeserializeObject<mServerCallback>(content);

                        if (callback.Status == "true")
                        {

                            return "true";
                        }

                        return callback.Mess;
                    }
                }

            }
            catch (Exception ex)
            {
                Debug.WriteLine(Keys.TAG + ex);
                return Strings.HttpFailed;
            }
        }

        public async Task<string> DeleteItem(string id,string email)
        {
            try
            {

                using (var client = new HttpClient())
                {

                    HttpResponseMessage response = await client.DeleteAsync(Keys.Url_Main + "cart/delete/" + id + "/" + email);

                    using (HttpContent spawn = response.Content)
                    {
                        string content = await spawn.ReadAsStringAsync();

                        mServerCallback callback = Newtonsoft.Json.JsonConvert.DeserializeObject<mServerCallback>(content);

                        if (callback.Status == "true")
                        {
                            return "true";
                        }

                        return callback.Mess;
                    }
                }


            }
            catch (Exception ex)
            {
                Debug.WriteLine(Keys.TAG + ex);
                return Strings.HttpFailed;
            }
        }


        public async Task<string> UpdateQuantity(mCartS item)
        {
            try
            {
                var Itemcreds = Newtonsoft.Json.JsonConvert.SerializeObject(item);

                HttpContent ItemContent = new StringContent(Itemcreds, Encoding.UTF8, "application/json");

                using (var client = new HttpClient())
                {
                    HttpResponseMessage response = await client.PutAsync(Keys.Url_Main + "cart/chg-quantity", ItemContent);

                    using (HttpContent spawn = response.Content)
                    {
                        string content = await spawn.ReadAsStringAsync();

                        mServerCallback callback = Newtonsoft.Json.JsonConvert.DeserializeObject<mServerCallback>(content);

                        if (callback.Status == "true")
                        {

                            return "true";
                        }

                        return callback.Mess;
                    }
                }

            }
            catch (Exception ex)
            {
                Debug.WriteLine(Keys.TAG + ex);
                return Strings.HttpFailed;
            }
        }

        public async Task<string> ClearCart(string email)
        {
            try
            {

                using (var client = new HttpClient())
                {

                    HttpResponseMessage response = await client.DeleteAsync(Keys.Url_Main + "cart/clear/" + email);

                    using (HttpContent spawn = response.Content)
                    {
                        string content = await spawn.ReadAsStringAsync();

                        mServerCallback callback = Newtonsoft.Json.JsonConvert.DeserializeObject<mServerCallback>(content);

                        if (callback.Status == "true")
                        {
                            return "true";
                        }

                        return callback.Mess;
                    }
                }


            }
            catch (Exception ex)
            {
                Debug.WriteLine(Keys.TAG + ex);
                return Strings.HttpFailed;
            }
        }

        public async Task<string> WeightPercentage(string email)
        {
            try
            {
                var httpClient = new HttpClient();

                var response = await httpClient.GetAsync(Keys.Url_Main + "cart/weight/" + email);

                response.EnsureSuccessStatusCode();

                string content = await response.Content.ReadAsStringAsync();

                mServerCallback callback = Newtonsoft.Json.JsonConvert.DeserializeObject<mServerCallback>(content);

                if (callback.Status == "true")
                {
                    var percentage = callback.Data.ToString();

                    if(percentage == null)
                    {
                        percentage = "6";
                    }
                    return percentage;

                }
                else
                {

                    DialogService.ShowError(Strings.ServerFailed);
                    return null;
                }
            }
            catch (Exception ex)
            {
                DialogService.ShowErrorToast(Strings.HttpFailed);
                Debug.WriteLine(Keys.TAG + ex);
                return null;
            }
        }

        public async Task<string> ShippingCost(string email)
        {
            try
            {
                var httpClient = new HttpClient();

                var response = await httpClient.GetAsync(Keys.Url_Main + "cart/check/shipping/" + email);

                response.EnsureSuccessStatusCode();

                string content = await response.Content.ReadAsStringAsync();

                mServerCallback callback = Newtonsoft.Json.JsonConvert.DeserializeObject<mServerCallback>(content);

                if (callback.Status == "true")
                {
                    var cost = callback.Data.ToString();

                    if (cost == "0" || cost == null)
                    {
                        cost = "0";
                    }
                    return cost;

                }
                else
                {

                    DialogService.ShowError(Strings.ServerFailed);
                    return null;
                }
            }
            catch (Exception ex)
            {
                DialogService.ShowErrorToast(Strings.HttpFailed);
                Debug.WriteLine(Keys.TAG + ex);
                return null;
            }
        }
    }
}
