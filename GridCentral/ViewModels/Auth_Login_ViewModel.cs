using GridCentral.Helpers;
using GridCentral.Interfaces;
using GridCentral.Models;
using GridCentral.Services;
using GridCentral.Views.Navigation;
using Microsoft.AppCenter.Crashes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace GridCentral.ViewModels
{
    public class Auth_Login_ViewModel : Base_ViewModel
    {

        #region Bind Property
        string _email;
        string _password;

        public string Email
        {
            get { return _email; }
            set { _email = value; OnPropertyChanged("Email"); }
        }

        public string Password
        {
            get { return _password; }
            set { _password = value; OnPropertyChanged("Password"); }
        }

        public ICommand SignInCommand { get; private set; }
        public ICommand GoSignUpCommand { get; private set; }
        private IPageService _pageService;
        #endregion

        public Auth_Login_ViewModel(IPageService pageService)
        {
            _pageService = pageService;

            SignInCommand = new Command(async () => await SignInAction());
            GoSignUpCommand = new Command(() => GoSignUpAction());
        }

        private void GoSignUpAction()
        {
            _pageService.PushModalAsync(new SignUp());
        }

        public async Task SignInAction()
        {
            if (IsBusy) return;

            IsBusy = true;

            if(String.IsNullOrEmpty(Email) || String.IsNullOrEmpty(Password))
            {
                DialogService.ShowError(Strings.Enter_All_Credentials);
                return;
            }

            DialogService.ShowLoading(Strings.Signing_In);
            try
            {
                var result = await SignIn();

                DialogService.HideLoading();

                if (result == "true")
                {
                    //DependencyService.Get<IMetricsManagerService>().TrackEvent("LoginSuccess");
                    // HockeyApp.MetricsManager.TrackEvent("LoginSuccess");
                    //DialogService.ShowToast("Logged In");
                    _pageService.ShowMain(new RootPage());
                }
                else
                {
                    DialogService.ShowError(result);
                }
            }
            catch (Exception ex) { DialogService.ShowError(Strings.Try_Later); Crashes.TrackError(ex);}
            finally { IsBusy = false; }

        }

        private async Task<string> SignIn()
        {
            var account = new mAccount
            {
                Email = Email,
                Password = Password
            };

            return await AccountService.Instance.Login(account);
        }
    }
}
