using GridCentral.Helpers;
using GridCentral.Models;
using GridCentral.Services;
using GridCentral.ViewModels;
using GridCentral.Views.Navigation;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace GridCentral.Views.Profile
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MyItems : ContentPage
    {

        private const uint AnimationDurantion = 250;

        private TapGestureRecognizer tapGestureRecognizer = new TapGestureRecognizer();

        public MyItems()
        {

            if (AccountService.Instance.Current_Account == null)
            {
                AccountService.Instance.autho(new RootPage());
                return;
            }

            viewModel = new Profile_MyItems_ViewModel(new PageService(Navigation));
            InitializeComponent();
            BindIcons();
            Random rnd = new Random();
            int tompo = rnd.Next(0, 3);

            if(tompo == 1)
            {
                EcommerceProductGridBanner.IsVisible = true;
            }
            else
            {
                EcommerceProductGridBanner.IsVisible = false;
            }

            //listView.ItemAppearing += (sender, e) =>
            //{
            //    if (viewModel.IsBusy || viewModel.MyItemList.Count < 4 || viewModel.isDone) return;

            //    if (e.Item == viewModel.MyItemList[viewModel.MyItemList.Count - 1])
            //    {
            //        viewModel.GetMyItems(viewModel.MyItemList.Count, 10, true);

            //    }
            //};

            listView.ItemSelected += ListView_ItemSelectedAsync;
        }

        private async void ListView_ItemSelectedAsync(object sender, SelectedItemChangedEventArgs e)
        {
            var item = e.SelectedItem as mUserItem;

            if (item != null)
            {
                await Navigation.PushAsync(new EditItem(item));

                listView.SelectedItem = null;
            }
        }

        public Profile_MyItems_ViewModel viewModel
        {
            get { return BindingContext as Profile_MyItems_ViewModel; }
            set { BindingContext = value; }
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            if (AccountService.Instance.Current_Account == null) return;
            viewModel.GetMyItems(1000,0);
            //viewModel.IsListRefereshing = true;

            tapGestureRecognizer.Tapped += OnBannerTapped;
            EcommerceProductGridBanner.GestureRecognizers.Add(tapGestureRecognizer);
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            if (AccountService.Instance.Current_Account == null)return;

            tapGestureRecognizer.Tapped -= OnBannerTapped;
            EcommerceProductGridBanner.GestureRecognizers.Remove(tapGestureRecognizer);
        }

        private async void OnBannerTapped(Object sender, EventArgs e)
        {
            var visualElement = (VisualElement)sender;

            await Task.WhenAll(
                visualElement.FadeTo(0, AnimationDurantion, Easing.CubicIn),
                visualElement.ScaleTo(0, AnimationDurantion, Easing.CubicInOut)
            );

            visualElement.HeightRequest = 0;
        }

        private void BindIcons()
        {
            if (Device.OS == TargetPlatform.iOS)
            {
                searchIcon.Icon = "search-20x20.png";
                addIcon.Icon = "add-20x20.png";
                
            }
            else
            {
                searchIcon.Icon = "ic_search.png";
                addIcon.Icon = "ic_add.png";
            }
        }

    }
}
