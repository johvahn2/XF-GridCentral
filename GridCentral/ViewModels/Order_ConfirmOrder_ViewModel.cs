using GridCentral.Helpers;
using GridCentral.Interfaces;
using GridCentral.Models;
using GridCentral.Services;
using GridCentral.Views.Navigation;
using GridCentral.Views.Order;
using Microsoft.AppCenter.Crashes;
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
    public class Order_ConfirmOrder_ViewModel : Base_ViewModel
    {
        #region Bind Property
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
        string _Address_Mod_Txt = "Change Address";
        string _address1;string _address2;
        string _name;
        bool _noItems = false;
        bool _asAddress = false;
        bool _noAddress = false;
        public bool noItems
        {
            get { return _noItems; }
            set
            {
                _noItems = value;
                OnPropertyChanged("noItems");
            }
        }
        public bool noAddress
        {
            get { return _noAddress; }
            set
            {
                _noAddress = value;
                OnPropertyChanged("noAddress");
            }
        }
        public bool asAddress
        {
            get { return _asAddress; }
            set
            {
                _asAddress = value;
                OnPropertyChanged("asAddress");
            }
        }
        public string Address_Mod_Txt
        {
            get { return _Address_Mod_Txt; }
            set
            {
                _Address_Mod_Txt = value;
                OnPropertyChanged("Address_Mod_Txt");
            }
        }

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
        #region Payment Method
        string _paymentmethod;
        mCard _currentcard;
        string _card_lastdigit;
        bool _nocard = false;
        bool _usecard = false;

        public bool UseCard
        {
            get { return _usecard; }
            set
            {
                _usecard = value;
                OnPropertyChanged("UseCard");
            }
        }
        public bool NoCard
        {
            get { return _nocard; }
            set
            {
                _nocard = value;
                OnPropertyChanged("NoCard");
            }
        }
        public string PaymentMethod
        {
            get { return _paymentmethod; }
            set
            {
            _paymentmethod = value;
                OnPropertyChanged("PaymentMethod");
            }
        }

        public string Card_lastdigit
        {
            get { return _card_lastdigit; }
            set
            {
                _card_lastdigit = value;
                OnPropertyChanged("Card_lastdigit");
            }
        }

        public mCard CurrentCard
        {
            get { return _currentcard; }
            set
            {
                _currentcard = value;
                OnPropertyChanged("CurrentCard");
                Card_lastdigit = CurrentCard.Cardnumber.Substring(12);
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


        #endregion
        public ICommand PlaceOrderCommand { get; private set; }
        IPageService _pageSerivce;
        mOrderAddress _address;
        public Order_ConfirmOrder_ViewModel(IPageService pageService,mOrderAddress address, mCard Card, ObservableCollection<mCart> CartList, bool usingCard)
        {
            //Display Card Info use CurrentCard
            if (address != null)
            {
            _address = address;
            }
            _pageSerivce = pageService;
            PlaceOrderCommand = new Command(() => PlaceOrder(CartList));
            PriceSum(CartList);
            #region Address
            if(address == null)
            {
                asAddress = false;
                noAddress = true;
                Address_Mod_Txt = "Add Address";
                return;
            }
            asAddress = true;
            noAddress = false;
            Name = AccountService.Instance.Current_Account.FirstName + " " + AccountService.Instance.Current_Account.LastName;
            Address1 = address.Address1;
            Address2 = address.Address2;
            #endregion
            UseCard = usingCard;
            if (usingCard){ PaymentMethod = Strings.Credit_Debit_Card;} else { PaymentMethod = Strings.Cash_On_Delivery; };
        }

        private async void PlaceOrder(ObservableCollection<mCart> cartList)
        {
            if (String.IsNullOrEmpty(Address1))
            {
                DialogService.ShowErrorToast("Please Add Address");
                return;
            }

            if (IsBusy) return;

            IsBusy = true;

            try
            {
                DialogService.ShowLoading("Processing Order");

                ObservableCollection<mOrderItems> orderItems = new ObservableCollection<mOrderItems>();

                for (var i = 0; i < cartList.Count; i++)
                {
                    orderItems.Add(new mOrderItems{ ItemId = cartList[i].ProductId,ItemNameSub = cartList[i].Name, ItemName = cartList[i].bName, Quantity = cartList[i].Quantity, Price = cartList[i].Price, Thumbnail = cartList[i].Thumbnail, Seller = cartList[i].Manufacturer });
                }
                mOrder newOrder = new mOrder()
                {
                    GrandPrice = GrandTotal,
                    ItemTotal = ItemTotal,
                    ShippingPrice = ShippingTotal,
                    TaxPrice = TaxTotal,
                    OwnerEmail = AccountService.Instance.Current_Account.Email,
                    DeliveryTime = DeliveryTime,
                    Items = orderItems,
                    Address1 = Address1,
                    Address2 = Address2,
                    PhoneNumber = _address.PhoneNumber,
                    ProfileNumber = AccountService.Instance.Current_Account.PhoneNumber,
                    CardInfo = CurrentCard,
                    Name = Name
                };

                if (UseCard)
                {
                    newOrder.PaymentType = "Card";
                }
                else
                {
                    newOrder.PaymentType = "NoCard";
                }

                var result = await OrderService.Instance.SendOrder(newOrder);

                DialogService.HideLoading();

                if(result != "true")
                {
                    DialogService.ShowError(result);
                    return;
                }

                CartService.Instance.ClearCart(AccountService.Instance.Current_Account.Email);

                DialogService.ShowSuccess("Thank You, Your Order Have Been Sent");
                CrossSettings.Current.AddOrUpdateValue<bool>("OrderDone", true);

                 _pageSerivce.ShowMain(new RootPage(false, null, new OrderList()));

            }
            catch(Exception ex)
            {
                Debug.WriteLine(Keys.TAG + ex);
                DialogService.ShowError(Strings.SomethingWrong);
                Crashes.TrackError(ex);
            }
            finally { IsBusy = false; }
        }

        private async void PriceSum(ObservableCollection<mCart> CartList)
        {
            decimal Itemtotal = 0;

            for (var i = 0; i < CartList.Count; i++)
            {
                Itemtotal += Convert.ToDecimal(CartList[i].Price) * Convert.ToInt16(CartList[i].Quantity);
            }

            //var weight_percentage = await CartService.Instance.WeightPercentage(AccountService.Instance.Current_Account.Email);
            var shipping_cost = await CartService.Instance.ShippingCost(AccountService.Instance.Current_Account.Email);
            //double shipping_price = 0.00;
            //double temp = 0.00;

            ItemTotal = Itemtotal.ToString();
            //temp = Convert.ToDouble(weight_percentage) / 100;
            if(shipping_cost == "0")
            {
                ShippingTotal = "0.00";
            }
            else
            {
                ShippingTotal = "5.00";

            }
             TaxTotal = "0.00";
            decimal grandtotal = Convert.ToDecimal(ItemTotal) + Convert.ToDecimal(ShippingTotal) + Convert.ToDecimal(TaxTotal);

            GrandTotal = grandtotal.ToString();
        }

    }
}
