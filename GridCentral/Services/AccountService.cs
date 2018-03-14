using GridCentral.Helpers;
using GridCentral.Models;
using GridCentral.ViewModels;
using GridCentral.Views.Navigation;
using Plugin.Settings;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace GridCentral.Services
{
    public class AccountService : Base_ViewModel
    {
        private bool hasToken = false;
        private static AccountService instance;

        public static AccountService Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new AccountService();
                }

                return instance;
            }
        }

        mAccount _current_account;
        public mAccount Current_Account { get { return _current_account; } set { _current_account = value; OnPropertyChanged("Current_Account"); } }

        private string CurrentUser_Data { get; set; }

        public bool ReadyToSignIn
        {
            get { return !string.IsNullOrEmpty(CurrentUser_Data); }
        }


        private AccountService()
        {
            FetchAuthenticationToken();

            if (ReadyToSignIn)
            {
                Current_Account = Newtonsoft.Json.JsonConvert.DeserializeObject<mAccount>(CurrentUser_Data);
                if (String.IsNullOrEmpty(Current_Account.Displayname)){ Current_Account.Displayname = Current_Account.FirstName; }
                //UpdateToken(CrossSettings.Current.GetValueOrDefault<string>("Token"));
            }

        }

        void FetchAuthenticationToken()
        {
            CurrentUser_Data = CrossSettings.Current.GetValueOrDefault<string>("Current_User");
        }

        public void UpdateCurr_Account(mAccount update)
        {
            Current_Account = update;
        }

        public async Task<string> Register(mAccount account)
        {
            try
            {
                var Usercreds = Newtonsoft.Json.JsonConvert.SerializeObject(account);

                HttpContent UserContent = new StringContent(Usercreds, Encoding.UTF8, "application/json");

                using (var client = new HttpClient())
                {
                    HttpResponseMessage response = await client.PostAsync(Keys.Url_Main + "auth/register", UserContent);

                    using (HttpContent spawn = response.Content)
                    {
                        string content = await spawn.ReadAsStringAsync();
                        mServerCallback callback = Newtonsoft.Json.JsonConvert.DeserializeObject<mServerCallback>(content);

                        if (callback.Status == "true")
                        {
                            Current_Account = Newtonsoft.Json.JsonConvert.DeserializeObject<mAccount>(callback.Data.ToString());

                            CrossSettings.Current.AddOrUpdateValue<string>("Current_User", callback.Data.ToString());
                            await UpdateToken(CrossSettings.Current.GetValueOrDefault<string>("Token"));

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

        public async Task<string> Login(mAccount account)
        {
            try
            {
                var Usercreds = Newtonsoft.Json.JsonConvert.SerializeObject(account);

                HttpContent UserContent = new StringContent(Usercreds, Encoding.UTF8, "application/json");

                using (var client = new HttpClient())
                {
                    HttpResponseMessage response = await client.PostAsync(Keys.Url_Main + "auth/login", UserContent);

                    using (HttpContent spawn = response.Content)
                    {
                        string content = await spawn.ReadAsStringAsync();

                        mServerCallback callback = Newtonsoft.Json.JsonConvert.DeserializeObject<mServerCallback>(content);

                        if (callback.Status == "true")
                        {
                           Instance.Current_Account = Newtonsoft.Json.JsonConvert.DeserializeObject<mAccount>(callback.Data.ToString());
                           Current_Account = Newtonsoft.Json.JsonConvert.DeserializeObject<mAccount>(callback.Data.ToString());

                            CrossSettings.Current.AddOrUpdateValue<string>("Current_User", callback.Data.ToString());

                            await UpdateToken(CrossSettings.Current.GetValueOrDefault<string>("Token"));

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

        public async Task<string> UpdateAccount(mAccount account)
        {
            try
            {
                var Usercreds = Newtonsoft.Json.JsonConvert.SerializeObject(account);

                HttpContent UserContent = new StringContent(Usercreds, Encoding.UTF8, "application/json");

                using (var client = new HttpClient())
                {
                    HttpResponseMessage response = await client.PutAsync(Keys.Url_Main + "user/update-profile", UserContent);

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
            catch(Exception ex)
            {
                Debug.WriteLine(Keys.TAG + ex);
                DialogService.ShowError(Strings.HttpFailed);
                return null;
            }
        }

        public async Task UpdateStatus(mAccount account)
        {
            try
            {
                var Usercreds = Newtonsoft.Json.JsonConvert.SerializeObject(account);

                HttpContent UserContent = new StringContent(Usercreds, Encoding.UTF8, "application/json");

                using (var client = new HttpClient())
                {
                    HttpResponseMessage response = await client.PutAsync(Keys.Url_Main + "user/update-status", UserContent);

                    using (HttpContent spawn = response.Content)
                    {
                        string content = await spawn.ReadAsStringAsync();

                        mServerCallback callback = Newtonsoft.Json.JsonConvert.DeserializeObject<mServerCallback>(content);

                        if (callback.Status == "true")
                        {
                            return;
                        }
                        else
                        {
                            DialogService.ShowErrorToast(callback.Mess);
                        }


                    }
                }

            }
            catch (Exception ex)
            {
                Debug.WriteLine(Keys.TAG + ex);
                DialogService.ShowError(Strings.HttpFailed);
            }
        }

        public async void autho(Page page,string Diss_mess="Go Back")
        {
            var result = await DialogService.DisplayAlert("Login", Diss_mess, "Authentication", "You Must Login First");

            DialogService.ShowLoading();
            if (result)
            {
                //Application.Current.MainPage = new RootPage();
                await Application.Current.MainPage.Navigation.PushModalAsync(new Login());

               // Application.Current.MainPage = new NavigationPage(new Login());

            }
            else
            {
                if (page != null)
                    Application.Current.MainPage = page;


            }
            DialogService.HideLoading();
        }


        public void SignOut()
        {
            AccountService.Instance.CurrentUser_Data = "";
            AccountService.Instance.Current_Account = null;
            CrossSettings.Current.Remove("Current_User");
            Application.Current.MainPage = new RootPage();
            Application.Current.MainPage.Navigation.PushModalAsync(new Login());
        }

        #region Push Token
        public void getToken(string token)
        {
            if (ReadyToSignIn)
            {
                hasToken = true;
                UpdateToken(token);
            }
            CrossSettings.Current.AddOrUpdateValue<string>("Token", token);
            hasToken = true;
        }

        public async Task UpdateToken(string token) //When someone logout and login into another account the token wont update until they restart the app
        {
            if (!hasToken) return;

            Current_Account.nToken = token;
            try
            {
                var Usercreds = Newtonsoft.Json.JsonConvert.SerializeObject(Current_Account);

                HttpContent UserContent = new StringContent(Usercreds, Encoding.UTF8, "application/json");

                using (var client = new HttpClient())
                {
                    HttpResponseMessage response = await client.PutAsync(Keys.Url_Main + "user/update-token", UserContent);

                    using (HttpContent spawn = response.Content)
                    {
                        string content = await spawn.ReadAsStringAsync();

                        mServerCallback callback = Newtonsoft.Json.JsonConvert.DeserializeObject<mServerCallback>(content);

                        if (callback.Status == "true")
                        {
                            hasToken = false;
                            Debug.WriteLine(Keys.TAG + callback.Mess);

                        }
                        else
                        {
                            Debug.WriteLine(Keys.TAG + "Fail To Update Token");
                        }


                    }
                }

            }
            catch (Exception ex)
            {
                Debug.WriteLine(Keys.TAG + ex);
                DialogService.ShowError(Strings.HttpFailed);
            }
        }
        #endregion

    }
}
