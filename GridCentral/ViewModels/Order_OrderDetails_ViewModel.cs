using GridCentral.Helpers;
using GridCentral.Interfaces;
using GridCentral.Models;
using GridCentral.Services;
using GridCentral.Views.Contact;
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
    public class Order_OrderDetails_ViewModel : Base_ViewModel
    {

        #region Bind Property
        #region Note
        bool _hasMessage;
        string _orderMessage;

        public bool hasMessage
        {
            get { return _hasMessage; }
            set { _hasMessage = value; OnPropertyChanged("hasMessage"); }
        }
        public string OrderMessage
        {
            get { return _orderMessage; }
            set { _orderMessage = value; OnPropertyChanged("OrderMessage"); }
        }
        #endregion
        string _orderDate;
        string _orderId;
        string _ordernumber;
        string _orderTotal;
        string _orderStatus;

        bool _NotApproved = false;
        string _statuscolor = "Gray";
        string _BgUpdateBtn = "Gray";
        bool _isUpdatable = false;

        bool _changeable = true;

        public bool isUpdatable
        {
            get { return _isUpdatable; }
            set {
                _isUpdatable = value; OnPropertyChanged("isUpdatable");
                if (isUpdatable)
                {
                    BgUpdateBtn = "#CB224E";
                }
            }
        }

        public string BgUpdateBtn
        {
            get { return _BgUpdateBtn; }
            set { _BgUpdateBtn = value; OnPropertyChanged("BgUpdateBtn"); }
        }

        public string OrderId
        {
            get { return _orderId; }
            set { _orderId = value; OnPropertyChanged("OrderId"); }
        }

        public string OrderNumber
        {
            get { return _ordernumber; }
            set { _ordernumber = value; OnPropertyChanged("OrderNumber"); }
        }

        public string OrderDate
        {
            get { return _orderDate; }
            set { _orderDate = value; OnPropertyChanged("OrderDate"); }
        }


        public string OrderTotal
        {
            get { return _orderTotal; }
            set { _orderTotal = value; OnPropertyChanged("OrderTotal"); }
        }

        public string OrderStatus
        {
            get { return _orderStatus; }
            set { _orderStatus = value; OnPropertyChanged("OrderStatus"); }
        }

        public string StatusColor
        {
            get { return _statuscolor; }
            set { _statuscolor = value; OnPropertyChanged("StatusColor"); }
        }

        public bool ShowStateBar
        {
            get { return _NotApproved; }
            set { _NotApproved = value; OnPropertyChanged("NotApproved"); }
        }

        public bool Changeable
        {
            get { return _changeable; }
            set { _changeable = value; OnPropertyChanged("Changeable"); }
        }

        #region Stages
        bool _stage1 = false;
        bool _stage2 = false;
        bool _stage3 = false;
        bool _stage4 = false;

        public bool Stage1
        {
            get { return _stage1; }
            set { _stage1 = value; OnPropertyChanged("Stage1"); }
        }

        public bool Stage2
        {
            get { return _stage2; }
            set { _stage2 = value; OnPropertyChanged("Stage2"); }
        }

        public bool Stage3
        {
            get { return _stage3; }
            set { _stage3 = value; OnPropertyChanged("Stage3"); }
        }

        public bool Stage4
        {
            get { return _stage4; }
            set { _stage4 = value; OnPropertyChanged("Stage4"); }
        }
        #endregion
        #region Summary

        string _ItemTotal;
        string _ShippingTotal;
        string _TaxTotal;
        string _GrandTotal;

        public string GrandTotal
        {
            get { return _GrandTotal; }
            set
            {
                _GrandTotal = value;
                OnPropertyChanged("GrandTotal");
            }
        }
        public string TaxTotal
        {
            get { return _TaxTotal; }
            set
            {
                _TaxTotal = value;
                OnPropertyChanged("TaxTotal");
            }
        }
        public string ShippingTotal
        {
            get { return _ShippingTotal; }
            set
            {
                _ShippingTotal = value;
                OnPropertyChanged("ShippingTotal");
            }
        }
        public string ItemTotal
        {
            get { return _ItemTotal; }
            set
            {
                _ItemTotal = value;
                OnPropertyChanged("ItemTotal");
            }
        }
        #endregion
        #region Address
        string _address1; string _address2;
        string _name;string _phonenumber;

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

        public string PhoneNumber
        {
            get { return _phonenumber; }
            set
            {
                _phonenumber = value;
                OnPropertyChanged("PhoneNumber");
            }
        }
        public string Name
        {
            get { return _name; }
            set
            {
                _name = value;
                OnPropertyChanged("Name");
            }
        }
        #endregion
        #region Time
        string _DeliveryTime;
        public string DeliveryTime
        {
            get { return _DeliveryTime; }
            set
            {
                _DeliveryTime = value;
                OnPropertyChanged("DeliveryTime");
            }
        }
        #endregion
        public mOrder _order = new mOrder();
        #endregion

        IPageService _pageService;
        public ICommand UpdateCommand { get; private set; }
        public Order_OrderDetails_ViewModel(mOrder order, IPageService pageService)
        {
            UpdateCommand = new Command(() => UpdateAction());
            _order = order;
            _pageService = pageService;
            OrderId = order.OrderId; OrderStatus = order.Status; OrderDate = order.Order_At.Split('T')[0]; OrderTotal = order.GrandPrice;
            Address1 = order.Address1; Address2 = order.Address2; Name = order.Name; DeliveryTime = order.DeliveryTime; PhoneNumber = order.PhoneNumber;
            ItemTotal = order.ItemTotal; TaxTotal = order.TaxPrice; ShippingTotal = order.ShippingPrice; GrandTotal = order.GrandPrice; OrderNumber = order.OrderNumber;
            if(order.OrderMessage != null)
            {
                OrderMessage = order.OrderMessage;
                hasMessage = true;
            }
            Check_Status_Stage();
        }

        private async void UpdateAction()
        {
            if (BgUpdateBtn == "Gray") return;

            if(_order.Status == Keys.OrderStatus[3] || _order.Status == Keys.OrderStatus[4] || _order.Status == Keys.OrderStatus[5])
            {
                var ans = await DialogService.DisplayAlert("Ok", null, "Notice", "Order can not be updated at this stage");
                return;
            }

            if (IsBusy) return;

            IsBusy = true;

            try
            {
                DialogService.ShowLoading("Updating Order");

                mOrder updateOrder = new mOrder()
                {
                    OrderId = _order.OrderId,
                    OwnerEmail = AccountService.Instance.Current_Account.Email,
                    DeliveryTime = DeliveryTime,
                    Address1 = Address1,
                    Address2 = Address2,
                    PhoneNumber = PhoneNumber,
                    Name = Name
                };

                var result = await OrderService.Instance.UpdateDetail(updateOrder);

                DialogService.HideLoading();

                if (result != "true")
                {
                    DialogService.ShowError(result);
                    return;
                }
                CrossSettings.Current.AddOrUpdateValue<bool>("StatusUpdate", true);
                DialogService.ShowSuccess("Order Updated");

            }
            catch(Exception ex)
            {

            }
            finally { IsBusy = true; isUpdatable = false; }
        }

        private void Check_Status_Stage()
        {
            if(OrderStatus == Keys.OrderStatus[0])//Pending
            {
                ShowStateBar = true;
                StatusColor = "Gray";
                return;
            }else if(OrderStatus == Keys.OrderStatus[1])//Approved
            {
                Stage1 = true;
                return;
            }else if(OrderStatus == Keys.OrderStatus[2])//Preparing
            {
                Changeable = false;
                Stage1 = true; Stage2 = true;
                return;
            }else if(OrderStatus == Keys.OrderStatus[3])//Delay
            {
                ShowStateBar = true;
                Changeable = false;
                Stage1 = true; Stage2 = true;
                StatusColor = "#F5D04C";
                return;
            }else if(OrderStatus == Keys.OrderStatus[4])//Transit
            {
                Changeable = false;
                Stage1 = true; Stage2 = true; Stage3 = true;
                return;
            } else if(OrderStatus == Keys.OrderStatus[5])//Delivered
            {
                Changeable = false;
                Stage1 = true; Stage2 = true; Stage3 = true; Stage4 = true;
                return;
            }
            else if (OrderStatus == Keys.OrderStatus[6])//Canceled
            {
                Changeable = false;
                ShowStateBar = true;
                StatusColor = "Red";
                return;
            }

        }

        public async void Cancel_Order()
        {
            if (IsBusy) return;

            IsBusy = true;

            try
            {
                DialogService.ShowLoading("Canceling Order");
                var result = await OrderService.Instance.CancelOrder(_order);
                DialogService.HideLoading();                                            

                if (result == "true")
                {
                    _order.Status = Keys.NotifyWhys[4];
                    CrossSettings.Current.AddOrUpdateValue<bool>("StatusUpdate", true);
                    //DialogService.HideLoading();
                    DialogService.ShowSuccess("Order Canceled");
                    await _pageService.PopAsync();
                    return;
                }

                if(result == "Grace Peroid Is Finish")   //Client Side State still giving cancel status
                {
                   // DialogService.HideLoading();
                    var res =  await DialogService.DisplayAlert(Strings.Contact_Us, Strings.Close,Strings.Cancelation, Strings.Cancelation_Period_Ended);

                    if (res)
                    {
                       await _pageService.PushAsync(new ContactUs());
                    }

                    return;
                }

                DialogService.ShowError(result);

            }catch(Exception ex)
            {
                Debug.WriteLine(Keys.TAG + ex);
                DialogService.ShowError(Strings.SomethingWrong);
                await _pageService.PopAsync();
            }
            finally { IsBusy = false; }
        }
    }
}
