using GridCentral.Helpers;
using GridCentral.Interfaces;
using GridCentral.Models;
using GridCentral.Services;
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
    public class Cart_Savelater_ViewModel : Base_ViewModel
    {

        #region Bind Property
        ObservableCollection<mSavelaterR> _MySaveList = new ObservableCollection<mSavelaterR>();
        ObservableCollection<Product> _MyProductList = new ObservableCollection<Product>();

        bool _asItems = false;
        bool _IsListRefereshing = false;

        public bool IsListRefereshing
        {
            get { return _IsListRefereshing; }
            set
            {
                _IsListRefereshing = value;

                OnPropertyChanged("IsListRefereshing");
                if (IsListRefereshing)
                {
                    GetSave();
                }
            }
        }

        public ObservableCollection<Product> MyProductList
        {
            get { return _MyProductList; }
            set
            {
                _MyProductList = value;
                OnPropertyChanged("MyItemList");
            }
        }

        public ObservableCollection<mSavelaterR> RevMySaveList
        {
            get { return new ObservableCollection<mSavelaterR>(MySaveList.Reverse()); }
        }

        public ObservableCollection<mSavelaterR> MySaveList
        {
            get { return _MySaveList; }
            set
            {
                _MySaveList = value;
                OnPropertyChanged("MySaveList");
                OnPropertyChanged("RevMySaveList");

            }
        }

        public bool noItems
        {
            get { return _asItems; }
            set { _asItems = value; OnPropertyChanged("noItems"); }
        }

        IPageService _pageService;


        #endregion


        public Cart_Savelater_ViewModel()
        {
            //GetSave();
            IsListRefereshing = true;
        }


        public async void GetSave()
        {
            if (IsBusy) return;

            IsBusy = true;noItems = false;

            try
            {
                ObservableCollection<Product> result = null;
                if (CrossConnectivity.Current.IsConnected)
                {
                    result = await SavelaterService.Instance.Fetchitems(AccountService.Instance.Current_Account.Email);
                    if (result == null)
                    {
                        noItems = true;
                        MySaveList = new ObservableCollection<mSavelaterR>();
                        return;
                    }
                    OfflineService.Write<ObservableCollection<Product>>(result, Strings.MySaveLater_Offline_fileName, null);
                }
                else
                {
                    result = await OfflineService.Read<ObservableCollection<Product>>(Strings.MySaveLater_Offline_fileName, null);
                }

  

                if (result == null)
                {
                    noItems = true;
                    MySaveList = new ObservableCollection<mSavelaterR>();
                    return;
                }
                MyProductList = result;
                MySaveList = await BindClickables(FormatList(result));
            }
            catch (Exception ex)
            {

                Debug.WriteLine(Keys.TAG + ex);
                DialogService.ShowError(Strings.SomethingWrong);
            }
            finally { IsBusy = false; IsListRefereshing = false; }
        }

        private ObservableCollection<mSavelaterR> FormatList(ObservableCollection<Product> result)
        {

            MyProductList = CalculatePercentages(MyProductList);

            ObservableCollection<mSavelaterR> save = new ObservableCollection<mSavelaterR>();
            for (var i = 0; i < result.Count; i++)
            {
                save.Add(new mSavelaterR
                {
                    Id = result[i].Id,
                    Name = result[i].Name,
                    Price = result[i].Price,
                    Status = result[i].Status,
                    Manufacturer = result[i].Manufacturer,
                    Thumbnail = result[i].Images[0]
                });

                int max_name_length = 17;

                save[i].bName = result[i].Name;

                if (result[i].Name.Length > max_name_length)
                {
                    save[i].Name = result[i].Name.Substring(0, max_name_length) + "...";
                }

                if (result[i].Status == "In Stock")
                {
                    save[i].StatusColor = "Green";
                }
                else
                {
                    save[i].StatusColor = "Red";
                }
            }
            return save;
        }

        private async Task<ObservableCollection<mSavelaterR>> BindClickables(ObservableCollection<mSavelaterR> result)
        {
            for (var i = 0; i < result.Count; i++)
            {
                result[i].RemoveCommand = new Command(async itemName => await RemoveActionAsync(itemName));
                result[i].AddToCartCommand = new Command(async itemName => await CartActionAsync(itemName));
            }
            return result;
        }


        private async Task CartActionAsync(object itemName)
        {
            try
            {
                DialogService.ShowLoading("Adding To Cart");
                mSavelaterR listitem = (from itm in MySaveList
                                  where itm.Name == itemName.ToString()
                                  select itm)
                                        .FirstOrDefault<mSavelaterR>();

                mCartS item = new mCartS()
                {
                    Owner = AccountService.Instance.Current_Account.Email,
                    Quantity = "1",
                    ProductId = listitem.Id
                };
                var result = await SavelaterService.Instance.MvtoCart(item);
                DialogService.HideLoading();
                if (result == "true")
                {
                    GetSave();
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
            }
        }

        private async Task RemoveActionAsync(object itemName)
        {
            try
            {
                DialogService.ShowLoading("Removing Item");
                mSavelaterR listitem = (from itm in MySaveList
                                  where itm.Name == itemName.ToString()
                                  select itm)
                                        .FirstOrDefault<mSavelaterR>();

                var result = await SavelaterService.Instance.DeleteItem(listitem.Id, AccountService.Instance.Current_Account.Email);
                DialogService.HideLoading();
                if (result == "true")
                {
                    GetSave();
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
            }
        }
    }
}
