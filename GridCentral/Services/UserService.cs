using GridCentral.Helpers;
using GridCentral.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace GridCentral.Services
{
    public class UserService
    {
        private static UserService instance;

        public static UserService Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new UserService();
                }

                return instance;
            }
        }


        public async Task<mAccount> FetchUser(string Email)
        {
            try
            {
                var httpClient = new HttpClient();

                var response = await httpClient.GetAsync(Keys.Url_Main + "user/get-profile/" + Email);

                response.EnsureSuccessStatusCode();

                string content = await response.Content.ReadAsStringAsync();

                mServerCallback callback = Newtonsoft.Json.JsonConvert.DeserializeObject<mServerCallback>(content);

                if (callback.Status == "true")
                {
                    mAccount User = Newtonsoft.Json.JsonConvert.DeserializeObject<mAccount>(callback.Data.ToString());
                    return User;

                }
                DialogService.ShowError(callback.Mess);
                return null;

            }
            catch (Exception ex)
            {
                DialogService.ShowErrorToast(Strings.HttpFailed);
                Debug.WriteLine(Keys.TAG + ex);
                return null;
            }

        }


        public async Task<string> EmailToken(string Email)
        {
            try
            {
                var httpClient = new HttpClient();

                var response = await httpClient.GetAsync(Keys.Url_Main + "auth/reset-token/" + Email);

                response.EnsureSuccessStatusCode();

                string content = await response.Content.ReadAsStringAsync();

                mServerCallback callback = Newtonsoft.Json.JsonConvert.DeserializeObject<mServerCallback>(content);

                if (callback.Status == Strings.True)
                {
                    return Strings.True;
                }
                DialogService.ShowError(callback.Mess);
                return null;

            }
            catch (Exception ex)
            {
                DialogService.ShowErrorToast(Strings.HttpFailed);
                Debug.WriteLine(Keys.TAG + ex);
                return null;
            }

        }
        public async Task<string> ValidateToken(string Token)
        {
            try
            {
                var httpClient = new HttpClient();

                var response = await httpClient.GetAsync(Keys.Url_Main + "auth/reset/" + Token);

                response.EnsureSuccessStatusCode();

                string content = await response.Content.ReadAsStringAsync();

                mServerCallback callback = Newtonsoft.Json.JsonConvert.DeserializeObject<mServerCallback>(content);

                if (callback.Status == Strings.True)
                {
                    return Strings.True;
                }
                return callback.Mess;

            }
            catch (Exception ex)
            {
                DialogService.ShowErrorToast(Strings.HttpFailed);
                Debug.WriteLine(Keys.TAG + ex);
                return Strings.HttpFailed;
            }

        }

        public async Task<string> UpdatePassword(mAccount newpass)
        {
            try
            {
                var Itemcreds = Newtonsoft.Json.JsonConvert.SerializeObject(newpass);

                HttpContent ItemContent = new StringContent(Itemcreds, Encoding.UTF8, "application/json");

                using (var client = new HttpClient())
                {
                    HttpResponseMessage response = await client.PutAsync(Keys.Url_Main + "auth/reset-password", ItemContent);

                    using (HttpContent spawn = response.Content)
                    {
                        string content = await spawn.ReadAsStringAsync();

                        mServerCallback callback = Newtonsoft.Json.JsonConvert.DeserializeObject<mServerCallback>(content);

                        if (callback.Status == "true")
                        {

                            return "true";
                        }

                        DialogService.ShowError(callback.Mess);
                        return null;
                    }
                }

            }
            catch (Exception ex)
            {
                Debug.WriteLine(Keys.TAG + ex);
                DialogService.ShowError(Strings.HttpFailed);
                return null;
            }
        }


    }
}
