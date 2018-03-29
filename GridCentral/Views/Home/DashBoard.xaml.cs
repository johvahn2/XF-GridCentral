using GridCentral.Helpers;
using GridCentral.Models;
using GridCentral.Services;
using GridCentral.ViewModels;
using GridCentral.Views.Deals;
using GridCentral.Views.Home.Template;
using GridCentral.Views.Navigation.Settings;
using GridCentral.Views.ObjectViews;
using Plugin.Connectivity;
using Plugin.Settings;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace GridCentral.Views.Navigation.Home
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class DashBoard : ContentPage
    {
        int ad1 = 0; int ad2 = 0; int ad3 = 0;
        public DashBoard()
        {
            viewModel = new Main_DashBoard_ViewModel(new PageService(Navigation));
            InitializeComponent();
            SetStrings();

            RetryBtn.Clicked += RetryBtn_Clicked;

            NavigationPage.SetHasNavigationBar(this, false);
            if (AccountService.Instance.Current_Account != null)
                GetList();
            else
                GetList(true);
        }

        private void SetStrings()
        {
            NoConnectinlbl.Text = Strings.No_Connection_Found;
            RetryBtn.Text = Strings.Retry;
            lbl1.Text = Strings.Item_Sold_By;
            lbl2.Text = Strings.Other_Users;
            ExploreBtn.Text = Strings.Explore;
        }

        private void RetryBtn_Clicked(object sender, EventArgs e)
        {
            if (CrossConnectivity.Current.IsConnected)
            {
                if (AccountService.Instance.Current_Account != null)
                    GetList();
                else
                    GetList(true);

            }
            else
            {
                DialogService.ShowErrorToast("No Connection");
            }
        }

        async void GetList(bool random =false)
        {
            
            if (random)
            {
                Interestlbl.IsVisible = false;
                PopulateInetrestLists(await viewModel.GetRandomProducts());
            }
            else
            {
                if(AccountService.Instance.Current_Account.Interests == null)
                {
                    Interestlbl.IsVisible = false;
                    PopulateInetrestLists(await viewModel.GetRandomProducts());
                }
                else if(AccountService.Instance.Current_Account.Interests.Count < 1)
                {
                    Interestlbl.IsVisible = false;
                    PopulateInetrestLists(await viewModel.GetRandomProducts());
                }
                else
                {
                    PopulateInetrestLists(await viewModel.GetInterestProductsAsync());
                }
            }
            PopulateDealList(await viewModel.GetDeals());
            PopulateNewList(await viewModel.GetRecentroducts());
            getAds1();getAds2(); getAds3();
        }

        //public void PopulateInetrestLists(ObservableCollection<Product> productsList)
        //{
        //    if (productsList == null || productsList.Count < 1) return;

        //    var lastHeight = "100";
        //    var y = 0;
        //    var column = LeftColumn;
        //    var productTapGestureRecognizer = new TapGestureRecognizer();
        //    productTapGestureRecognizer.Tapped += OnProductTapped;

        //    for (var i = 0; i < productsList.Count; i++)
        //    {
        //        var item = new ProductGridItemTemplate();

        //        if (i > 0)
        //        {

        //            if (i == 3 || i == 4 || i == 7 || i == 8 || i == 11 || i == 12)
        //            {

        //                lastHeight = "100";
        //            }
        //            else
        //            {
        //                lastHeight = "190";
        //            }

        //            if (i % 2 == 0)
        //            {
        //                column = LeftColumn;
        //                y++;
        //            }
        //            else
        //            {
        //                column = RightColumn;
        //            }
        //        }

        //        productsList[i].ThumbnailHeight = lastHeight;
        //        item.BindingContext = productsList[i];
        //        item.GestureRecognizers.Add(productTapGestureRecognizer);
        //        column.Children.Add(item);
        //    }
        //}Uneven rows

        private void PopulateInetrestLists(ObservableCollection<Product> productsList)
        {
            var lastHeight = "100";
            var y = 0;
            var column = LeftColumn;
            var productTapGestureRecognizer = new TapGestureRecognizer();
            productTapGestureRecognizer.Tapped += OnProductTapped;

            for (var i = 0; i < productsList.Count; i++)
            {
                var item = new ProductGridItemTemplate();

                if (i % 2 == 0)
                {
                    column = LeftColumn;
                    y++;
                }
                else
                {

                    column = RightColumn;
                }

                if( i == 0)
                {
                    item.AutomationId = "Interest_Product";
                }

                productsList[i].ThumbnailHeight = lastHeight;
                item.BindingContext = productsList[i];
                item.GestureRecognizers.Add(productTapGestureRecognizer);
                column.Children.Add(item);
            }
        }

        public void PopulateDealList(ObservableCollection<Product> productlist)
        {
            if (productlist == null || productlist.Count < 1) return;

            var productTapGestureRecognizer = new TapGestureRecognizer();
            productTapGestureRecognizer.Tapped += OnDealTapped;
            var row = DealList;
            var row2 = DealList2; 

            #region Row 1
            for (var i = 0; i < productlist.Count; i++)
            {
                if (i >= 3)
                    break;

                var item = new DealListTemplate();

                item.BindingContext = productlist[i];
                item.GestureRecognizers.Add(productTapGestureRecognizer);
                row.Children.Add(item);

            }
            #endregion
       
            #region Row 2
            for (var i = 3; i < productlist.Count; i++)
            {
                if (i >= 6)
                    break;

                var item = new DealListTemplate();

                item.BindingContext = productlist[i];
                item.GestureRecognizers.Add(productTapGestureRecognizer);
                row2.Children.Add(item);

            }
            #endregion
        }

        public void PopulateNewList(ObservableCollection<Product> newlist)
        {
            if (newlist == null || newlist.Count < 1) return;

            var productTapGestureRecognizer = new TapGestureRecognizer();
            productTapGestureRecognizer.Tapped += OnRecentTapped;
            var column = NewList;

            for (var i = 0; i < newlist.Count; i++)
            {
                var item = new NewListTemplate();

                item.BindingContext = newlist[i];
                item.GestureRecognizers.Add(productTapGestureRecognizer);
                column.Children.Add(item);

            }
        }

        private async void OnRecentTapped(object sender, EventArgs e)
        {
            var selectedItem = (Product)((NewListTemplate)sender).BindingContext;
            await Navigation.PushAsync(new ProductView(selectedItem));
        }

        private async void OnDealTapped(object sender, EventArgs e)
        {
            var selectedItem = (Product)((DealListTemplate)sender).BindingContext;
            await Navigation.PushAsync(new ProductView(selectedItem));
        }


        private async void OnProductTapped(Object sender, EventArgs e)
        {

            var selectedItem = (Product)((ProductGridItemTemplate)sender).BindingContext;

            await Navigation.PushAsync(new ProductView(selectedItem));
        }



        public Main_DashBoard_ViewModel viewModel
        {
            get { return BindingContext as Main_DashBoard_ViewModel; }
            set { BindingContext = value; }
        }


        protected override void OnAppearing()
        {
            base.OnAppearing();
            if (!CrossSettings.Current.GetValueOrDefault<bool>("AlertView"))
            {
                CheckLog();
                CrossSettings.Current.AddOrUpdateValue("AlertView", true);

            }

            lbl1.FontSize = Device.GetNamedSize(NamedSize.Small, typeof(Label));
            lbl2.FontSize = Device.GetNamedSize(NamedSize.Small, typeof(Label));
        }


        private void Deal_ViewMore(object sender, EventArgs e)
        {
            Navigation.PushAsync(new Views.Deals.Deals());
        }

        private async void CheckLog()
        {
            if (AccountService.Instance.Current_Account == null)
            {
                await Task.Delay(2000);
                var toLog = await DisplayAlert("Sign In", "Please Sign In", "Sign In", "Not Now");

                if (toLog)
                {
                    await Navigation.PushModalAsync(new Login());
                }

            }
        }

        private async Task Recent_ViewMore(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new RecentProductList());
        }

        private async void Interest_ViewMore(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new RecentProductList());

        }

        protected override bool OnBackButtonPressed()
        {
            return true;
        }

     

        #region Get Ads

        private async void getAds1()
        {
            try
            {
                var result = await AdService.Instance.FetchAds("dashboard-Header");
                if (result == null) return;
                ad1 = result.Count;
                ObservableCollection<mCarouselImage> car = new ObservableCollection<mCarouselImage>();
                for (var i = 0; i < result.Count; i++)
                {
                    if (result[i].Show == "true")
                    {
                        car.Add(new mCarouselImage() { Image = result[i].Image, Description = result[i].Description });
                    }
                    else
                    {
                        ad1 --;
                    }

                }
                CarouselImages1.ItemsSource = car;
                changeposit1();

            }
            catch (Exception ex)
            {
                Debug.WriteLine(Keys.TAG + ex);
            }
        }
        private async void getAds2()
        {
            try
            {
                var result = await AdService.Instance.FetchAds("dashboard-Feature");
                if (result == null) return;
                ad2 = result.Count;
                ObservableCollection<mCarouselImage> car = new ObservableCollection<mCarouselImage>();
                for (var i = 0; i < result.Count; i++)
                {
                    if (result[i].Show == "true")
                    {
                        car.Add(new mCarouselImage() { Image = result[i].Image, Description = result[i].Description });
                    }
                    else
                    {
                        ad2--;
                    }

                }
                CarouselImages2.ItemsSource = car;
                changeposit2();

            }
            catch (Exception ex)
            {
                Debug.WriteLine(Keys.TAG + ex);
            }
        }

        private async void getAds3()
        {
            try
            {
                var result = await AdService.Instance.FetchAds("dashboard-Footer");
                if (result == null) return;
                ad3 = result.Count;
                ObservableCollection<mCarouselImage> car = new ObservableCollection<mCarouselImage>();
                for (var i = 0; i < result.Count; i++)
                {
                    if (result[i].Show == "true")
                    {
                        car.Add(new mCarouselImage() { Image = result[i].Image, Description = result[i].Description });
                    }
                    else
                    {
                        ad2--;
                    }

                }
                CarouselImages3.ItemsSource = car;
                changeposit3();

            }
            catch (Exception ex)
            {
                Debug.WriteLine(Keys.TAG + ex);
            }
        }

        async void changeposit2()
        {
            if (ad2 < 2) return;

            while (1 > 0)
            {
                for (var i = 0; i < ad2; i++)
                {
                    CarouselImages2.Position = i;
                    await Task.Delay(2500);
                }
            }
        }
        async void changeposit1()
        {
            if (ad1 < 2) return;

            while (1 > 0)
            {
                for (var i = 0; i < ad1; i++)
                {
                    CarouselImages1.Position = i;
                    await Task.Delay(2500);
                }
            }
        }

        async void changeposit3()
        {
            if (ad3 < 2) return;

            while (1 > 0)
            {
                for (var i = 0; i < ad3; i++)
                {
                    CarouselImages3.Position = i;
                    await Task.Delay(2500);
                }
            }
        }
        #endregion

    }
}
