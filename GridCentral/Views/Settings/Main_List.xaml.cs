using GridCentral.Helpers;
using GridCentral.Services;
using GridCentral.Views.Settings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace GridCentral.Views.Navigation.Settings
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Main_List : ContentPage
    {
        List<string> sets = new List<string>(){
                "Account Settings",
                "My Interests",
                "Term of Use",
                "Version",
                "Logout"
            };


        public Main_List()
        {
            if (AccountService.Instance.Current_Account == null)
            {
                sets[4] = "Login";
            }

            InitializeComponent();

            listView.ItemsSource = sets;

            listView.ItemSelected += ListView_ItemSelected;


        }

        private async void ListView_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            var item = e.SelectedItem as string;

            if (item != null)
            {
                if (item == sets[0])
                {
                    if(AccountService.Instance.Current_Account == null)
                    {
                        AccountService.Instance.autho(null, "Dismiss");
                        return;
                    }
                    await Navigation.PushAsync(new AccountSettings());
                    listView.SelectedItem = null;
                }
                else if (item == sets[1])
                {
                    if (AccountService.Instance.Current_Account == null)
                    {
                        AccountService.Instance.autho(null, "Dismiss");
                        return;
                    }
                    await Navigation.PushAsync(new Edit_Interests());
                    listView.SelectedItem = null;

                }else if(item == sets[2])
                {
                    await Navigation.PushAsync(new TermsOfService());
                    listView.SelectedItem = null;
                }
                else if(item == sets[3])
                {

                    //await _pageService.PushAsync(new FeedBack());
                    if (Device.OS == TargetPlatform.iOS)
                    {
                        DialogService.ShowToast(Strings.IOS_Version);

                    }
                    else
                    {
                        DialogService.ShowToast(Strings.Version);
                    }

                    listView.SelectedItem = null;

                } else if(item == sets[4])
                {
                    if (sets[2] == "Login")
                    {
                        AccountService.Instance.autho(null, "Dismiss");
                    }
                    else
                    {
                        AccountService.Instance.SignOut();

                    }
                }
            }
        }
    }
}
