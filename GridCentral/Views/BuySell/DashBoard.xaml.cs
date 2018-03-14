using GridCentral.Helpers;
using GridCentral.Models;
using GridCentral.Services;
using GridCentral.ViewModels;
using GridCentral.Views.Home.Template;
using GridCentral.Views.ObjectViews;
using Plugin.Connectivity;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace GridCentral.Views.BuySell
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class DashBoard : ContentPage
    {

        int Curr_Amount = 0;
        public DashBoard()
        {
            viewModel = new BuySell_DashBoard_ViewModel(new PageService(Navigation));
            InitializeComponent();
            SetStrings();
            RetryBtn.Clicked += RetryBtn_Clicked;
            LoadMoreBtn.Clicked += LoadMoreBtn_Clicked;

            NavigationPage.SetHasNavigationBar(this, false);
            GetList();

        }

        private void SetStrings()
        {
            Connectionlbl.Text = Strings.No_Connection_Found;
            RetryBtn.Text = Strings.Retry;
            lbl1.Text = Strings.Item_Sold_By;
            lbl2.Text = Strings.Grid_Central;
            ExploreBtn.Text = Strings.Explore;
        }

        private void RetryBtn_Clicked(object sender, EventArgs e)
        {
            if (CrossConnectivity.Current.IsConnected)
            {
                GetList();
            }
            else
            {
                DialogService.ShowErrorToast("No Connection");
            }
        }

        public async void GetList()
        {
            PopulateProductsLists(await viewModel.GetRandomProducts(20,0));
            getAds1();
        }


        public async void LoadMoreBtn_Clicked(object sender, EventArgs e)
        {
            PopulateProductsLists(await viewModel.GetRandomProducts(20, Curr_Amount));

        }

        private void PopulateProductsLists(ObservableCollection<mUserItem> productsList)
        {
            Curr_Amount += productsList.Count;
            var lastHeight = "100";
            var y = 0;
            var column = LeftColumn;
            var productTapGestureRecognizer = new TapGestureRecognizer();
            productTapGestureRecognizer.Tapped += OnProductTapped;

            for (var i = 0; i < productsList.Count; i++)
            {
                var item = new ItemGridItemTemplate();

                if (i % 2 == 0)
                {
                    column = LeftColumn;
                    y++;
                }
                else
                {

                    column = RightColumn;
                }

                productsList[i].ThumbnailHeight = lastHeight;
                item.BindingContext = productsList[i];
                item.GestureRecognizers.Add(productTapGestureRecognizer);
                column.Children.Add(item);
            }
        }


        private async void OnProductTapped(Object sender, EventArgs e)
        {
            var selectedItem = (mUserItem)((ItemGridItemTemplate)sender).BindingContext;

            await Navigation.PushAsync(new ItemView(selectedItem));
        }

        public BuySell_DashBoard_ViewModel viewModel
        {
            get { return BindingContext as BuySell_DashBoard_ViewModel; }
            set { BindingContext = value; }
        }

        #region Get Ads
        int ad1 = 0;
        private async void getAds1()
        {
            try
            {
                var result = await AdService.Instance.FetchAds("buysell-Header");
                if (result == null) return;
                ad1 = result.Count;
                ObservableCollection<mCarouselImage> car = new ObservableCollection<mCarouselImage>();
                for (var i = 0; i < result.Count; i++)
                {
                    if(result[i].Show == "true")
                    {
                        car.Add(new mCarouselImage() { Image = result[i].Image, Description = result[i].Description });
                    }
                    else
                    {
                        ad1--;
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
        #endregion

        protected override void OnAppearing()
        {
            base.OnAppearing();


            lbl1.FontSize = Device.GetNamedSize(NamedSize.Small, typeof(Label));
            lbl2.FontSize = Device.GetNamedSize(NamedSize.Small, typeof(Label));
        }
    }
}