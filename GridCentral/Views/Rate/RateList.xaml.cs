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

namespace GridCentral.Views.Rate
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class RateList : ContentPage
    {
        string _RateNum = "";
        Product selected_item = null;
        public RateList(mOrder order)
        {
            viewModel = new Rate_RateList_ViewModel(order);
            InitializeComponent();
            InitTaps();
            listView.ItemSelected += ListView_ItemSelected;
            listView.SelectedItem = ((ObservableCollection<Product>)listView.ItemsSource).FirstOrDefault();

        }

        private void ListView_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            var item = e.SelectedItem as Product;

            foreach (var finsih in viewModel.Finsih)
            {
                if(finsih == item.Id)
                {
                    DialogService.ShowToast("Item Already Rated");
                    return;
                }
            }

            selected_item = item;
        }

        private void InitTaps()
        {
            Rone.Clicked += (sender, e) =>
            {
                Rone.BackgroundColor = Color.FromHex("#CB224E");
                GrayOut("Rone");
            };

            Rtwo.Clicked += (sender, e) =>
            {
                Rtwo.BackgroundColor = Color.FromHex("#CB224E");
                GrayOut("Rtwo");
            };


            Rthree.Clicked += (sender, e) =>
            {
                Rthree.BackgroundColor = Color.FromHex("#CB224E");
                GrayOut("Rthree");
            };
            Rfour.Clicked += (sender, e) =>
            {

                Rfour.BackgroundColor = Color.FromHex("#CB224E");
                GrayOut("Rfour");
            };

            Rfive.Clicked += (sender, e) =>
            {
                Rfive.BackgroundColor = Color.FromHex("#CB224E");
                GrayOut("Rfive");
            };

            RateBtn.Clicked += (sender, e) =>
            {
                if(selected_item == null)
                {
                    DialogService.ShowToast("Please Select Product");
                    return;
                }
                if (String.IsNullOrEmpty(_RateNum))
                {
                    DialogService.ShowToast("Please Select Rating Number");
                    return;
                }

                int RateNum = 0;

                switch (_RateNum)
                {
                    case "Rone":
                        RateNum = 1;
                        break;
                    case "Rtwo":
                        RateNum = 2;
                        break;
                    case "Rthree":
                        RateNum = 3;
                        break;
                    case "Rfour":
                        RateNum = 4;
                        break;
                    case "Rfive":
                        RateNum = 5;
                        break;
                    default:
                        RateNum = 0;
                        break;
                }
                viewModel.RateItem(RateNum, selected_item.Id);

            };

        }

        private void GrayOut(string execpt)
        {
            _RateNum = execpt;
            if(execpt != "Rone")
            {
                Rone.BackgroundColor = Color.Gray;
            }
            if (execpt != "Rtwo")
            {
                Rtwo.BackgroundColor = Color.Gray;
            }
            if (execpt != "Rthree")
            {
                Rthree.BackgroundColor = Color.Gray;
            }
            if (execpt != "Rfour")
            {
                Rfour.BackgroundColor = Color.Gray;
            }
            if (execpt != "Rfive")
            {
                Rfive.BackgroundColor = Color.Gray;
            }
        }

        public Rate_RateList_ViewModel viewModel
        {
            get { return BindingContext as Rate_RateList_ViewModel; }
            set { BindingContext = value; }
        }
    }
}
