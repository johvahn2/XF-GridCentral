using GridCentral.Helpers;
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

namespace GridCentral.Views.Order.Card
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AddCard : ContentPage
    {
        public AddCard(ObservableCollection<mCart> CartList)
        {
            viewModel = new Order_AddCard_ViewModel(new PageService(Navigation), CartList);
            InitializeComponent();
            Categories.SelectedItem = Keys.Countries[8];
        }



        public Order_AddCard_ViewModel viewModel
        {
            get { return BindingContext as Order_AddCard_ViewModel; }
            set { BindingContext = value; }
        }
    }
}