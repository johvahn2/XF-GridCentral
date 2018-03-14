using GridCentral.Helpers;
using GridCentral.Models;
using GridCentral.Services;
using GridCentral.ViewModels;
using GridCentral.Views.Navigation;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace GridCentral.Views.Profile
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MyProfile : ContentPage
    {

        public MyProfile()
        {
            if (AccountService.Instance.Current_Account == null)
            {
                AccountService.Instance.autho(new RootPage());
                return;
            }

            // viewModel = new Profile_MyProfile_ViewModel();
            viewModel = new Profile_MyProfile_ViewModel();
            InitializeComponent();

            //if (AccountService.Instance.Current_Account.IsAvailable)
            //{
            //    togglerName.Text = "Available";

            //    toggler.IsToggled = true;
            //}
            //else
            //{
            //    togglerName.Text = "Not Available";

            //    toggler.IsToggled = false;
            //}
            getAds1();

        }

        private async void GoToMyItems(object sender, EventArgs e)
        {
             await Navigation.PushAsync(new MyItems());
        }

        public Profile_MyProfile_ViewModel viewModel
        {
            get { return BindingContext as Profile_MyProfile_ViewModel; }
            set { BindingContext = value; }
        }

        #region Get Ads
        int ad1 = 0;
        private async void getAds1()
        {
            try
            {
                var result = await AdService.Instance.FetchAds("profile-Footer");
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

        //private void Switch_Toggled(object sender, ToggledEventArgs e)
        //{
        //    try
        //    {
        //        if (AccountService.Instance.Current_Account.IsAvailable == toggler.IsToggled) return;

        //        if (toggler.IsToggled)
        //        {
        //            togglerName.Text = "Available";
        //            Profile_MyProfile_ViewModel.Instance.toggler = true;
        //        }
        //        else
        //        {

        //            togglerName.Text = "Not Available";
        //            Profile_MyProfile_ViewModel.Instance.toggler = false;
        //        }

        //        AccountService.Instance.Current_Account.IsAvailable = toggler.IsToggled;

        //        AccountService.Instance.UpdateStatus(AccountService.Instance.Current_Account);

        //    }
        //    catch (Exception ex)
        //    {
        //        DialogService.ShowError(Strings.SomethingWrong);
        //        Debug.WriteLine(Keys.TAG + ex);
        //    }
        //}
    }
}
