using GridCentral.Helpers;
using GridCentral.Models;
using GridCentral.Views.Cart;
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
    public class SavelaterService
    {


        private static SavelaterService instance;

        public static SavelaterService Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new SavelaterService();
                }

                return instance;
            }
        }


        public async Task<string> Additem(mSavelater item)
        {
            try
            {
                var Itemcreds = Newtonsoft.Json.JsonConvert.SerializeObject(item);

                HttpContent ItemContent = new StringContent(Itemcreds, Encoding.UTF8, "application/json");

                using (var client = new HttpClient())
                {
                    System.Net.Http.HttpResponseMessage response = await client.PostAsync(Keys.Url_Main + "savelater/add", ItemContent);

                    using (HttpContent spawn = response.Content)
                    {
                        string content = await spawn.ReadAsStringAsync();

                        mServerCallback callback = Newtonsoft.Json.JsonConvert.DeserializeObject<mServerCallback>(content);

                        if (callback.Status == "true")
                        {

                            return "true";

                        }else if(callback.Mess == "Item Already Saved")
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
        public async Task<string> MvtoCart(mCartS item)
        {
            try
            {
                var Itemcreds = Newtonsoft.Json.JsonConvert.SerializeObject(item);

                HttpContent ItemContent = new StringContent(Itemcreds, Encoding.UTF8, "application/json");

                using (var client = new HttpClient())
                {
                    System.Net.Http.HttpResponseMessage response = await client.PutAsync(Keys.Url_Main + "savelater/mv-cart", ItemContent);

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
        public async Task<ObservableCollection<Product>> Fetchitems(string email)
        {
            try
            {
                var httpClient = new HttpClient();

                var response = await httpClient.GetAsync(Keys.Url_Main + "savelater/get/" + email);

                response.EnsureSuccessStatusCode();

                string content = await response.Content.ReadAsStringAsync();

                mServerCallback callback = Newtonsoft.Json.JsonConvert.DeserializeObject<mServerCallback>(content);

                if (callback.Status == "true")
                {
                    ObservableCollection<Product> newitems = new ObservableCollection<Product>();

                    newitems = Newtonsoft.Json.JsonConvert.DeserializeObject<ObservableCollection<Product>>(callback.Data.ToString());



                    if (newitems.Count < 1) return null;


                    return newitems;
                }
                else
                {

                    DialogService.ShowError(Strings.ServerFailed);
                    return null;
                }
            }
            catch (Exception ex)
            {
                DialogService.ShowError(Strings.HttpFailed);
                Debug.WriteLine(Keys.TAG + ex);
                return null;
            }
        }


        public async Task<string> DeleteItem(string id, string email)
        {
            try
            {

                using (var client = new HttpClient())
                {

                    HttpResponseMessage response = await client.DeleteAsync(Keys.Url_Main + "savelater/delete/" + id + "/" + email);

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

    }
}
