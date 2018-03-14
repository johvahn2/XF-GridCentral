using GridCentral.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace GridCentral.Views.Navigation.Settings
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AccountSettings : ContentPage
    {
        public AccountSettings()
        {
            viewModel = new Settings_AccountSettings_ViewModel();
            InitializeComponent();
        }

        async Task OnImageTap(object sender, EventArgs args)
        {
            byte[] ImgArr = await viewModel.GetProfileImage();

            if (ImgArr == null)
            {
                viewModel.bProfileImage = null;
                return;
            }

            if (ImgArr.Length > 0)
            {
                var source = ImageSource.FromStream(() => new MemoryStream(ImgArr));
                Profileimage.Source = source;
            }
            else
            {
                viewModel.bProfileImage = null;
            }
        }

        public Settings_AccountSettings_ViewModel viewModel
        {
            get { return BindingContext as Settings_AccountSettings_ViewModel; }
            set { BindingContext = value; }
        }
    }
}
