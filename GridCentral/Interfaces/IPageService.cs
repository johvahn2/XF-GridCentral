using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace GridCentral.Interfaces
{
    public interface IPageService
    {
        Task<bool> DisplayAlert(string title, string message, string ok, string cancel);
        Task PushAsync(Page page);

        Task PushModalAsync(Page page);

        Task PopAsync();

        Task PopModalAsync();

        void ShowMain(Page page);

    }
}
