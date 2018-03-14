using GridCentral.Models;
using GridCentral.Services;
using GridCentral.ViewModels;
using GridCentral.Views.Navigation;
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
    public partial class ConfirmOrder : ContentPage
    {
        ObservableCollection<mCart> _CartList = new ObservableCollection<mCart>();
        public ConfirmOrder(mOrderAddress address,ObservableCollection<mCart> CartList,bool usingCard = false)
        {
            _CartList = CartList;
            viewModel = new Order_ConfirmOrder_ViewModel(new PageService(Navigation), address,CartList,usingCard);
            InitializeComponent();
            NavigationPage.SetHasBackButton(this, false);
            CancelBtn.Clicked += CancelBtn_Clicked;
            PopulateCartList(CartList);
        }

        private void CancelBtn_Clicked(object sender, EventArgs e)
        {
            DialogService.ShowLoading();
            PageService pageService = new PageService();
            pageService.ShowMain(new RootPage());
            DialogService.HideLoading();
        }

        private void ChgAddress_Tapped(object sender, EventArgs e)
        {
            Navigation.PushAsync(new ModfiyAddress(_CartList));
        }

        private void ChgTime_Tapped(object sender, EventArgs e)
        {
            Navigation.PushModalAsync(new ChgDeliveryTime());
        }

        public Order_ConfirmOrder_ViewModel viewModel
        {
            get { return BindingContext as Order_ConfirmOrder_ViewModel; }
            set { BindingContext = value; }
        }

        protected override bool OnBackButtonPressed()
        {
            return true;
        }


        public void PopulateCartList(ObservableCollection<mCart> cartlist)
        {
            if (cartlist == null || cartlist.Count < 1) return;

            var column = ItemList;

            for (var i = 0; i < cartlist.Count; i++)
            {
                //if (i >= 3)
                //    break;

                var item = new ItemListTemplate();

                item.BindingContext = cartlist[i];
                column.Children.Add(item);

            }
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

               // CrossSettings.Current.Remove("FromTime"); CrossSettings.Current.Remove("ToTime");
            }
            else
            {
                DeliveryTime.Text = "11:00:00 - 19:00:00";
            }

            viewModel.DeliveryTime = DeliveryTime.Text;
        }

        private void RtnPolicy_Tapped(object sender, EventArgs e)
        {
            DialogService.ShowToast("To come");
        }
    }
}
