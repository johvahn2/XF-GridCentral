using GridCentral.Helpers;
using GridCentral.Models;
using GridCentral.Services;
using Microsoft.AppCenter.Crashes;
using Plugin.Connectivity;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace GridCentral.ViewModels
{
    public class Deal_Deals_ViewModel : Base_ViewModel
    {

        #region Bind Property
        bool _noItems;
        ObservableCollection<mSearchProduct> _DealList = new ObservableCollection<mSearchProduct>();
        ObservableCollection<Product> _MyProductList = new ObservableCollection<Product>();

        public ObservableCollection<Product> ProductList
        {
            get { return _MyProductList; }
            set
            {
                _MyProductList = value;
                OnPropertyChanged("MyItemList");
            }
        }

        public bool isDone = false;
        public bool noItems
        {
            get { return _noItems; }
            set { _noItems = value; OnPropertyChanged("noItems"); }
        }

        public ObservableCollection<mSearchProduct> DealList
        {
            get { return _DealList; }
            set { _DealList = value; OnPropertyChanged("DealList"); }
        }
        #endregion

        public Deal_Deals_ViewModel()
        {
            GetDeals(0, 10, false);
        }

        public async void GetDeals(int len, int amount, bool addon)
        {
            if (IsBusy) return;

            IsBusy = true; noItems = false;

            try
            {
                ObservableCollection<Product> result = null;
                if (CrossConnectivity.Current.IsConnected)
                {
                    result = await ProductService.Instance.FetchDeals(amount, null, null,len);
                    if (result == null)
                    {
                        isDone = true;
                        if (DealList.Count < 1)
                            noItems = true;
                        return;
                    }
                    OfflineService.Write<ObservableCollection<Product>>(result, Strings.DealList_Offline_fileName, null);

                }
                else
                {
                    result = await OfflineService.Read<ObservableCollection<Product>>(Strings.DealList_Offline_fileName, null);
                }

                if (result == null)
                {
                    isDone = true;
                    if (DealList.Count < 1)
                        noItems = true;
                    return;
                }
                ProductList = result;
                if (addon)
                    DealList.AddRange(formData(result));
                else
                    DealList = formData(result);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(Keys.TAG + ex);
                DialogService.ShowError(Strings.SomethingWrong);
                Crashes.TrackError(ex);
            }
            finally { IsBusy = false; }
        }

        private ObservableCollection<mSearchProduct> formData(ObservableCollection<Product> result)
        {
            ObservableCollection<mSearchProduct> search = new ObservableCollection<mSearchProduct>();
            for (var i = 0; i < result.Count; i++)
            {
                search.Add(new mSearchProduct
                {
                    Id = result[i].Id,
                    Price = result[i].Price,
                    PRating = result[i].PRating,
                    Status = result[i].Status,
                    Manufacturer = result[i].Manufacturer,
                    Thumbnail = result[i].Images[0],
                    Rating = "0%",
                });

                if (result[i].PRating == null)
                {
                    search[i].Rating = "---";
                }

                int max_description_length = 95;
                int max_Name_Length = 21;

                if (result[i].Description.Length > max_description_length)
                {
                    search[i].Description = result[i].Description.Substring(0, max_description_length) + "...";
                }
                else
                {
                    search[i].Description = result[i].Description;
                }

                search[i].bName = result[i].Name;
              
                if(result[i].Name.Length > max_Name_Length)
                {
                    search[i].Name = result[i].Name.Substring(0, max_Name_Length) + "...";
                }
                else
                {
                    search[i].Name = result[i].Name;
                }

                if (result[i].Status == "In Stock")
                {
                    search[i].StatusColor = "Green";
                }
                else
                {
                    search[i].StatusColor = "Red";
                }
                search[i].SaveCommand = new Command(async itemName => await SaveAction(itemName));
            }
            return search;
        }

        private async Task SaveAction(object itemName)
        {
            try
            {
                if (AccountService.Instance.Current_Account == null)
                {
                    AccountService.Instance.autho(null, Strings.Dismiss);
                    return;
                }

                DialogService.ShowLoading(Strings.Saving_item);

                mSearchProduct listitem = (from itm in DealList
                                           where itm.Name == itemName.ToString()
                                           select itm)
                                        .FirstOrDefault<mSearchProduct>();

                mSavelater item = new mSavelater()
                {
                    Owner = AccountService.Instance.Current_Account.Email,
                    ProductId = listitem.Id
                };
                var result = await SavelaterService.Instance.Additem(item);
                DialogService.HideLoading();

                if (result == "true")
                {
                    DialogService.ShowToast(Strings.Item_Saved);
                    return;
                }
                else
                {
                    DialogService.ShowError(result);
                }

            }
            catch (Exception ex)
            {
                Debug.WriteLine(Keys.TAG + ex);
                DialogService.ShowError(Strings.SomethingWrong);
                Crashes.TrackError(ex);
            }
        }
    }
}
