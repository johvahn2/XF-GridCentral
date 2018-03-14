using GridCentral.Helpers;
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

namespace GridCentral.Views.Deals
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Deals : ContentPage
    {
        public Deals()
        {
            viewModel = new Deal_Deals_ViewModel();
            InitializeComponent();
            SetStrings();

            GoCartBtn.Clicked += GoCartBtn_Clicked;

            listView.ItemAppearing += (sender, e) =>
            {
                if (viewModel.IsBusy || viewModel.DealList.Count < 4 || viewModel.isDone) return;

                if (e.Item == viewModel.DealList[viewModel.DealList.Count - 1])
                {
                        viewModel.GetDeals(viewModel.DealList.Count, 10, true);

                }
            };

            listView.ItemSelected += ListView_ItemSelected;

        }

        private void SetStrings()
        {
            NoItemlbl.Text = Strings.No_Items_Found;
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

        public Deal_Deals_ViewModel viewModel
        {
            get { return BindingContext as Deal_Deals_ViewModel; }
            set { BindingContext = value; }
        }
    }
}
