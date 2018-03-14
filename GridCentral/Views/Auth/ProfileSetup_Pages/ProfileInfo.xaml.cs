using GridCentral.Helpers;
using GridCentral.Services;
using GridCentral.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace GridCentral.Views.Auth.ProfileSetup_Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ProfileInfo : ContentPage
    {
        public ProfileInfo()
        {
            viewModel = new Profile_ProfileRegister_ViewModel(new PageService(Navigation));
            InitializeComponent();
            NavigationPage.SetHasNavigationBar(this, false);

            SetStrings();

        }

        private void SetStrings()
        {
            PrimaryActionButton.Text = Strings.Get_Started;
            title.Text = Strings.You_All_Set;
            FeelFreelbl.Text = Strings.Feel_Free_To;
            lbl1.Text = Strings.Post_New_Used_Item_To_Personal;
            lbl2.Text = Strings.Order_New_Item_On_GridShop;
            lbl3.Text = Strings.Have_Order_Delivered_To_You;
        }

        public Profile_ProfileRegister_ViewModel viewModel
        {
            get { return BindingContext as Profile_ProfileRegister_ViewModel; }
            set { BindingContext = value; }
        }
    }
}