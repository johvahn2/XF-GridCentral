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
    public class ItemService
    {

        private static ItemService instance;

        public static ItemService Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new ItemService();
                }

                return instance;
            }
        }

        public async Task<string> Additem(mUserItem item)
        {
            try
            {
                var Itemcreds = Newtonsoft.Json.JsonConvert.SerializeObject(item);

                HttpContent ItemContent = new StringContent(Itemcreds, Encoding.UTF8, "application/json");

                using (var client = new HttpClient())
                {
                    HttpResponseMessage response = await client.PostAsync(Keys.Url_Main + "item/add", ItemContent);

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

        public async Task<ObservableCollection<mUserItem>> FetchRandom(int amount, string category, string by, int len)
        {
            var link = Keys.Url_Main + "item/get/random?amount=" + amount.ToString() + "&len=" + len.ToString();
            if (!String.IsNullOrEmpty(category))
            {
                link += "&category=" + category;
            }
            if (!String.IsNullOrEmpty(by))
            {
                link += "&by=" + by;
            }

            try
            {
                var httpClient = new HttpClient();


                var response = await httpClient.GetAsync(link);

                response.EnsureSuccessStatusCode();

                string content = await response.Content.ReadAsStringAsync();


                mServerCallback callback = Newtonsoft.Json.JsonConvert.DeserializeObject<mServerCallback>(content);

                if (callback.Status == "true")
                {
                    ObservableCollection<mUserItem> newitems = new ObservableCollection<mUserItem>();
                    newitems = Newtonsoft.Json.JsonConvert.DeserializeObject<ObservableCollection<mUserItem>>(callback.Data.ToString());
                    if (newitems.Count < 1) return null;

                    return newitems;

                }

                DialogService.ShowError(callback.Mess);
                return null;

            }
            catch (Exception ex)
            {
                Debug.WriteLine(Keys.TAG + ex);
                DialogService.ShowErrorToast(Strings.HttpFailed);
                return null;
            }
        }

        public async Task<ObservableCollection<mUserItem>> FetchUsersItems(string userEmail,int amount,int len,string searchTxt=null)
        {
            var link = Keys.Url_Main + "item/user-items/" + userEmail+"?amount="+amount.ToString()+"&len="+len.ToString();

            if(searchTxt != null)
            {
                link += "&Search=" + searchTxt;
            }
            
            try
            {
                var httpClient = new HttpClient();

                var response = await httpClient.GetAsync(link);

                response.EnsureSuccessStatusCode();

                string content = await response.Content.ReadAsStringAsync();

                mServerCallback callback = Newtonsoft.Json.JsonConvert.DeserializeObject<mServerCallback>(content);

                if (callback.Status == "true")
                {
                    ObservableCollection<mUserItem> newitems = new ObservableCollection<mUserItem>();

                    newitems = Newtonsoft.Json.JsonConvert.DeserializeObject<ObservableCollection<mUserItem>>(callback.Data.ToString());


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

        public async Task<string> DeleteItem(string Itemid)
        {
            try
            {

                using (var client = new HttpClient())
                {

                    HttpResponseMessage response = await client.DeleteAsync(Keys.Url_Main + "item/delete/" + Itemid);

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

        public async Task<string> UpdateItem(mUserItem item)
        {
            try
            {
                var Itemcreds = Newtonsoft.Json.JsonConvert.SerializeObject(item);

                HttpContent ItemContent = new StringContent(Itemcreds, Encoding.UTF8, "application/json");

                using (var client = new HttpClient())
                {

                    HttpResponseMessage response = await client.PutAsync(Keys.Url_Main + "item/update", ItemContent);

                    using (HttpContent spawn = response.Content)
                    {
                        string content = await spawn.ReadAsStringAsync();

                        mServerCallback callback = Newtonsoft.Json.JsonConvert.DeserializeObject<mServerCallback>(content);

                        if (callback.Status == "true")
                        {
                            return "true";
                        }

                        DialogService.ShowError(Strings.ServerFailed);
                        return callback.Mess;
                    }

                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(Keys.TAG + ex);
                return Strings.ServerFailed;
            }
        }

        public async Task<ObservableCollection<mUserItem>> FetchSearch(string link)
        {
            try
            {
                var httpClient = new HttpClient();


                var response = await httpClient.GetAsync(link);

                response.EnsureSuccessStatusCode();

                string content = await response.Content.ReadAsStringAsync();

                mServerCallback callback = Newtonsoft.Json.JsonConvert.DeserializeObject<mServerCallback>(content);

                if (callback.Status == "true")
                {
                    ObservableCollection<mUserItem> newitems = new ObservableCollection<mUserItem>();
                    newitems = Newtonsoft.Json.JsonConvert.DeserializeObject<ObservableCollection<mUserItem>>(callback.Data.ToString());
                    if (newitems.Count < 1) return null;

                    return newitems;

                }

                DialogService.ShowError(callback.Mess);
                return null;

            }
            catch (Exception ex)
            {
                Debug.WriteLine(Keys.TAG + ex);
                DialogService.ShowErrorToast(Strings.HttpFailed);
                return null;
            }
        }

        public async Task<mUserItem> FetchItem(string Itemid)
        {
            try
            {

                var httpClient = new HttpClient();

                var response = await httpClient.GetAsync(Keys.Url_Main + "item/" + Itemid);

                response.EnsureSuccessStatusCode();

                string content = await response.Content.ReadAsStringAsync();

                mServerCallback callback = Newtonsoft.Json.JsonConvert.DeserializeObject<mServerCallback>(content);

                if (callback.Status == "true")
                {
                    mUserItem newitem = new mUserItem();

                    newitem = Newtonsoft.Json.JsonConvert.DeserializeObject<mUserItem>(callback.Data.ToString());


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
