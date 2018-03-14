using GridCentral.Helpers;
using GridCentral.Models;
using GridCentral.Services;
using GridCentral.Views.Auth;
using GridCentral.Views.Auth.ProfileSetup_Pages;
using GridCentral.Views.Navigation;
using GridCentral.Views.Notifaction;
using GridCentral.Views.ObjectViews;
using GridCentral.Views.Order;
using GridCentral.Views.Profile;
using Plugin.Connectivity;
using Plugin.Settings;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


using Microsoft.AppCenter;
using Microsoft.AppCenter.Analytics;
using Microsoft.AppCenter.Crashes;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Plugin.DeviceOrientation;
using GridCentral.Views.Order.Card;

namespace GridCentral
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class App : Application
    {

        bool IsBusy;
        public App(mPushNotify note = null)
        {
            InitializeComponent();

            CrossDeviceOrientation.Current.LockOrientation(CrossDeviceOrientation.Current.CurrentOrientation);  //Lock Orientation

            //var ISS = AccountService.Instance.ReadyToSignIn;
            if (note == null || !AccountService.Instance.ReadyToSignIn)
            {
                CrossSettings.Current.AddOrUpdateValue("AlertView", false);
                MainPage = GetMainPage();
                MainPage.SetValue(NavigationPage.BarTextColorProperty, Color.White);

            }
            else
            {
                //NavigateNotfiy(note);
                MainPage = new RootPage(false,note); 
                MainPage.SetValue(NavigationPage.BarTextColorProperty, Color.White);
            }

           // var crashed = HockeyApp.CrashManager.DidCrashInLastSession;
        }

        public static Page GetMainPage()
        {
            return new RootPage(false); //Login(); // 
        }

        protected override void OnStart()
        {
            base.OnStart();

            AppCenter.Start("android=7fa2962d-0c59-4e27-8941-3a2d6fb56b0e;" + "uwp={Your UWP App secret here};" +
                   "ios=d486ba17-73ae-4246-b966-113fe74cb5f5",
                   typeof(Analytics), typeof(Crashes));

            if (!CrossConnectivity.Current.IsConnected)
            {
                DialogService.ShowError("Check Connection");
            }
            //Crashes.GenerateTestCrash();
        }


        private async void NavigateNotfiy(mPushNotify note)
        {
            if (IsBusy) return;

            IsBusy = true;

            DialogService.ShowLoading("Please wait");

            if(note.Type == Keys.NotifyTypes[0])//item
            {


            }else if(note.Type == Keys.NotifyTypes[1])//question
            {
                if(note.Why == Keys.NotifyWhys[2])//answer-question
                {
                    var question = await QuestionService.Instance.FetchQuestion(note.Objecter);
                    DialogService.HideLoading();

                    var response = await DialogService.DisplayAlert("View item", "Dismiss", "Q:" + question.Question, "A:" + question.Answer);

                    if (!response) return;

                    DialogService.ShowLoading("Relocating");

                    var item = await ItemService.Instance.FetchItem(question.ProductId);
                    DialogService.HideLoading();

                    await Current.MainPage.Navigation.PushAsync(new ItemView(item));

                }

            }else if(note.Type == Keys.NotifyTypes[2])//order
            {
                if (note.Why == Keys.NotifyWhys[8])//order-update
                {
                    var order = await OrderService.Instance.FetchOrder(note.Objecter);
                    DialogService.HideLoading();

                    DialogService.ShowToast(order.Status);
                    //await Current.MainPage.Navigation.PushAsync(new OrderDetail(order));
                    return;
                }
            }
        }
    }
}
