using GridCentral.Helpers;
using GridCentral.Interfaces;
using GridCentral.Models;
using GridCentral.Services;
using GridCentral.Views.Order;
using Microsoft.AppCenter.Crashes;
using Plugin.Connectivity;
using Plugin.Settings;
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
    public class Order_ModifyAddress_ViewModel : Base_ViewModel
    {
        #region Bind Property
        ObservableCollection<mOrderAddress> _addresses = new ObservableCollection<mOrderAddress>();
        bool _noItems = false;

        public bool noItems
        {
            get { return _noItems; }
            set
            {
                _noItems = value;
                OnPropertyChanged("noItems");
            }
        }

        public ObservableCollection<mOrderAddress> RevAddresses
        {
            get { return new ObservableCollection<mOrderAddress>(Addresses.Reverse()); }
        }

        public ObservableCollection<mOrderAddress> Addresses
        {
            get { return _addresses; }
            set
            {
                _addresses = value;
                OnPropertyChanged("Addresses");
                OnPropertyChanged("RevAddresses");

            }
        }

        public ICommand AddCommand { get; private set; }

        #endregion

        IPageService _pageService;
        ObservableCollection<mCart> _CartList = new ObservableCollection<mCart>();
        public Order_ModifyAddress_ViewModel(IPageService pageService, ObservableCollection<mCart> CartList)
        {
            _CartList = CartList;
            _pageService = pageService;
            AddCommand = new Command(() => { _pageService.PushAsync(new AddAddress("new")); });
        }
        public async void GetAddresses()
        {
            if (IsBusy) return;

            IsBusy = true; noItems = false;

            try
            {
                ObservableCollection<mOrderAddress> result = null;
                if (CrossConnectivity.Current.IsConnected)
                {
                    result = await AddressService.Instance.FetchAddresses(AccountService.Instance.Current_Account.Email,100,0);
                    if (result == null)
                    {
                        noItems = true;
                        Addresses = new ObservableCollection<mOrderAddress>();
                        return;
                    }
                    OfflineService.Write<ObservableCollection<mOrderAddress>>(result, Strings.MyAddress_Offline_fileName, null);
                }
                else
                {
                    result = await OfflineService.Read<ObservableCollection<mOrderAddress>>(Strings.MyAddress_Offline_fileName, null);
                }



                if (result == null)
                {
                    noItems = true;
                    Addresses = new ObservableCollection<mOrderAddress>();
                    return;
                }
                Addresses = await BindClickables(result);
            }
            catch (Exception ex)
            {

                Debug.WriteLine(Keys.TAG + ex);
                DialogService.ShowError(Strings.SomethingWrong);
                Crashes.TrackError(ex);
            }
            finally { IsBusy = false;}
        }

        private async Task<ObservableCollection<mOrderAddress>> BindClickables(ObservableCollection<mOrderAddress> result)
        {
            for (var i = 0; i < result.Count; i++)
            {
                result[i].RemoveCommand = new Command(async itemName => await RemoveAction(itemName));
                result[i].UseCommand = new Command(async itemName => await UseAction(itemName));
                result[i].ModifyCommand = new Command(async itemName => await ModifyAction(itemName));
            }
            return result;
        }

        private async Task ModifyAction(object itemName)
        {
            DialogService.ShowLoading("Modfiy");

            mOrderAddress listitem = (from itm in Addresses
                              where itm.Address2 == itemName.ToString()
                              select itm)
                                    .FirstOrDefault<mOrderAddress>();

            await _pageService.PushAsync(new AddAddress("modify", listitem));
            DialogService.HideLoading();
        }

        private async Task UseAction(object itemName)
        {
            DialogService.ShowLoading("Proceeding");

            mOrderAddress listitem = (from itm in Addresses
                                      where itm.Address2 == itemName.ToString()
                                      select itm)
                                    .FirstOrDefault<mOrderAddress>();

            if(_CartList != null)
            {
                await _pageService.PushAsync(new ConfirmOrder(listitem,_CartList));
            }
            else
            {
                var AddressContent = Newtonsoft.Json.JsonConvert.SerializeObject(listitem);
                CrossSettings.Current.AddOrUpdateValue<string>("ModAddress", AddressContent);
                await _pageService.PopAsync();
            }
            DialogService.HideLoading();

        }

        private async Task RemoveAction(object itemName)
        {
            DialogService.ShowLoading("Removing Address");

            mOrderAddress listitem = (from itm in Addresses
                                      where itm.Address2 == itemName.ToString()
                                      select itm)
                                    .FirstOrDefault<mOrderAddress>();

            try { 
                var result = await AddressService.Instance.DeleteAddress(listitem._id, AccountService.Instance.Current_Account.Email);

                DialogService.HideLoading();

                if (result == "true")
                {
                    GetAddresses();
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
