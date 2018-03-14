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
    public class NotifyService
    {

        private static NotifyService instance;

        public static NotifyService Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new NotifyService();
                }

                return instance;
            }
        }

        public async Task<ObservableCollection<mNotify>> FetchNotfiy(string Email)
        {
            try
            {
                var httpClient = new HttpClient();

                var response = await httpClient.GetAsync(Keys.Url_Main + "notifaction/get/" + Email);

                response.EnsureSuccessStatusCode();

                string content = await response.Content.ReadAsStringAsync();

                mServerCallback callback = Newtonsoft.Json.JsonConvert.DeserializeObject<mServerCallback>(content);

                if (callback.Status == "true")
                {
                    ObservableCollection<mNotify> newitems = new ObservableCollection<mNotify>();
                    newitems = Newtonsoft.Json.JsonConvert.DeserializeObject<ObservableCollection<mNotify>>(callback.Data.ToString());
                    if (newitems.Count < 1)
                    {

                        return null;
                    }


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

        public async Task<bool> DeleteNotify(string email)
        {
            try
            {

                using (var client = new HttpClient())
                {

                    HttpResponseMessage response = await client.DeleteAsync(Keys.Url_Main + "notifaction/delete/" + email);

                    using (HttpContent spawn = response.Content)
                    {
                        string content = await spawn.ReadAsStringAsync();

                        mServerCallback callback = Newtonsoft.Json.JsonConvert.DeserializeObject<mServerCallback>(content);

                        if (callback.Status == "true")
                        {
                            return true;
                        }

                        DialogService.ShowError(callback.Mess);
                        return false;
                    }
                }


            }
            catch (Exception ex)
            {
                Debug.WriteLine(Keys.TAG + ex);
                DialogService.ShowErrorToast(Strings.HttpFailed);
                return false;
            }
        }
    }
}
