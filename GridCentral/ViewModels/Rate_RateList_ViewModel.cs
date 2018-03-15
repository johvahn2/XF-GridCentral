using GridCentral.Helpers;
using GridCentral.Models;
using GridCentral.Services;
using Microsoft.AppCenter.Crashes;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace GridCentral.ViewModels
{
    public class Rate_RateList_ViewModel : Base_ViewModel
    {

        #region Bind Propery
        bool _noItems;
        ObservableCollection<Product> _RateList = new ObservableCollection<Product>();
        ObservableCollection<string> _finish = new ObservableCollection<string>();
        public ObservableCollection<Product> RateList
        {
            get { return _RateList; }
            set
            {
                _RateList = value;
                OnPropertyChanged("RateList");
            }
        }

        public ObservableCollection<string> Finsih
        {
            get { return _finish; }
            set
            {
                _finish = value;
                OnPropertyChanged("Finsih");
            }
        }

        public bool isDone = false;
        public bool noItems
        {
            get { return _noItems; }
            set { _noItems = value; OnPropertyChanged("noItems"); }
        }

        public ICommand RateCommand { get; private set; }
        #endregion

        public Rate_RateList_ViewModel(mOrder order)
        {
            RateCommand = new Command(() => RateAction());
            FetchItems(order);
        }

        private void RateAction()
        {
            //
        }

        private async void FetchItems(mOrder order)
        {
            if (IsBusy) return;

            IsBusy = true;


            try
            {
                DialogService.ShowLoading("Fetching Items");

                ObservableCollection<Product> products = new ObservableCollection<Product>();

                for (var i = 0; i < order.Items.Count; i++)
                {
                    Product item = await ProductService.Instance.FetchProduct(order.Items[i].ItemId);
                    item.Thumbnail = item.Images[0];
                    products.Add(item);
                }


                RateList = products;
                DialogService.HideLoading();

            }catch(Exception ex)
            {
                Debug.WriteLine(Keys.TAG + ex);
                DialogService.ShowError(Strings.SomethingWrong);
                Crashes.TrackError(ex);
            }
            finally { IsBusy = false; }
        }

        public async void RateItem(int RateNum,string itemId)
        {
            if (IsBusy) return;

            try
            {
                DialogService.ShowLoading("Please wait");
                mRate rate = new mRate()
                {
                    Rate = RateNum.ToString(),
                    By = AccountService.Instance.Current_Account.Email,
                    Itemid = itemId.ToString()
                };

                var result = await ProductService.Instance.RateProduct(rate);
                DialogService.HideLoading();

                if(result == "true")
                {
                    Finsih.Add(itemId);
                    DialogService.ShowToast("Thank you for your feedback");
                    return;
                }

                DialogService.ShowError(result);


            }
            catch(Exception ex)
            {
                Debug.WriteLine(Keys.TAG + ex);
                DialogService.ShowError(Strings.SomethingWrong);
                Crashes.TrackError(ex);
            }
            finally { IsBusy = false; }
        }
    }
    
}
