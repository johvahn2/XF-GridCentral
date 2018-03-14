using GridCentral.Helpers;
using GridCentral.Services;
using GridCentral.ViewModels;
using GridCentral.Views.Navigation.Auth;
using GridCentral.Views.Navigation.Settings;
using GridCentral.Views.Navigation.WalkThroughs;
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
    public partial class Login : ContentPage
    {
        public Login()
        {
            viewModel = new Auth_Login_ViewModel(new PageService(Navigation));
            InitializeComponent();
            NavigationPage.SetHasNavigationBar(this, false);

            SetStrings();

            ForgotPass.GestureRecognizers.Add(new TapGestureRecognizer
            {
                Command = new Command(async () =>
                {
                    await Navigation.PushModalAsync(new PasswordRecovery());
                })
            });

        }

        private void SetStrings()
        {
            LoginBtn.Text = Strings.Login;
            SignUpBtn.Text = Strings.Signup;
            ForgotPass.Text = Strings.Forgot_Your_Password;
            PasswordEntry.Placeholder = Strings.Password;
            EmailEntry.Placeholder = Strings.Email;
            Sublbl.Text = Strings.Use_Cred_To_Login;
            Title.Text = Strings.Welcome_To_GridCentral;
        }

        async void OnCloseButtonClicked(object sender, EventArgs args)
        {
             await Navigation.PopModalAsync();
            //Application.Current.MainPage = new RootPage(false);
        }

        public Auth_Login_ViewModel viewModel
        {
            get { return BindingContext as Auth_Login_ViewModel; }
            set { BindingContext = value; }
        }

        public static bool IsPageInNavigationStack<TPage>(INavigation navigation) where TPage : Page
        {
            if (navigation.NavigationStack.Count > 1)
            {
                var last = navigation.NavigationStack[navigation.NavigationStack.Count - 2];

                if (last is TPage)
                {
                    return true;
                }
            }
            return false;
        }
    }
}
