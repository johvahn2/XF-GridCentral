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

namespace GridCentral.Views.Auth
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ProfileSignUp : ContentPage
    {

        public ProfileSignUp()
        {
            viewModel = new Auth_SignUp_ViewModel(new PageService(Navigation));
            InitializeComponent();

            SetStrings();
        }

        private void SetStrings()
        {
            SaveBtn.Text = Strings.Signup;
            Firstname.Placeholder = Strings.First_Name;
            Lastname.Placeholder = Strings.Last_Name;
            Title.Text = Strings.Profile_Information;
        }

        private void SetupEventHandlers()
        {
            Firstname.Completed += (sender, e) =>
            {
                Lastname.Focus();
            };

            Lastname.Completed += (sender, e) =>
            {
                GenderPicker.Focus();
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