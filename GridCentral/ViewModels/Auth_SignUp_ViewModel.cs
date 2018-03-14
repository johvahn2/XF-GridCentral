using GridCentral.Interfaces;
using GridCentral.Models;
using GridCentral.Services;
using GridCentral.Views.Auth;
using GridCentral.Views.Auth.ProfileSetup_Pages;
using Plugin.Settings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace GridCentral.ViewModels
{
    public class Auth_SignUp_ViewModel : Base_ViewModel
    {
        #region Bind Property
        string _email;
        string _password;
        string _repassword;
        string _firstname;
        string _lastname;
        string _phonenumber;
        string _address;
        List<string> _gender = new List<string>() { "Male","Female"};
        int _GenderTypeIndex;
        DateTime _bDay = DateTime.Now.Date;

        public string Email
        {
            get { return _email; }
            set { _email = value; OnPropertyChanged("Email");
                CrossSettings.Current.AddOrUpdateValue<string>("Email", value); }
        }

        public string Password
        {
            get { return _password; }
            set { _password = value; OnPropertyChanged("Password");
                CrossSettings.Current.AddOrUpdateValue<string>("Password", value);}
        }

        public string RePassword
        {
            get { return _repassword; }
            set
            { _repassword = value; OnPropertyChanged("RePassword");}
        }

        public string FirstName
        {
            get { return _firstname; }
            set { _firstname = value; OnPropertyChanged("FirstName"); }
        }

        public string LastName
        {
            get { return _lastname; }
            set { _lastname = value; OnPropertyChanged("LastName"); }
        }

        public string PhoneNumber
        {
            get { return _phonenumber; }
            set
            { _phonenumber = value; OnPropertyChanged("PhoneNumber");
            CrossSettings.Current.AddOrUpdateValue<string>("PhoneNumber", value);}
        }
        public string Address
        {
            get { return _address; }
            set
            {_address = value; OnPropertyChanged("Address");}
        }

        public DateTime bDay
        {
            get { return _bDay; }
            set { _bDay = value; OnPropertyChanged("bDay"); }
        }

        public List<String> Gender
        {
            get { return _gender; }
        }

        public int GenderTypeIndex
        {
            get { return _GenderTypeIndex; }
            set { _GenderTypeIndex = value; OnPropertyChanged("GenderTypeIndex"); }
        }
        #endregion

        public ICommand SignUpCommand { get; private set; }
        public ICommand SaveCommand { get; private set; }


        private readonly IPageService _pageService;

        public Auth_SignUp_ViewModel(IPageService pageService)
        {
            _pageService = pageService;

            SignUpCommand = new Command(async () => await SignUpAction());
            SaveCommand = new Command(async () => await SaveAction());
        }

        private async Task SignUpAction()
        {
            if (string.IsNullOrEmpty(Email) || string.IsNullOrEmpty(Password) || string.IsNullOrEmpty(RePassword) || string.IsNullOrEmpty(PhoneNumber)) {
                DialogService.ShowError("Enter All Fields"); return;
            }
            if (RePassword != Password)
            {
                DialogService.ShowErrorToast("Password Does Not Match!");
                return;
            }

            await _pageService.PushModalAsync(new ProfileSignUp());
        }

        private async Task SaveAction()
        {
            if (IsBusy) return;

            IsBusy = true;

            DialogService.ShowLoading();


            try
            {

                var result = await SignUp();
                DialogService.HideLoading();

                if (result == "true")
                {
                    await _pageService.PushModalAsync(new ProfileImage());
                   // DialogService.ShowSuccess("DONE");
                }
                else
                {
                    DialogService.ShowError(result);
                }

            }
            catch (Exception ex)
            {
                DialogService.ShowError("Try Later");
            }
            finally { IsBusy = false; }
        }

        private async Task<string> SignUp()
        {
            var account = new mAccount
            {
                PhoneNumber = CrossSettings.Current.GetValueOrDefault<string>("PhoneNumber", null),
                Address = Address,
                FirstName = FirstName,
                LastName = LastName,
                Gender = Gender[GenderTypeIndex],
                bDay = bDay,
                Email = CrossSettings.Current.GetValueOrDefault<string>("Email", null),
                Password = CrossSettings.Current.GetValueOrDefault<string>("Password", null)
            };
            return await AccountService.Instance.Register(account);

        }
    }
}
