using GridCentral.Helpers;
using GridCentral.Services;
using GridCentral.ViewModels;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace GridCentral.Views.Navigation
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MainMenu : ContentPage
    {
        private readonly INavigation _navigation;
        public MainMenu(INavigation navigation)
        {
            InitializeComponent();

            _navigation = navigation;

            BindingContext = new SamplesViewModel(navigation);

            if(AccountService.Instance.Current_Account != null)
            {
                //toggler.BindingContext = Profile_MyProfile_ViewModel.Instance;


            }
            //CheckAvailablity();

        }

        //private void CheckAvailablity()
        //{
        //    if(AccountService.Instance.Current_Account == null)
        //    {
        //        StatusLayout.IsVisible = false;
        //        return;
        //    }

        //    if (AccountService.Instance.Current_Account.IsAvailable)
        //    {
        //        togglerName.Text = "Available for Delivery";

        //        toggler.IsToggled = true;
        //    }
        //    else
        //    {
        //        togglerName.Text = "Not Available for Delivery";

        //        toggler.IsToggled = false;
        //    }
        //}

        private void SignInBtn_Clicked(object sender, EventArgs e)
        {
            Navigation.PushModalAsync(new Login());
        }


        public async void OnItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            var sample = sampleListView.SelectedItem as Sample;

            if (sample != null)
            {
                if (sample.PageType == typeof(RootPage))
                {
                    await DisplayAlert("Hey", string.Format("You are already here, on sample {0}.", sample.Name), "OK");
                }
                else
                {
                    await sample.NavigateToSample(_navigation);
                }

                sampleListView.SelectedItem = null;
            }
        }

        private async void OnCloseButtonClicked(object sender, EventArgs args)
        {
            await Navigation.PopAsync();
        }

        public void OnBtnCustomClicked()
        {
            //var uri = "mailto:hello@grialkit.com?subject=I%20want%20a%20custom%20theme%20for%20my%20Grial%20app&body=Please%20leave%20us%20your%20comments.";
            var uri = "http://grialkit.com/getquote";
            Device.OpenUri(new Uri(uri));
        }

        //private void Switch_Toggled(object sender, ToggledEventArgs e)
        //{
        //    try
        //    {
        //        if (toggler.IsToggled)
        //            togglerName.Text = "Available for Delivery";
        //        else
        //            togglerName.Text = "Not Available for Delivery";
        //        if (AccountService.Instance.Current_Account.IsAvailable == toggler.IsToggled) return;

        //        if (toggler.IsToggled)
        //        {

        //            togglerName.Text = "Available for Delivery";
        //            Profile_MyProfile_ViewModel.Instance.toggler = true;
        //        }
        //        else
        //        {

        //            togglerName.Text = "Not Available for Delivery";
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
