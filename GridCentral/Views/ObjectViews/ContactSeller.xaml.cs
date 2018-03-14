using GridCentral.Helpers;
using GridCentral.Models;
using GridCentral.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace GridCentral.Views.ObjectViews
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ContactSeller : ContentPage
    {
        public ContactSeller(mAccount seller)
        {
            viewModel = new BuySell_ContactSeller_ViewModel(seller);
            InitializeComponent();
            SetStrings();
        }

        private void SetStrings()
        {
            Emaillbl.Text = Strings.Email + ":";
            Namelbl.Text = Strings.FullName + ":";
            Phonelbl.Text = Strings.Phone_Number + ":";
            CallBtn.Text = Strings.Call_Now;
        }

        private void TapGestureRecognizer_Tapped(object sender, EventArgs e)
        {
            Navigation.PopModalAsync();
        }

        public BuySell_ContactSeller_ViewModel viewModel
        {
            get { return BindingContext as BuySell_ContactSeller_ViewModel; }
            set { BindingContext = value; }
        }
    }
}
