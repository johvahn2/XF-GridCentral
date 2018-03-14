using GridCentral.Helpers;
using GridCentral.Services;
using GridCentral.ViewModels;
using Plugin.Media;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace GridCentral.Views.Auth.ProfileSetup_Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ProfileImage : ContentPage
    {
        public ProfileImage()
        {
            viewModel = new Profile_ProfileRegister_ViewModel(new PageService(Navigation));
            InitializeComponent();


            NavigationPage.SetHasNavigationBar(this, false);

            SetStrings();
        }

        private void SetStrings()
        {
            PrimaryActionButton.Text = Strings.Next;
            Addlbl.Text = Strings.Add_Displayname_ProfileImage;
            DisplaynameEntry.Placeholder = Strings.Enter_Displayname;
        }

        async Task OnImageTap(object sender, EventArgs args)
        {
            var action = await DisplayActionSheet("Image Options", "Cancel", null, "Selected Picture", "Take Picture");
            byte[] ImgArr = await viewModel.GetProfileImage(action);

            if (ImgArr == null)
            {
                viewModel.bProfileImage = null;
                Profileimage.Source = "profile_placeholder.png";
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

        public Profile_ProfileRegister_ViewModel viewModel
        {
            get { return BindingContext as Profile_ProfileRegister_ViewModel; }
            set { BindingContext = value; }
        }

        protected override bool OnBackButtonPressed()
        {
            return true;
        }

    }
}
