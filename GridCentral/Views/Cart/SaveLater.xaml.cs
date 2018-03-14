using GridCentral.Helpers;
using GridCentral.Models;
using GridCentral.ViewModels;
using GridCentral.Views.ObjectViews;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace GridCentral.Views.Cart
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SaveLater : ContentPage
    {
        public SaveLater()
        {
            viewModel = new Cart_Savelater_ViewModel();
            InitializeComponent();
            SetStrings();

            listView.ItemSelected += ListView_ItemSelected;
        }

        private void SetStrings()
        {
            NoItemlbl.Text = Strings.No_Items_Found;
        }

        private void ListView_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            var item = e.SelectedItem as mSavelaterR;


            if (item != null)
            {
                Product listitem = (from itm in viewModel.MyProductList
                                    where itm.Name == item.bName
                                    select itm)
                        .FirstOrDefault<Product>();

                Navigation.PushAsync(new ProductView(listitem));
            }
            listView.SelectedItem = null;
        }

        public Cart_Savelater_ViewModel viewModel
        {
            get { return BindingContext as Cart_Savelater_ViewModel; }
            set { BindingContext = value; }
        }
    }
}