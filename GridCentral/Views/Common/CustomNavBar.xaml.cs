using GridCentral.Services;
using GridCentral.ViewModels;
using GridCentral.Views.Cart;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace GridCentral.Views.Common
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CustomNavBar : ContentView
    {
        public CustomNavBar()
        {
            viewModel = new Navbar_Search_ViewModel(new PageService(Navigation));
            InitializeComponent();
            NavigationPage.SetBackButtonTitle(this, "");
            SearchFor.SetBinding(SearchBar.SearchCommandProperty, "SearchTxtCommand");

            SearchFor.Text = Navbar_Search_ViewModel.tempo;
        }

        public void OnHamburgerIconTapped(Object sender, EventArgs e)
        {
            var currentPage = App.Current.MainPage;
            var master = currentPage as MasterDetailPage;
            master.IsPresented = true;
        }

        public async void OnCogIconTapped(Object sender, EventArgs e)//Cart
        {
            if(AccountService.Instance.Current_Account == null)
            {
                AccountService.Instance.autho(null, "Dismiss");
                return;
            }
            NavigationPage.SetBackButtonTitle(this, "");
            await Navigation.PushAsync(new Connector());
            NavigationPage.SetBackButtonTitle(this, "");
        }

        public Navbar_Search_ViewModel viewModel
        {
            get { return BindingContext as Navbar_Search_ViewModel; }
            set { BindingContext = value; }
        }
    }
}
