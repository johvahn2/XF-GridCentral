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
    public class OrderService
    {
        private static OrderService instance;

        public static OrderService Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new OrderService();
                }

                return instance;
            }
        }

        public async Task<string> SendOrder(mOrder order)
        {
            try
            {
                var Itemcreds = Newtonsoft.Json.JsonConvert.SerializeObject(order);

                HttpContent ItemContent = new StringContent(Itemcreds, Encoding.UTF8, "application/json");

                using (var client = new HttpClient())
                {
                    HttpResponseMessage response = await client.PostAsync(Keys.Url_Main + "order/send", ItemContent);

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

        public async Task<string> UpdateDetail(mOrder order)
        {
            try
            {
                var Itemcreds = Newtonsoft.Json.JsonConvert.SerializeObject(order);

                HttpContent ItemContent = new StringContent(Itemcreds, Encoding.UTF8, "application/json");

                using (var client = new HttpClient())
                {
                    HttpResponseMessage response = await client.PutAsync(Keys.Url_Main + "order/update-detail", ItemContent);

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

        public async Task<ObservableCollection<mOrder>> FetchOrders(string email)
        {
            try
            {
                var httpClient = new HttpClient();

                var response = await httpClient.GetAsync(Keys.Url_Main + "order/get/" + email);

                response.EnsureSuccessStatusCode();

                string content = await response.Content.ReadAsStringAsync();

                mServerCallback callback = Newtonsoft.Json.JsonConvert.DeserializeObject<mServerCallback>(content);

                if (callback.Status == "true")
                {
                    ObservableCollection<mOrder> newitems = new ObservableCollection<mOrder>();

                    newitems = Newtonsoft.Json.JsonConvert.DeserializeObject<ObservableCollection<mOrder>>(callback.Data.ToString());


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

        public async Task<string> CancelOrder(mOrder order)
        {
            try
            {
                order.Status = "Canceled";

                var Itemcreds = Newtonsoft.Json.JsonConvert.SerializeObject(order);

                HttpContent ItemContent = new StringContent(Itemcreds, Encoding.UTF8, "application/json");

                using (var client = new HttpClient())
                {
                    HttpResponseMessage response = await client.PutAsync(Keys.Url_Main + "order/cancel", ItemContent);

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

        public async Task<mOrder> FetchOrder(string orderId)
        {
            try
            {

                var httpClient = new HttpClient();

                var response = await httpClient.GetAsync(Keys.Url_Main + "order/get-one/" + orderId);

                response.EnsureSuccessStatusCode();

                string content = await response.Content.ReadAsStringAsync();

                mServerCallback callback = Newtonsoft.Json.JsonConvert.DeserializeObject<mServerCallback>(content);

                if (callback.Status == "true")
                {
                    mOrder newitem = new mOrder();

                    newitem = Newtonsoft.Json.JsonConvert.DeserializeObject<mOrder>(callback.Data.ToString());


                    if (newitem == null) return null;

                    return newitem;
                }
                else
                {

                    DialogService.ShowError(Strings.ServerFailed);
                    return null;
                }


            }
            catch (Exception ex)
            {
                Debug.WriteLine(Keys.TAG + ex);
                DialogService.ShowErrorToast(Strings.HttpFailed);
                return null;
            }
        }
    }
}
