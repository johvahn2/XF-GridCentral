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
    public partial class NewPassword : ContentPage
    {
        public NewPassword(string email)
        {
            viewModel = new Auth_PasswordRecovery_ViewModel(new PageService(Navigation),email); 
            InitializeComponent();
            NavigationPage.SetHasBackButton(this, false);

        }

        public Auth_PasswordRecovery_ViewModel viewModel
        {
            get { return BindingContext as Auth_PasswordRecovery_ViewModel; }
            set { BindingContext = value; }
        }


        protected override bool OnBackButtonPressed()
        {
            return true;
        }
    }
}