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

namespace GridCentral.Views.Search
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ProductSearch : ContentPage
    {
        public ProductSearch(string SearchedTxt, string category)
        {
            viewModel = new Product_ProductSearch_ViewModel(SearchedTxt, category);
            InitializeComponent();
            NavigationPage.SetHasNavigationBar(this, false);

            listView.ItemAppearing += (sender, e) =>
            {
                if (viewModel.IsBusy || viewModel.SearchList.Count < 4 || viewModel.isDone) return;

                if (e.Item == viewModel.SearchList[viewModel.SearchList.Count - 1])
                {
                    //viewModel.checker(SearchedTxt, category, 10 ,viewModel.SearchList.Count);
                    if (Product_ProductSearch_ViewModel.isRandom)
                    {
                        if (viewModel.CategoryIndex == 0)
                            viewModel.GetRandomProducts(viewModel.SearchList.Count, 10, true);
                        else
                            viewModel.GetCategoryItems(category, viewModel.SearchList.Count, 10,true);
                    }
                    else
                    {
                        viewModel.GetSearch(SearchedTxt, category, 10, viewModel.SearchList.Count,true);

                    }

                }
            };

            listView.ItemSelected += ListView_ItemSelected;

        }

        private void ListView_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            var item = e.SelectedItem as mSearchProduct;

            if (item != null)
            {
                Product listitem = (from itm in viewModel.ProductList
                                where itm.Id == item.Id
                                select itm)
                        .FirstOrDefault<Product>();

                //if(listitem == null)
                //{
                //    DialogService.ShowErrorToast("Something When Wrong");
                //    return;
                //}
                Navigation.PushAsync(new ProductView(listitem));
            }
            listView.SelectedItem = null;
        }

        public Product_ProductSearch_ViewModel viewModel
        {
            get { return BindingContext as Product_ProductSearch_ViewModel; }
            set { BindingContext = value; }
        }
    }
}
