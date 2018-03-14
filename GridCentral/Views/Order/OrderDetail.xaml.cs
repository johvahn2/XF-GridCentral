using GridCentral.Helpers;
using GridCentral.Models;
using GridCentral.Services;
using GridCentral.ViewModels;
using GridCentral.Views.Contact;
using GridCentral.Views.Order.Template;
using Plugin.Settings;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace GridCentral.Views.Order
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class OrderDetail : ContentPage
    {
        mOrder _order = new mOrder();
        public OrderDetail(mOrder order)
        {
            _order = order;
            viewModel = new Order_OrderDetails_ViewModel(order, new PageService(Navigation));
            InitializeComponent();
            PopulateItemList(order.Items);
        }

        private void SeeMore_Tapped(object sender, EventArgs e)
        {
            //NOT IN USE
        }

        private async void CancelOrder_Tapped(object sender, EventArgs e)
        {
            if(_order.Status == Keys.OrderStatus[6])//Canceled
            {
                DialogService.ShowToast("Item Canceled Already");
                return;
            }
            var result = await DisplayAlert("Cancel Order", "Are You Sure?", "Yes", "No");
            if (!result) return;

            if (_order.Status == Keys.OrderStatus[4])//Transit
            {
               var toContact = await DialogService.DisplayAlert("Contact","Nevermind","Contact to Cancel","Item is Already in Transit, Please Contact us ASAP if you would like to cancel");
               if (!toContact) return;

                await Navigation.PushAsync(new ContactUs());
                return;
            }

            if(_order.Status == Keys.OrderStatus[5])//Delivered
            {
                DialogService.ShowToast("Item Already Delivered");
                return;
            }

            viewModel.Cancel_Order();
        }

        private void ChgAddress_Tapped(object sender, EventArgs e)
        {
            Navigation.PushAsync(new ModfiyAddress(null));
        }

        private void ChgTime_Tapped(object sender, EventArgs e)
        {
            Navigation.PushModalAsync(new ChgDeliveryTime());
        }

        public void PopulateItemList(ObservableCollection<mOrderItems> itemlist)
        {
            if (itemlist == null || itemlist.Count < 1) return;

            var column = ItemList;

            for (var i = 0; i < itemlist.Count; i++)
            {
                //if (i >= 3)
                //    break;

                var item = new NameItemListTemplate();

                item.BindingContext = itemlist[i];
                column.Children.Add(item);

            }
        }

        public Order_OrderDetails_ViewModel viewModel
        {
            get { return BindingContext as Order_OrderDetails_ViewModel; }
            set { BindingContext = value; }
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            if (!String.IsNullOrEmpty(CrossSettings.Current.GetValueOrDefault<string>("FromTime")) ||
                !String.IsNullOrEmpty(CrossSettings.Current.GetValueOrDefault<string>("ToTime")))
            {
                string from = CrossSettings.Current.GetValueOrDefault<string>("FromTime");
                string to = CrossSettings.Current.GetValueOrDefault<string>("ToTime");
                DeliveryTime.Text = from + " - " + to;
                viewModel.isUpdatable = true;

                CrossSettings.Current.Remove("FromTime");CrossSettings.Current.Remove("ToTime");
            }
            else
            {
                DeliveryTime.Text = viewModel.DeliveryTime;
            }

            if (CrossSettings.Current.Contains("ModAddress"))
            {
                string addressContent = CrossSettings.Current.GetValueOrDefault<string>("ModAddress");
                mOrderAddress address = Newtonsoft.Json.JsonConvert.DeserializeObject<mOrderAddress>(addressContent);
                viewModel.PhoneNumber = address.PhoneNumber;
                viewModel.Address1 = address.Address1;
                viewModel.Address2 = address.Address2;

                CrossSettings.Current.Remove("ModAddress");
                viewModel.isUpdatable = true;
            }

            viewModel.DeliveryTime = DeliveryTime.Text;
        }
    }
}
