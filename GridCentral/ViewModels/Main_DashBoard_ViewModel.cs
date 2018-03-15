using GridCentral.Helpers;
using GridCentral.Interfaces;
using GridCentral.Models;
using GridCentral.Services;
using GridCentral.Views.BuySell;
using Microsoft.AppCenter.Crashes;
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
    public class Main_DashBoard_ViewModel : Base_ViewModel
    {
        bool _IsBusy_Deal;
        bool _IsBusy_Recent;
        bool _IsNoConnection = false;

        public bool IsNoConnection
        {
            get { return _IsNoConnection; }
            set { _IsNoConnection = value; OnPropertyChanged("IsNoConnection"); }
        }

        public bool IsBusy_Deal
        {
            get { return _IsBusy_Deal; }
            set { _IsBusy_Deal = value; OnPropertyChanged("IsBusy_Deal"); }
        }

        public bool IsBusy_Recent
        {
            get { return _IsBusy_Recent; }
            set { _IsBusy_Recent = value; OnPropertyChanged("IsBusy_Recent"); }
        }


        public ICommand GoToBuySellCommand { get; private set; }
        IPageService _pageService;
        public Main_DashBoard_ViewModel(IPageService pageService)
        {
            usingMain = true;
            _pageService = pageService;
            GoToBuySellCommand = new Command(() => GoToBuySellAction());
        }

        private void GoToBuySellAction()
        {
            _pageService.PushAsync(new DashBoard());
        }

        public async Task<ObservableCollection<Product>> GetInterestProductsAsync()
        {
            IsBusy = true;

            try
            {
                ObservableCollection<Product> result = null;

                if (CrossConnectivity.Current.IsConnected)
                {
                    result = await ProductService.Instance.FetchUserInterets(AccountService.Instance.Current_Account.Email);
                    if (result == null)
                    {
                        //ComeBack
                        return new ObservableCollection<Product>();
                    }
                    OfflineService.Write<ObservableCollection<Product>>(result, Strings.MyInterest_Offline_fileName, null);
                }
                else
                {
                  result = await OfflineService.Read<ObservableCollection<Product>>(Strings.MyInterest_Offline_fileName, null);

                    if (result == null)
                        IsNoConnection = true;
                }


                if(result == null)
                {
                    //ComeBack
                    return new ObservableCollection<Product>();
                }

                for(var i=0; i < result.Count; i++)
                {
                    result[i].Thumbnail = result[i].Images[0];

                    if (i == result.Count)
                    {
                        result[i].ThumbnailHeight = "1000";

                        continue;
                    }

                    result[i].ThumbnailHeight = "100";

                    int max_Name_Length = 39;

                    if(result[i].Name.Length > max_Name_Length)
                    {
                        result[i].SummaryName = result[i].Name.Substring(0, max_Name_Length) + "...";
                    }
                    else
                    {
                        result[i].SummaryName = result[i].Name;
                    }
                }

                IsNoConnection = false; return result;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(Keys.TAG + ex);
                DialogService.ShowError(Strings.SomethingWrong);
                Crashes.TrackError(ex);
                return null;
            }
            finally { IsBusy = false;}
        }


        public async Task<ObservableCollection<Product>> GetRandomProducts()
        {

            IsBusy = true;

            try
            {
                ObservableCollection<Product> result = null;

                if (CrossConnectivity.Current.IsConnected)
                {
                    result = await ProductService.Instance.FetchRandom(5, null, null, 0);
                    if (result == null)
                    {
                        //No Item Found(WORK ON)

                        return new ObservableCollection<Product>();
                    }
                    OfflineService.Write<ObservableCollection<Product>>(result, Strings.DashRandom_Offline_fileName, null);
                }
                else
                {
                    result = await OfflineService.Read<ObservableCollection<Product>>(Strings.DashRandom_Offline_fileName, null);
                    if (result == null)
                        IsNoConnection = true;
                }


                if (result == null)
                {
                    //No Item Found(WORK ON)

                    return new ObservableCollection<Product>();
                }

                for (var i = 0; i < result.Count; i++)
                {
                    result[i].Thumbnail = result[i].Images[0];

                    if (i == result.Count)
                    {
                        result[i].ThumbnailHeight = "1000";

                        continue;
                    }

                    result[i].ThumbnailHeight = "100";


                    int max_Name_Length = 39;

                    if (result[i].Name.Length > max_Name_Length)
                    {
                        result[i].SummaryName = result[i].Name.Substring(0, max_Name_Length) + "...";
                    }
                    else
                    {
                        result[i].SummaryName = result[i].Name;
                    }
                }

                IsNoConnection = false; return result;

            }
            catch (Exception ex)
            {
                Debug.WriteLine(Keys.TAG + ex);
                DialogService.ShowError(Strings.SomethingWrong);
                Crashes.TrackError(ex);
                return null;
            }
            finally { IsBusy = false;}
        }

        public async Task<ObservableCollection<Product>> GetDeals()
        {
            if (IsBusy_Deal) return null;

            IsBusy_Deal = true;

            try
            {

                ObservableCollection<Product> result = null;

                if (CrossConnectivity.Current.IsConnected)
                {
                    result = await ProductService.Instance.FetchDeals(6, null, null, 0);
                    if (result == null)
                    {
                        //No Item Found(WORK ON)

                        return new ObservableCollection<Product>();
                    }
                    OfflineService.Write<ObservableCollection<Product>>(result, Strings.DashDeals_Offline_fileName, null);
                }
                else
                {
                    result = await OfflineService.Read<ObservableCollection<Product>>(Strings.DashDeals_Offline_fileName, null);

                }

                if (result == null)
                {
                    //No Item Found(WORK ON)

                    return new ObservableCollection<Product>();
                }

                for (var i = 0; i < result.Count; i++)
                {
                    result[i].Thumbnail = result[i].Images[0];
                }

                return result;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(Keys.TAG + ex);
                DialogService.ShowError(Strings.SomethingWrong);
                Crashes.TrackError(ex);
                return null;
            }
            finally { IsBusy_Deal = false; }
        }


        public async Task<ObservableCollection<Product>> GetRecentroducts()
        {
            if (IsBusy_Recent) return null;
            IsBusy_Recent = true;

            try
            {
                ObservableCollection<Product> result = null;

                if (CrossConnectivity.Current.IsConnected)
                {
                    result = await ProductService.Instance.FetchRecent(8, null, null, 0);
                    if (result == null)
                    {
                        //No Item Found(WORK ON)

                        return new ObservableCollection<Product>();
                    }
                    OfflineService.Write<ObservableCollection<Product>>(result, Strings.DashRecent_Offline_fileName, null);
                }
                else
                {
                    result = await OfflineService.Read<ObservableCollection<Product>>(Strings.DashRecent_Offline_fileName, null);
                    if (result == null)
                        IsNoConnection = true;
                }


                if (result == null)
                {
                    //No Item Found(WORK ON)

                    return new ObservableCollection<Product>();
                }

                for (var i = 0; i < result.Count; i++)
                {
                    result[i].Thumbnail = result[i].Images[0];

                    int max_Name_length = 24;

                    if (result[i].Name.Length > max_Name_length)
                    {
                        result[i].SummaryName = result[i].Name.Substring(0, max_Name_length) + "...";
                    }
                    else
                    {
                        result[i].SummaryName = result[i].Name;
                    }

                }

                IsNoConnection = false; return result;

            }
            catch (Exception ex)
            {
                Debug.WriteLine(Keys.TAG + ex);
                DialogService.ShowError(Strings.SomethingWrong);
                Crashes.TrackError(ex);
                return null;
            }
            finally { IsBusy_Recent = false; }
        }


    }
}
