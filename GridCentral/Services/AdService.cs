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
    public class AdService
    {
        private static AdService instance;

        public static AdService Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new AdService();
                }

                return instance;
            }
        }

        public async Task<ObservableCollection<mAdvert>> FetchAds(string location)
        {
            try
            {
                var httpClient = new HttpClient();

                var response = await httpClient.GetAsync(Keys.Url_Main + "ads/get/" + location);

                response.EnsureSuccessStatusCode();

                string content = await response.Content.ReadAsStringAsync();

                mServerCallback callback = Newtonsoft.Json.JsonConvert.DeserializeObject<mServerCallback>(content);

                if (callback.Status == "true")
                {
                    ObservableCollection<mAdvert> newitems = new ObservableCollection<mAdvert>();
                    newitems = Newtonsoft.Json.JsonConvert.DeserializeObject<ObservableCollection<mAdvert>>(callback.Data.ToString());
                    if (newitems.Count < 1)
                    {
                        return null;
                    }

                    return newitems;
                }
                else
                {
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
