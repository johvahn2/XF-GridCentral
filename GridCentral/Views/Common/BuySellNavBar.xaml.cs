using GridCentral.Services;
using GridCentral.ViewModels;
using GridCentral.Views.Profile;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace GridCentral.Views.Common
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class BuySellNavBar : ContentView
    {
        public BuySellNavBar()
        {
            viewModel = new Navbar_Search_ViewModel(new PageService(Navigation));
            InitializeComponent();

            SearchFor.SetBinding(SearchBar.SearchCommandProperty, "SearchTxtCommand");

            SearchFor.Text = Navbar_Search_ViewModel.tempo;
        }

        public void OnHamburgerIconTapped(Object sender, EventArgs e)
        {
            var currentPage = App.Current.MainPage;
            var master = currentPage as MasterDetailPage;
            master.IsPresented = true;
        }


        public async void OnCogIconTapped(Object sender, EventArgs e)//my items
        {
            if (AccountService.Instance.Current_Account == null)
            {
                AccountService.Instance.autho(null, "Dismiss");
                return;
            }

            await Navigation.PushAsync(new MyItems());
        }

        public Navbar_Search_ViewModel viewModel
        {
            get { return BindingContext as Navbar_Search_ViewModel; }
            set { BindingContext = value; }
        }
    }
}