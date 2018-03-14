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

namespace GridCentral.Views.Navigation.Auth
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PasswordRecovery : ContentPage
    {
        public PasswordRecovery()
        {
            viewModel = new Auth_PasswordRecovery_ViewModel(new PageService(Navigation));
            InitializeComponent();
            NavigationPage.SetHasNavigationBar(this, false);

            SetStrings();

        }

        private void SetStrings()
        {
            Title.Text = Strings.Forgot_Your_Password;
            ValidEmailLbl.Text = Strings.Enter_Valid_Email;
            EmailEntry.Placeholder = Strings.Email;
            TokenNote.Text = Strings.Check_Spam;
            TokenEntry.Placeholder = Strings.Enter_Token;
        }

        private async void OnCloseButtonClicked(object sender, EventArgs args)
        {
            await Navigation.PopModalAsync();
        }


        public Auth_PasswordRecovery_ViewModel viewModel
        {
            get { return BindingContext as Auth_PasswordRecovery_ViewModel; }
            set { BindingContext = value; }
        }
    }
}
