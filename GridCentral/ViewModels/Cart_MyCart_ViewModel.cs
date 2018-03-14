using Acr.UserDialogs;
using GridCentral.Helpers;
using GridCentral.Interfaces;
using GridCentral.Models;
using GridCentral.Services;
using GridCentral.Views.Order;
using Newtonsoft.Json;
using Plugin.Connectivity;
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
    public class Cart_MyCart_ViewModel : Base_ViewModel
    {

        #region Bind Property
        ObservableCollection<mCart> _MyCartList = new ObservableCollection<mCart>();
        ObservableCollection<Product> _MyProductList = new ObservableCollection<Product>();

        bool _asItems = false;
        bool _IsListRefereshing = false;
        bool _hidecheck = false;
        string _grandpirce;

        public bool IsListRefereshing
        {
            get { return _IsListRefereshing; }
            set
            {
                _IsListRefereshing = value;

                OnPropertyChanged("IsListRefereshing");
                if (IsListRefereshing)
                {
                    GetCart();
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

        public ObservableCollection<mCart> RevMyCartList
        {
            get { return new ObservableCollection<mCart>(MyCartList.Reverse()); }
        }

        public ObservableCollection<mCart> MyCartList
        {
            get { return _MyCartList; }
            set
            {
                _MyCartList = value;
                OnPropertyChanged("MyCartList");
                OnPropertyChanged("RevMyCartList");

            }
        }

        public bool noItems
        {
            get { return _asItems; }
            set { _asItems = value; OnPropertyChanged("noItems"); }
        }

        public bool hidecheck
        {
            get { return _hidecheck; }
            set { _hidecheck = !noItems; OnPropertyChanged("hidecheck"); }
        }

        public string GrandPrice
        {
            get { return _grandpirce; }
            set { _grandpirce = value; OnPropertyChanged("GrandPrice"); }
        }


        IPageService _pageService;
        public ICommand CheckoutCommand { get; private set; }


        #endregion

        public Cart_MyCart_ViewModel(IPageService pageService)
        {
            _pageService = pageService;
            CheckoutCommand = new Command(() => CheckOutAction());
            GetCart();
        }

        private async void CheckOutAction()
        {
            if (IsBusy) return;

            IsBusy = true;

            DialogService.ShowLoading("Proccesing");

            try
            {
                //double AddonTotal = 0;

                //for(var i=0; i < MyCartList.Count; i++)
                //{
                //    if (MyCartList[i].Addon == "true")
                //    {
                //        AddonTotal = AddonTotal + (Convert.ToDouble(MyCartList[i].Price) * Convert.ToInt16(MyCartList[i].Quantity));

                //    }
                //    else   //MAYBE
                //    {
                //        if ((Convert.ToDouble(MyCartList[i].Price) * Convert.ToInt16(MyCartList[i].Quantity)) > 24.00)
                //            break;
                //    }
                //    if ((i + 1) == MyCartList.Count)
                //    {
                //        DialogService.HideLoading();

                //        if (Convert.ToDouble(GrandPrice) > 44.00) break;

                //        if (AddonTotal < 44.00)
                //        {
                //            await DialogService.DisplayAlert(null, Strings.Ok, "Info", "You only have Addons in your Cart, Please add an item over 25 XCD or make addon items addup over 45 XCD");
                //            return;
                //        }

                //    }

                //}

                 if(!(Convert.ToDouble(GrandPrice) > 24.00))
                {
                    DialogService.HideLoading();
                    await DialogService.DisplayAlert(null, Strings.Ok, "Info", "Cart Total Most be $25.00 Or Over");
                    return;
                }


                var address = await AddressService.Instance.FetchAddresses(AccountService.Instance.Current_Account.Email, 1, 0);
                DialogService.HideLoading();

                bool useCard = false; //temp
                //string payment_response = await DialogService.DisplayActionSheet("Payment Methods", "Close", null, Strings.Cash_On_Delivery, Strings.Credit_Debit_Card);

                //if (payment_response == Strings.Credit_Debit_Card)
                //{
                //    useCard = true;
                //    DialogService.ShowToast("Not Available At The Moment");
                //    return;
                //}else if (payment_response == Strings.Cash_On_Delivery)
                //{
                //    useCard = false;
                //}
                //else
                //{
                //    return;
                //}


                if(address == null)
                {
                    await _pageService.PushAsync(new ConfirmOrder(null,MyCartList,useCard));
                    return;
                }
                await _pageService.PushAsync(new ConfirmOrder(address[0],MyCartList,useCard));

            }
            catch(Exception ex)
            {
                Debug.WriteLine(Keys.TAG + ex);
                DialogService.ShowError(Strings.SomethingWrong);
            }
            finally { IsBusy = false; }

        }


        public async void GetCart()
        {
            if (IsBusy) return;

            IsBusy = true; noItems = false;

            try
            {
                CartType result = null;
                if (CrossConnectivity.Current.IsConnected)
                {
                    result = await CartService.Instance.FetchUserCart(AccountService.Instance.Current_Account.Email);
                    if (result == null)
                    {
                        noItems = true;hidecheck = false;
                        MyCartList = new ObservableCollection<mCart>();
                        CalculateGrandPrice();
                        return;
                    }
                    OfflineService.Write<CartType>(result, Strings.MyCart_Offline_fileName, null);
                }
                else
                {
                    result = await OfflineService.Read<CartType>(Strings.MyCart_Offline_fileName, null);
                }

                if (result == null)
                {
                    noItems = true; hidecheck = false;
                    MyCartList = new ObservableCollection<mCart>();
                    CalculateGrandPrice();
                    return;
                }

                CartType orgnize = result;

                MyProductList = orgnize.Prop1;

                MyCartList = await BindClickables(FormatCart(MyProductList,orgnize.Prop2));

                CalculateGrandPrice();
                hidecheck = true;
            }
            catch (Exception ex)
            {

                Debug.WriteLine(Keys.TAG + ex);
                DialogService.ShowError(Strings.SomethingWrong);
            }
            finally { IsBusy = false; IsListRefereshing = false; }
        }

        private ObservableCollection<mCart> FormatCart(ObservableCollection<Product> myProductList, ObservableCollection<string> quantity)
        {
            myProductList = CalculatePercentages(myProductList);

            ObservableCollection<mCart> cart = new ObservableCollection<mCart>();
           for(var i=0; i < MyProductList.Count; i++)
            {
                cart.Add(new mCart
                {
                    Id = MyProductList[i].Id,
                    Name = MyProductList[i].Name,
                    Price = MyProductList[i].Price,
                    Status = MyProductList[i].Status,
                    Addon = MyProductList[i].Addon,
                    Manufacturer = MyProductList[i].Manufacturer,
                    Quantity = quantity[i],
                    Thumbnail = MyProductList[i].Images[0],
                    ProductId = myProductList[i].Id
                    
                });

                int max_name_length = 17;

                myProductList[i].SummaryName = MyProductList[i].Name;

                cart[i].bName = MyProductList[i].Name;

                if(MyProductList[i].Name.Length > max_name_length)
                {
                    myProductList[i].SummaryName = MyProductList[i].Name.Substring(0, max_name_length) + "...";
                    cart[i].Name = MyProductList[i].Name.Substring(0, max_name_length) + "...";
                }

                if(MyProductList[i].Status == "In Stock")
                {
                    cart[i].StatusColor = "Green";
                }
                else
                {
                    cart[i].StatusColor = "Red";
                }

            }

            return cart;
        }

        private void CalculateGrandPrice()
        {
            decimal total = 0;

            if(noItems)
            {
                GrandPrice = total.ToString();
                return;
            }

            for (var i = 0; i < MyCartList.Count; i++)
            {
                total += Convert.ToDecimal(MyCartList[i].Price) * Convert.ToInt16(MyCartList[i].Quantity);
            }
            GrandPrice = total.ToString();
        }

        private async Task<ObservableCollection<mCart>> BindClickables(ObservableCollection<mCart> result)
        {
            for (var i = 0; i < result.Count; i++)
            {
                result[i].RemoveCommand = new Command(async itemName => await RemoveActionAsync(itemName));
                result[i].SaveCommand = new Command(async itemName => await SaveActionAsync(itemName));
                result[i].QtyChgCommand = new Command(async itemName => await QtyChgActionAsync(itemName));
            }
            return result;
        }

        private async Task QtyChgActionAsync(object itemName)
        {
            try
            {
                mCart listitem = (from itm in MyCartList
                                  where itm.Name == itemName.ToString()
                                  select itm)
                        .FirstOrDefault<mCart>();

                Product product = (from itm in MyProductList
                                  where itm.SummaryName == itemName.ToString()
                                  select itm)
                        .FirstOrDefault<Product>();

                var chg = await DialogService.ShowInputPrompt("Update", "Cancel", "Change Quantity", "Quantity", listitem.Quantity, InputType.Number);

                if (chg == null) return;

                if(Convert.ToInt16(chg) > Convert.ToInt16(product.Quantity))
                {
                    DialogService.ShowErrorToast("Max QTY is " + product.Quantity);
                    return;
                }

                DialogService.ShowLoading("Updating Quantity");

                mCartS updateCart = new mCartS() { Quantity = chg, Owner = AccountService.Instance.Current_Account.Email,ProductId = listitem.Id};

                var result = await CartService.Instance.UpdateQuantity(updateCart);
                DialogService.HideLoading();
                if (result == "true")
                {
                    DialogService.ShowToast("Quantity Changed");
                    GetCart();
                    return;
                }

                DialogService.ShowError(result);



            }
            catch (Exception ex)
            {
                Debug.WriteLine(Keys.TAG + ex);
                DialogService.ShowError(Strings.SomethingWrong);
            }
        }

        private async Task SaveActionAsync(object itemName)
        {
            try
            {
                DialogService.ShowLoading("Saving Item");

                mCart listitem = (from itm in MyCartList
                                  where itm.Name == itemName.ToString()
                                  select itm)
                                        .FirstOrDefault<mCart>();

                mSavelater item = new mSavelater()
                {
                    Owner = AccountService.Instance.Current_Account.Email,
                    ProductId = listitem.Id
                };
                var result = await SavelaterService.Instance.Additem(item);
                DialogService.HideLoading();

                if (result == "true")
                {
                    GetCart();
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
                mCart listitem = (from itm in MyCartList
                                  where itm.Name == itemName.ToString()
                                  select itm)
                                        .FirstOrDefault<mCart>();

                MyCartList.Remove(listitem);
                OnPropertyChanged("RevMyItems");
                var result = await CartService.Instance.DeleteItem(listitem.Id,AccountService.Instance.Current_Account.Email);
                DialogService.HideLoading();

                if (result == "true")
                {
                    GetCart();
                    Debug.WriteLine("DELETED");
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

    public class CartType
    {
        public ObservableCollection<Product> Prop1 { get; set; }
        public ObservableCollection<string> Prop2 { get; set; }
    }
}
