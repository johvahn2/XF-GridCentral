using GridCentral.Helpers;
using GridCentral.Services;
using GridCentral.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace GridCentral.Views.Navigation
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SignUp : ContentPage
    {
        public SignUp()
        {
            viewModel = new Auth_SignUp_ViewModel(new PageService(Navigation));
            InitializeComponent();
            SetStrings();
            SetupEventHandlers();
        }

        private void SetStrings()
        {
            ValidEmailLbl.Text = Strings.Enter_Valid_Email;
            SignupBtn.Text = Strings.Next;
            RePassword.Placeholder = Strings.RePassword;
            Password.Placeholder = Strings.Password;
            Phonenumber.Placeholder = Strings.Phone_Number;
            Email.Placeholder = Strings.Email;
            Sublbl.Text = Strings.Enter_Your_Infomation;
            Title.Text = Strings.RegisterNow;
        }

        private void SetupEventHandlers()
        {
            Email.Completed += (sender, e) =>
            {
                Phonenumber.Focus();
            };

            Phonenumber.Completed += (sender, e) =>
            {
                Password.Focus();
            };


            Password.Completed += (sender, e) =>
            {
               RePassword.Focus();
            };

            RePassword.Completed += (sender, e) =>
            {
                SignupBtn.Focus();
            };
        }

        async void OnCloseButtonClicked(object sender, EventArgs args)
        {
            await Navigation.PopModalAsync();
        }

        public Auth_SignUp_ViewModel viewModel
        {
            get { return BindingContext as Auth_SignUp_ViewModel; }
            set { BindingContext = value; }
        }
    }
}
