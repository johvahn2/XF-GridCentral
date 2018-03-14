using GridCentral.Models;
using GridCentral.ViewModels;
using GridCentral.Views.Cart;
using GridCentral.Views.ObjectViews;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace GridCentral.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class InterestList : ContentPage
    {
        public InterestList()
        {
            viewModel = new Product_InterestList_ViewModel();
            InitializeComponent();
            GoCartBtn.Clicked += GoCartBtn_Clicked;
        }


        private void ListView_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            var item = e.SelectedItem as mSearchProduct;

            if (item != null)
            {
                Product listitem = (from itm in viewModel.ProductList
                                    where itm.Name == item.bName
                                    select itm)
                        .FirstOrDefault<Product>();

                Navigation.PushAsync(new ProductView(listitem));
            }
            listView.SelectedItem = null;
        }


        private void GoCartBtn_Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new Connector());
        }

        public Product_InterestList_ViewModel viewModel
        {
            get { return BindingContext as Product_InterestList_ViewModel; }
            set { BindingContext = value; }
        }
    }
}