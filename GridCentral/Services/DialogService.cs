using Acr.UserDialogs;
using GridCentral.Helpers;
using PhoneCall.Forms.Plugin.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace GridCentral.Services
{
    public class DialogService
    {
        public static void ShowLoading()
        {
            UserDialogs.Instance.ShowLoading();
        }

        public static void ShowLoading(string loadingMessage)
        {
            UserDialogs.Instance.ShowLoading(loadingMessage);
        }

        public static void HideLoading()
        {
            UserDialogs.Instance.HideLoading();
        }

        public static void ShowError(string errorMessage)
        {
            UserDialogs.Instance.ShowError(errorMessage);
        }

        public static void ShowSuccess(string successMessage)
        {
            UserDialogs.Instance.ShowSuccess(successMessage);
        }

        public static void ShowSuccess(string successMessage, int timeOut)
        {
            UserDialogs.Instance.ShowSuccess(successMessage, timeOut);


        }

        private static void xShowToast(string title)
        {
            ToastConfig t_config = new ToastConfig(ToastEvent.Info, title);

            t_config.SetDuration(3000);


            UserDialogs.Instance.Toast(t_config);
        }

        private static void iShowToast(string msg)
        {
            iToast.iMessage(msg);
        }

        public static void ShowToast(string title)
        {
            if (Device.OS == TargetPlatform.Android)
            {
                xShowToast(title);

            }else if(Device.OS == TargetPlatform.iOS)
            {
                iShowToast(title);
            }
            else
            {
                return;
            }

        }

        public static void ShowErrorToast(string title)
        {
            //UserDialogs.Instance.ErrorToast(title);

            if (Device.OS == TargetPlatform.Android)
            {
                UserDialogs.Instance.ErrorToast(title);

            }
            else if (Device.OS == TargetPlatform.iOS)
            {
                iShowToast(title);
            }
            else
            {
                return;
            }
        }

        public static async Task<string> ShowInputPrompt(string Ok, string Cancel, string Title, string subTitle, string inputText, InputType type)
        {
            PromptConfig t_config = new PromptConfig();
            t_config.SetCancelText(Cancel);
            t_config.SetOkText(Ok);
            t_config.SetTitle(Title);
            t_config.SetInputMode(type);
            t_config.SetText(inputText);
            t_config.SetMessage(subTitle);

            PromptResult tm = await UserDialogs.Instance.PromptAsync(t_config);

            if (tm.Ok)
            {
                return tm.Text;
            }

            if(tm.Text.Length > 0)   // Work around for IOS
            {
                return tm.Text;
            }

            return null;
        }


        public static async Task<bool> DisplayAlert(string Ok, string Cancel, string Title, string subTitle)
        {
            return await Application.Current.MainPage.DisplayAlert(Title, subTitle, Ok, Cancel);
        }

        public static async Task<string> DisplayActionSheet(string title,string cancel,string destruction, params string[] buttons)
        {
            return await Application.Current.MainPage.DisplayActionSheet(title, cancel, destruction, buttons);
        }

        public static void QCall(string number)
        {

            if (Device.OS == TargetPlatform.Android)
            {
                var notificator = DependencyService.Get<IPhoneCall>();
                notificator.MakeQuickCall(number);

            }
            else if (Device.OS == TargetPlatform.iOS)
            {
                Dial.Iphone_Dial(number);
            }
            else
            {
                return;
            }
        }

    }
}
