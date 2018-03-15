using GridCentral.Helpers;
using GridCentral.Interfaces;
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
    public class Order_AddAddress_ViewModel : Base_ViewModel
    {

        #region Bind Property

        string _address1;
        string _phone;
        string _address2;
        string _BtnText = "Add Address";
        public string Address1
        {
            get { return _address1; }
            set
            {
                _address1 = value;
                OnPropertyChanged("Address1");
            }
        }
        public string Address2
        {
            get { return _address2; }
            set
            {
                _address2 = value;
                OnPropertyChanged("Address2");
            }
        }

        public string Phone
        {
            get { return _phone; }
            set
            {
                _phone = value;
                OnPropertyChanged("Phone");
            }
        }

        public string BtnText
        {
            get { return _BtnText; }
            set
            {
                _BtnText = value;
                OnPropertyChanged("BtnText");
            }
        }

        public ICommand BtnCommand { get; private set; }
        #endregion

        IPageService _pageService;
        string itemId;
        public Order_AddAddress_ViewModel(IPageService pageSerivce,string type = "new", mOrderAddress item = null)
        {
            Phone = AccountService.Instance.Current_Account.PhoneNumber;
            BtnCommand = new Command(() => AddAddress());

            if (type == "modify")
            {
                itemId = item._id;
                BtnText = Strings.Save_Address;
                Address1 = item.Address1;Address2 = item.Address2; Phone = item.PhoneNumber;
                BtnCommand = new Command(() => EditAddress());
            }
            _pageService = pageSerivce;
        }

        private async void EditAddress()
        {
            if (IsBusy) return;

            IsBusy = true;


            DialogService.ShowLoading(Strings.Adding_Address);


            try
            {

                mOrderAddress item = new mOrderAddress()
                {
                    Address1 = Address1,
                    Address2 = Address2,
                    Owner = AccountService.Instance.Current_Account.Email,
                    PhoneNumber = Phone,
                    _id = itemId

                };

                var result = await AddressService.Instance.EditAddress(item);

                DialogService.HideLoading();

                if (result == "true")
                {
                    DialogService.ShowSuccess(Strings.Address_Updated);
                    await _pageService.PopAsync();

                }
                else
                {
                    DialogService.ShowError(result);
                }

            }
            catch (Exception ex)
            {
                DialogService.ShowError(Strings.SomethingWrong);
                Debug.WriteLine(Keys.TAG + ex);
                Crashes.TrackError(ex);
            }
            finally { IsBusy = false; }
        }

        private async void AddAddress()
        {
            if(String.IsNullOrEmpty(Address1) || String.IsNullOrEmpty(Address2) || String.IsNullOrEmpty(Phone))
            {
                DialogService.ShowErrorToast(Strings.Enter_All_Fields);
                return;
            }

            if (IsBusy) return;

            IsBusy = true;


            DialogService.ShowLoading(Strings.Adding_Address);


            try
            {

                mOrderAddress item = new mOrderAddress()
                {
                    Address1 = Address1,
                    Address2 = Address2,
                    Owner = AccountService.Instance.Current_Account.Email,
                    PhoneNumber = Phone
                };

                var result = await AddressService.Instance.AddAddress(item);

                DialogService.HideLoading();

                if (result == "true")
                {
                    DialogService.ShowSuccess(Strings.Address_Added);
                    await _pageService.PopAsync();

                }
                else
                {
                    DialogService.ShowError(result);
                }

            }
            catch (Exception ex)
            {
                DialogService.ShowError(Strings.SomethingWrong);
                Debug.WriteLine(Keys.TAG + ex);
                Crashes.TrackError(ex);
            }
            finally { IsBusy = false; }
        }
    }
}
