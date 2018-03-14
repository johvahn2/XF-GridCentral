using GridCentral.Helpers;
using GridCentral.Models;
using GridCentral.Services;
using GridCentral.ViewModels;
using GridCentral.Views.ObjectViews;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace GridCentral.Views.Cart
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Cart : ContentPage
    {
        public Cart()
        {
            viewModel = new Cart_MyCart_ViewModel(new PageService(Navigation));
            InitializeComponent();
            SetStrings();

            listView.ItemSelected += ListView_ItemSelected;
        }

        private void SetStrings()
        {
            NoItemlbl.Text = Strings.No_Items_Found;
            CheckoutBtn.Text = Strings.Checkout;

        }

        private void ListView_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            var item = e.SelectedItem as mCart;

            if (item != null)
            {
                Product listitem = (from itm in viewModel.MyProductList
                                  where itm.Name == item.bName
                                  select itm)
                        .FirstOrDefault<Product>();

                Navigation.PushAsync(new ProductView(listitem));
            }
            listView.SelectedItem = null;
        }

        public Cart_MyCart_ViewModel viewModel
        {
            get { return BindingContext as Cart_MyCart_ViewModel; }
            set { BindingContext = value; }
        }
    }
}