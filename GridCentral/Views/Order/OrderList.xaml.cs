using GridCentral.Models;
using GridCentral.Services;
using GridCentral.ViewModels;
using GridCentral.Views.Navigation;
using Plugin.Settings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace GridCentral.Views.Order
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class OrderList : ContentPage
    {
        public OrderList()
        {
            if (AccountService.Instance.Current_Account == null)
            {
                AccountService.Instance.autho(new RootPage());
                return;
            }

            viewModel = new Order_OrderList_ViewModel();
            InitializeComponent();

            listView.ItemSelected += ListView_ItemSelected;

        }

        private void ListView_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            var item = e.SelectedItem as mOrder;

            if (item != null)
            {
                Navigation.PushAsync(new OrderDetail(item));
            }
            listView.SelectedItem = null;
        }

        public Order_OrderList_ViewModel viewModel
        {
            get { return BindingContext as Order_OrderList_ViewModel; }
            set { BindingContext = value; }
        }


        protected override void OnAppearing()
        {
            base.OnAppearing();

            if (CrossSettings.Current.GetValueOrDefault<bool>("StatusUpdate"))
            {
                viewModel.GetMyOrders();
                if (AccountService.Instance.Current_Account == null)
                {
                    return;
                }
                CrossSettings.Current.AddOrUpdateValue<bool>("StatusUpdate",false);
            }
        }
    }
}
