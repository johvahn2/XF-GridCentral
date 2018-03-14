using GridCentral.Models;
using GridCentral.ViewModels;
using GridCentral.Views.ObjectViews;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace GridCentral.Views.Search
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class BuySellSearch : ContentPage
    {
        public BuySellSearch(string SearchedTxt, string category)
        {
            viewModel = new BuySell_BuySellSearch_ViewModel(SearchedTxt, category);
            InitializeComponent();
            NavigationPage.SetHasNavigationBar(this, false);

            listView.ItemAppearing += (sender, e) =>
            {
                if (viewModel.IsBusy || viewModel.SearchList.Count < 4 || viewModel.isDone) return;

                if (e.Item == viewModel.SearchList[viewModel.SearchList.Count - 1])
                {
                    if (Product_ProductSearch_ViewModel.isRandom)
                        viewModel.GetCategoryItems(category, viewModel.SearchList.Count, 10, true);
                    else
                        viewModel.GetSearch(SearchedTxt, category, 10, viewModel.SearchList.Count, true);

                }
            };

            listView.ItemSelected += ListView_ItemSelected;

        }

        private void ListView_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            var item = e.SelectedItem as mSearchProduct;

            if (item != null)
            {
                mUserItem listitem = (from itm in viewModel.ItemList
                                where itm.Name == item.bName
                                select itm)
                     .FirstOrDefault<mUserItem>();

                Navigation.PushAsync(new ItemView(listitem));
            }
            listView.SelectedItem = null;
        }

        public BuySell_BuySellSearch_ViewModel viewModel
        {
            get { return BindingContext as BuySell_BuySellSearch_ViewModel; }
            set { BindingContext = value; }
        }
    }
}
