using GridCentral.Helpers;
using GridCentral.Interfaces;
using GridCentral.Models;
using GridCentral.Services;
using GridCentral.Views.Navigation;
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
    public class BuySell_DashBoard_ViewModel : Base_ViewModel
    {

        #region Bind Property
        bool _IsNoConnection = false;
        bool _showloadmore = false;

        public bool IsNoConnection
        {
            get { return _IsNoConnection; }
            set { _IsNoConnection = value; OnPropertyChanged("IsNoConnection"); }
        }

        public bool ShowLoadMore
        {
            get { return _showloadmore; }
            set { _showloadmore = value; OnPropertyChanged("ShowLoadMore"); }
        }

        public ICommand GoToGridShopCommand { get; private set; }
        public ICommand LoadMoreCommand { get; private set; }
        #endregion
        ObservableCollection<mUserItem> result = null;
        IPageService _pageService;
        public BuySell_DashBoard_ViewModel(IPageService pageService)
        {
            _pageService = pageService;
            usingMain = false;

            GoToGridShopCommand = new Command(() => { _pageService.ShowMain(new RootPage()); });
        }


        public async Task<ObservableCollection<mUserItem>> GetRandomProducts(int amount, int len)
        {
            IsBusy = true;

            try
            {
                //ObservableCollection<mUserItem> result = null;

                if (CrossConnectivity.Current.IsConnected)
                {
                    result = await ItemService.Instance.FetchRandom(amount, null, null, len);
                    if (result == null)
                    {
                        DialogService.ShowToast("No More Results");

                        return new ObservableCollection<mUserItem>();
                    }
                    OfflineService.Write<ObservableCollection<mUserItem>>(result, Strings.BuySellDash_Offline_fileName, null);
                }
                else
                {
                    result = await OfflineService.Read<ObservableCollection<mUserItem>>(Strings.BuySellDash_Offline_fileName, null);
                    if (result == null)
                        IsNoConnection = true;
                }

                if (result == null)
                {
                    //No Item Found(WORK ON)

                    return new ObservableCollection<mUserItem>();
                }

                if (result.Count > 19) ShowLoadMore = true;

                for (var i = 0; i < result.Count; i++)
                {
                    result[i].Thumbnail = result[i].Images[0];

                    if (i == result.Count)
                    {
                        result[i].ThumbnailHeight = "1000";

                        continue;
                    }

                    result[i].ThumbnailHeight = "100";


                    int max_Name_Length = 28;

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
                return null;
            }
            finally { IsBusy = false; }
        }
    }
}
