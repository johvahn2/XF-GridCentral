using GridCentral.Helpers;
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

namespace GridCentral.ViewModels
{
    public class Order_OrderList_ViewModel : Base_ViewModel
    {

        #region Bind Property
        ObservableCollection<mOrder> _MyOrders = new ObservableCollection<mOrder>();
        bool _IsListRefereshing = false;
        bool _asItems;

        public ObservableCollection<mOrder> RevMyOrders
        {
            get { return new ObservableCollection<mOrder>(MyOrders.Reverse()); }
        }

        ObservableCollection<mOrder> MyOrders
        {
            get { return _MyOrders; }
            set { _MyOrders = value; OnPropertyChanged("MyOrders"); OnPropertyChanged("RevMyOrders"); }
        }

        public bool IsListRefereshing
        {
            get { return _IsListRefereshing; }
            set
            {
                _IsListRefereshing = value;

                OnPropertyChanged("IsListRefereshing");
                if (IsListRefereshing)
                {
                    GetMyOrders();
                }
            }
        }

        public bool noItems
        {
            get { return _asItems; }
            set { _asItems = value; OnPropertyChanged("noItems"); }
        }
        #endregion

        public Order_OrderList_ViewModel()
        {
            IsBusy = true;
            GetMyOrders();
        }

        public async void GetMyOrders()
        {
            try
            {

                noItems = false;
                if (CrossConnectivity.Current.IsConnected)
                {
                    var result = await OrderService.Instance.FetchOrders(AccountService.Instance.Current_Account.Email);


                    if (result == null)
                    {
                        noItems = true;
                        MyOrders = new ObservableCollection<mOrder>();
                        return;
                    }

                    MyOrders = FormatData(result);

                    OfflineService.Write<ObservableCollection<mOrder>>(result, Strings.Order_Offline_fileName, null);

                }
                else
                {
                    var result = await OfflineService.Read<ObservableCollection<mOrder>>(Strings.Order_Offline_fileName, null);

                    if (result == null)
                    {
                        noItems = true;
                        MyOrders = new ObservableCollection<mOrder>();
                        return;
                    }

                    MyOrders = FormatData(result);

                }



            }
            catch (Exception ex)
            {
                Debug.WriteLine(Keys.TAG + ex);
                DialogService.ShowError(Strings.SomethingWrong);
            }
            finally { IsListRefereshing = false; IsBusy = false; }
        }

        private ObservableCollection<mOrder> FormatData(ObservableCollection<mOrder> orders)
        {
            for (var i = 0; i < orders.Count; i++)
            {
                orders[i].Order_At = orders[i].Order_At.Split('T')[0];
            }

            return orders;
        }
    }
}
