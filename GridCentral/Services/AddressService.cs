using GridCentral.Helpers;
using GridCentral.Models;
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
    public class AddressService
    {

        private static AddressService instance;

        public static AddressService Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new AddressService();
                }

                return instance;
            }
        }


        public async Task<ObservableCollection<mOrderAddress>> FetchAddresses(string email, int amount,int len)
        {
            try
            {
                var httpClient = new HttpClient();

                var response = await httpClient.GetAsync(Keys.Url_Main + "address/get/" + email + "?amount="+amount+"&len="+len);

                response.EnsureSuccessStatusCode();

                string content = await response.Content.ReadAsStringAsync();

                mServerCallback callback = Newtonsoft.Json.JsonConvert.DeserializeObject<mServerCallback>(content);

                if (callback.Status == "true")
                {
                    ObservableCollection<mOrderAddress> newitems = new ObservableCollection<mOrderAddress>();

                    newitems = Newtonsoft.Json.JsonConvert.DeserializeObject<ObservableCollection<mOrderAddress>>(callback.Data.ToString());



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
                DialogService.ShowErrorToast(Strings.HttpFailed);
                Debug.WriteLine(Keys.TAG + ex);
                return null;
            }
        }


        public async Task<string> AddAddress(mOrderAddress item)
        {
            try
            {
                var Itemcreds = Newtonsoft.Json.JsonConvert.SerializeObject(item);

                HttpContent ItemContent = new StringContent(Itemcreds, Encoding.UTF8, "application/json");

                using (var client = new HttpClient())
                {
                    HttpResponseMessage response = await client.PostAsync(Keys.Url_Main + "address/add", ItemContent);

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

        public async Task<string> EditAddress(mOrderAddress item)
        {
            try
            {
                var Itemcreds = Newtonsoft.Json.JsonConvert.SerializeObject(item);

                HttpContent ItemContent = new StringContent(Itemcreds, Encoding.UTF8, "application/json");

                using (var client = new HttpClient())
                {
                    HttpResponseMessage response = await client.PutAsync(Keys.Url_Main + "address/edit", ItemContent);

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

        public async Task<string> DeleteAddress(string id, string email)
        {
            try
            {

                using (var client = new HttpClient())
                {

                    HttpResponseMessage response = await client.DeleteAsync(Keys.Url_Main + "address/delete/" + id + "/" + email);

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
