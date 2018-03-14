using GridCentral.Helpers;
using GridCentral.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace GridCentral.Services
{
    public class PageService : IPageService
    {
        INavigation _navigation;
        public PageService(INavigation navigation)
        {
            _navigation = navigation;
        }

        public PageService()
        {

        }

        public async Task<bool> DisplayAlert(string title, string message, string ok, string cancel)
        {
            return await Application.Current.MainPage.DisplayAlert(title, message, ok, cancel);
        }
        public async Task PopAsync()
        {
            //await Application.Current.MainPage.Navigation.PopAsync();
            await _navigation.PopAsync();
        }

        public async Task PopModalAsync()
        {

            //await Application.Current.MainPage.Navigation.PopModalAsync();
            await _navigation.PopModalAsync();

        }

        public async Task PushAsync(Page page)
        {
            if (App.Current.MainPage.Navigation.NavigationStack.Count == 0 ||
                App.Current.MainPage.Navigation.NavigationStack.Last().GetType() != page.GetType())
            {

                //await Application.Current.MainPage.Navigation.PushAsync(page);
                await _navigation.PushAsync(page);

            }


        }

        public async Task PushModalAsync(Page page)
        {
            if (App.Current.MainPage.Navigation.ModalStack.Count == 0 ||
                App.Current.MainPage.Navigation.ModalStack.Last().GetType() != page.GetType())
            {
                //await Application.Current.MainPage.Navigation.PushModalAsync(page);
                await _navigation.PushModalAsync(page);

            }
        }

        public void ShowMain(Page page)
        {
           Application.Current.MainPage = page;
        }
    }
}
