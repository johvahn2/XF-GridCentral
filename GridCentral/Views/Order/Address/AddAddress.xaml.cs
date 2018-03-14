using GridCentral.Models;
using GridCentral.Services;
using GridCentral.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace GridCentral.Views.Order
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AddAddress : ContentPage
    {
        public AddAddress(string type, mOrderAddress item = null)
        {
            viewModel = new Order_AddAddress_ViewModel(new PageService(Navigation), type, item);
            InitializeComponent();
        }

        public Order_AddAddress_ViewModel viewModel
        {
            get { return BindingContext as Order_AddAddress_ViewModel; }
            set { BindingContext = value; }
        }
    }
}
