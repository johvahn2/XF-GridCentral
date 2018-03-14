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
    public partial class ProfileInterest : ContentPage
    {        
        public ProfileInterest()
        {
            viewModel = new Profile_ProfileRegister_ViewModel(new PageService(Navigation));
            InitializeComponent();
            NavigationPage.SetHasNavigationBar(this, false);

            SetStrings();

        }

        private void SetStrings()
        {
            title.Text = Strings.Please_Select_Your_Interest;
            box13.CheckedText = Strings.Appliances; box13.UncheckedText = Strings.Appliances;
            box9.CheckedText = Strings.Art; box9.UncheckedText = Strings.Art;
            box10.CheckedText = Strings.Baby; box10.UncheckedText = Strings.Baby;
            box1.CheckedText = Strings.Books; box1.UncheckedText = Strings.Books;
            box2.CheckedText = Strings.Cars; box2.UncheckedText = Strings.Cars;
            box3.CheckedText = Strings.Clothing; box3.UncheckedText = Strings.Clothing;
            box4.CheckedText = Strings.Electronics; box4.UncheckedText = Strings.Electronics;
            box5.CheckedText = Strings.Furniture; box5.UncheckedText = Strings.Furniture;
            box11.CheckedText = Strings.Home_Supplies; box11.UncheckedText = Strings.Home_Supplies;
            box6.CheckedText = Strings.Personal_Care; box6.UncheckedText = Strings.Personal_Care;
            box7.CheckedText = Strings.Makeup_Beauty; box7.UncheckedText = Strings.Makeup_Beauty;
            box12.CheckedText = Strings.Jewelry; box12.UncheckedText = Strings.Jewelry;
            box8.CheckedText = Strings.Toys_Games; box8.UncheckedText = Strings.Toys_Games;
            PrimaryActionButton.Text = Strings.Next;
        }

        public Profile_ProfileRegister_ViewModel viewModel
        {
            get { return BindingContext as Profile_ProfileRegister_ViewModel; }
            set { BindingContext = value; }
        }
    }
}