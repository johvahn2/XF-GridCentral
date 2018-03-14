using GridCentral.Models;
using GridCentral.Services;
using GridCentral.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace GridCentral.Views.Order
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ModfiyAddress : ContentPage
    {
        public ModfiyAddress(ObservableCollection<mCart> CartList)
        {
            viewModel = new Order_ModifyAddress_ViewModel(new PageService(Navigation),CartList);
            InitializeComponent();
            BindIcons();
        }

        private void BindIcons()
        {
            if (Device.OS == TargetPlatform.iOS)
            {
                addIcon.Icon = "add-20x20.png";

            }
            else
            {
                addIcon.Icon = "ic_add.png";
            }
        }

        public Order_ModifyAddress_ViewModel viewModel
        {
            get { return BindingContext as Order_ModifyAddress_ViewModel; }
            set { BindingContext = value; }
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            viewModel.GetAddresses();
        }

    }
}
