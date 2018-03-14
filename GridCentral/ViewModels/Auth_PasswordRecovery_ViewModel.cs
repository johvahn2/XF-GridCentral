using GridCentral.Helpers;
using GridCentral.Interfaces;
using GridCentral.Models;
using GridCentral.Services;
using GridCentral.Views.Auth;
using GridCentral.Views.Navigation;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace GridCentral.ViewModels
{
    public class Auth_PasswordRecovery_ViewModel : Base_ViewModel
    {
        #region Bind Property
        string _email;
        string _token;
        string _newpassword;
        string _repassword;
        bool _tokenSent = false;
        bool _EmailShow = true;
        string _proccedBtn = Strings.Send;

        public bool TokenSent
        {
            get { return _tokenSent; }
            set { _tokenSent = value; OnPropertyChanged("TokenSent"); }
        }

        public bool EmailShow
        {
            get { return _EmailShow; }
            set { _EmailShow = value; OnPropertyChanged("EmailShow"); }
        }

        public string ProccedBtn
        {
            get { return _proccedBtn; }
            set { _proccedBtn = value; OnPropertyChanged("ProccedBtn"); }
        }

        public string Email
        {
            get { return _email; }
            set { _email = value; OnPropertyChanged("Email"); }
        }
        public string NewPassword
        {
            get { return _newpassword; }
            set { _newpassword = value; OnPropertyChanged("NewPassword"); }
        }
        public string RePassword
        {
            get { return _repassword; }
            set { _repassword = value; OnPropertyChanged("Repassword"); }
        }
        public string Token
        {
            get { return _token; }
            set { _token = value; OnPropertyChanged("Token"); }
        }
        public ICommand ProccedCommand { get; private set; }
        public ICommand UpdateCommand { get; private set; }

        IPageService _pageService;
        #endregion


        public Auth_PasswordRecovery_ViewModel(IPageService pageService, string email=null)
        {
            if(email != null)
            {
                Email = email;
            }
            _pageService = pageService;
            ProccedCommand = new Command(() => Procced());
            UpdateCommand = new Command(() => UpdatePass());
        }

        private async void UpdatePass()
        {
            if (String.IsNullOrEmpty(NewPassword) || String.IsNullOrEmpty(RePassword))
            {
                DialogService.ShowToast(Strings.Enter_All_Fields);
                return;
            }

            if(NewPassword != RePassword)
            {
                DialogService.ShowToast(Strings.Password_Dont_Match);
                return;
            }

            if (IsBusy) return;

            try
            {
                IsBusy = true;

                mAccount newpassword = new mAccount { NewPassword = NewPassword, Email = Email };

                DialogService.ShowLoading(Strings.Proccessing);

                var result = await UserService.Instance.UpdatePassword(newpassword);

                DialogService.HideLoading();

                if (result != Strings.True) return;

                _pageService.ShowMain(new RootPage());


            }
            catch (Exception ex)
            {
                DialogService.ShowErrorToast(Strings.SomethingWrong);
                Debug.WriteLine(Keys.TAG + ex);
            }
            finally { IsBusy = false; }
        }

        private void Procced()
        {
            if(ProccedBtn == Strings.Send && !TokenSent)
            {
                SendToken();
            }
            else if(ProccedBtn == Strings.Activate && TokenSent)
            {
                ActivateToken();
            }
        }

        private async void ActivateToken()
        {
            if (String.IsNullOrEmpty(Token))
            {
                DialogService.ShowToast(Strings.Enter_The_Token);
                return;
            }
            if (IsBusy) return;

            try
            {
                IsBusy = true;

                DialogService.ShowLoading(Strings.Proccessing);

                var result = await UserService.Instance.ValidateToken(Token);

                DialogService.HideLoading();

                if (result != Strings.True)
                {
                    DialogService.ShowError(result);
                    return;
                }

                _pageService.ShowMain(new NavigationPage(new NewPassword(Email)));

            }
            catch (Exception ex)
            {
                DialogService.ShowErrorToast(Strings.SomethingWrong);
                Debug.WriteLine(Keys.TAG + ex);
            }
            finally { IsBusy = false; }
        }

        private async void SendToken()
        {
            if (String.IsNullOrEmpty(Email))
            {
                DialogService.ShowToast(Strings.Enter_An_Email);
                return;
            }
            if (IsBusy) return;

            try
            {
                IsBusy = true;

                DialogService.ShowLoading(Strings.Proccessing);

                var result =await  UserService.Instance.EmailToken(Email);

                DialogService.HideLoading();

                if (result != Strings.True) return;

                TokenSent = true;EmailShow = false;
                ProccedBtn = "Activate";

            }catch(Exception ex)
            {
                DialogService.ShowErrorToast(Strings.SomethingWrong);
                Debug.WriteLine(Keys.TAG + ex);
            }
            finally { IsBusy = false; }
        }
    }
}
