using GridCentral.Helpers;
using GridCentral.Models;
using GridCentral.Services;
using GridCentral.ViewModels;
using GridCentral.Views.Common;
using GridCentral.Views.Navigation.Home;
using GridCentral.Views.Navigation.WalkThroughs;
using GridCentral.Views.Notifaction;
using GridCentral.Views.ObjectViews;
using GridCentral.Views.Order;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace GridCentral.Views.Navigation
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class RootPage : MasterDetailPage
    {

        private bool _showWelcome;
        public RootPage() : this(false)
        {
        }

        public RootPage(bool sayWelcome, mPushNotify note = null,Page page = null)
        {
            InitializeComponent();
            NavigationPage.SetHasNavigationBar(this, false);

            _showWelcome = sayWelcome;

            // Empty pages are initially set to get optimal launch experience
            //Master = new ContentPage { Title = "GridCentral" };
            //Detail = NavigationPageHelper.Create(new ContentPage());

            InitializeMasterDetail(page);

            if(note != null)
            {
                NavigateNotfiy(note);
            }
            
        }

        public async void NavigateNotfiy(mPushNotify note)
        {
            DialogService.ShowLoading("Please wait");

            if (note.Type == Keys.NotifyTypes[2])//order
            {
                if (note.Why == Keys.NotifyWhys[8])//order-update
                {
                    var order = await OrderService.Instance.FetchOrder(note.Objecter);
                    DialogService.HideLoading();

                    Detail = NavigationPageHelper.Create(new OrderDetail(order));
                    return;
                }
            }
            else if (note.Type == Keys.NotifyTypes[1])//question
            {
                if (note.Why == Keys.NotifyWhys[2] || note.Why == Keys.NotifyTypes[1])//answer-question & new-question
                {
                    DialogService.HideLoading();
                    Detail = NavigationPageHelper.Create(new Notify());
                    return;

                }

            }else if(note.Type == Keys.NotifyTypes[0])//item
            {
                if(note.Why == Keys.NotifyWhys[9])//Rate
                {
                    DialogService.HideLoading();
                    Detail = NavigationPageHelper.Create(new Notify());
                }
            }
            DialogService.HideLoading();

        }

        public async void OnSettingsTapped(Object sender, EventArgs e)
        {
           // await Navigation.PushAsync(new SettingsPage());
        }

        //protected async override void OnAppearing()
        //{
        //    base.OnAppearing();

        //    SampleCoordinator.SampleSelected += SampleCoordinator_SampleSelected;

        //    if (_showWelcome)
        //    {
        //        _showWelcome = false;

        //        await Navigation.PushModalAsync(NavigationPageHelper.Create(new WalkthroughVariantPage()));//here

        //        await Task.Delay(500)
        //            .ContinueWith(t => NavigationService.BeginInvokeOnMainThreadAsync(InitializeMasterDetail));
        //    }
        //}

        //protected override void OnDisappearing()
        //{
        //    base.OnDisappearing();

        //    SampleCoordinator.SampleSelected -= SampleCoordinator_SampleSelected;
        //}

        private void InitializeMasterDetail(Page page)
        {
            Master = new MainMenu(new NavigationService(Navigation, LaunchSampleInDetail));
            if(page != null)
            {
                Detail = NavigationPageHelper.Create(page);
                return;
            }
            Detail = NavigationPageHelper.Create(new DashBoard());//here
        }

        private void LaunchSampleInDetail(Page page, bool animated)
        {
            // CustomNavBarPage must be handled differently because XF seems not to be considering the
            // "NavigationPage.SetHasNavigationBar(this, false);" when you add the page as the 
            // root of the NavigationPage, when you are working in Android.
            if (page is CustomNavBar)
            {
                var navigationPage = NavigationPageHelper.Create(new ContentPage());

                Detail = navigationPage;

                navigationPage.PushAsync(page, false);
            }
            else
            {
                Detail = NavigationPageHelper.Create(page);
            }

            IsPresented = false;
        }

        private void SampleCoordinator_SampleSelected(object sender, SampleEventArgs e)
        {
            if (e.Sample.PageType == typeof(RootPage))
            {
                IsPresented = true;
            }
        }
    }
}
