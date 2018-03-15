using GridCentral.Helpers;
using GridCentral.Interfaces;
using GridCentral.Models;
using GridCentral.Services;
using GridCentral.Views.Profile;
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
    public class Profile_MyItems_ViewModel : Base_ViewModel
    {
        #region Bind Property
        string _TxtSearched;
        bool _isDone;
        public string TxtSearch
        {
            get { return _TxtSearched; }
            set { _TxtSearched = value; OnPropertyChanged("TxtSearch"); if (string.IsNullOrEmpty(TxtSearch)) IsListRefereshing = true; }
        }

        ObservableCollection<mUserItem> _MyItemList = new ObservableCollection<mUserItem>();

        bool _IsListRefereshing = false;
        bool _asItems = false;
        bool _ShowSearchBar = false;

        public ObservableCollection<mUserItem> RevMyItems
        {
            get { return new ObservableCollection<mUserItem>(MyItemList.Reverse()); }
        }

        public ObservableCollection<mUserItem> MyItemList
        {
            get { return _MyItemList; }
            set
            {
                _MyItemList = value;
                OnPropertyChanged("MyItemList");
                OnPropertyChanged("RevMyItems");

            }
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
                    GetMyItems(1000,0);
                }
            }
        }

        public bool isDone
        {
            get { return _isDone; }
            set { _isDone = value; OnPropertyChanged("isDone"); }
        }

        public bool noItems
        {
            get { return _asItems; }
            set { _asItems = value; OnPropertyChanged("noItems"); }
        }


        public bool ShowSearchBar
        {
            get { return _ShowSearchBar; }
            set { _ShowSearchBar = value; OnPropertyChanged("ShowSearchBar"); }
        }

        IPageService _pageService;
        INavigation _navigation;
        public ICommand SearchCommand { get; private set; }
        public ICommand SearchTxt { get; private set; }
        public ICommand AddCommand { get; private set; }
        #endregion


        public Profile_MyItems_ViewModel(IPageService pageService)
        {
            AddCommand = new Command(() => AddItemAction());
            SearchCommand = new Command(() => SearchAction());
            SearchTxt = new Command(txt => SearchTxtAction(txt));
            _pageService = pageService;

           // GetMyItems(10,0);
        }

        private async void SearchTxtAction(object txt)
        {
            if (IsBusy) return;

            IsBusy = true;

            try
            {

                noItems = false;

                var result = await ItemService.Instance.FetchUsersItems(AccountService.Instance.Current_Account.Email, 15, 0, txt.ToString());

                if (result != null)
                {
                    var newResult = await BindDelete(result);

                    for (var i = 0; i < newResult.Count; i++)
                    {
                        newResult[i].Thumbnail = newResult[i].Images[0];
                        //newResult[i].CreatedDate = newResult[i].CreatedDate.Split('T')[0];


                        int max_description_length = 159;
                        int max_Name_length = 21;

                        if (newResult[i].Name.Length > max_Name_length)
                        {
                            newResult[i].SummaryName = newResult[i].Name.Substring(0, max_Name_length) + "...";
                        }
                        else
                        {
                            newResult[i].SummaryName = newResult[i].Name;
                        }

                        if (newResult[i].Description.Length > max_description_length)
                        {
                            newResult[i].SummaryDesc = newResult[i].Description.Substring(0, max_description_length) + "...";
                        }
                        else
                        {
                            newResult[i].SummaryDesc = newResult[i].Description;
                        }

                        if (newResult[i].Visable == "false")
                        {
                            newResult[i].Visable = "Not Visible";
                        }
                        else
                        {
                            newResult[i].Visable = "Visible";
                        }
                    }
                    MyItemList = newResult;
                    return;
                }

                noItems = true;
                MyItemList = new ObservableCollection<mUserItem>();

            }
            catch(Exception ex)
            {
                DialogService.ShowError(Strings.SomethingWrong);
                Debug.WriteLine(Keys.TAG + ex);
                Crashes.TrackError(ex);
            }
            finally { IsBusy = false; }
        }

        private void SearchAction()
        {
            ShowSearchBar = !ShowSearchBar;

        }

        private void AddItemAction()
        {
            _pageService.PushAsync(new AddItem());
        }

        public async void GetMyItems(int amount, int len , bool addon=false)
        {
            if (IsBusy) return;

            IsBusy = true;
            try
            {
                noItems = false;

                ObservableCollection<mUserItem> result = null;
                if (CrossConnectivity.Current.IsConnected)
                {
                   result = await ItemService.Instance.FetchUsersItems(AccountService.Instance.Current_Account.Email,amount,len);
                    if (result != null)
                    {
                        var newResult = await BindDelete(result);

                        for (var i = 0; i < newResult.Count; i++)
                        {
                            newResult[i].Thumbnail = newResult[i].Images[0];
                            //newResult[i].CreatedDate = newResult[i].CreatedDate.Split('T')[0];

                            int max_description_length = 159;
                            int max_Name_length = 21;

                            if(newResult[i].Name.Length > max_Name_length)
                            {
                                newResult[i].SummaryName = newResult[i].Name.Substring(0, max_Name_length) + "...";
                            }
                            else
                            {
                                newResult[i].SummaryName = newResult[i].Name;
                            }

                            if (newResult[i].Description.Length > max_description_length)
                            {
                                newResult[i].SummaryDesc = newResult[i].Description.Substring(0, max_description_length) + "...";
                            }
                            else
                            {
                                newResult[i].SummaryDesc = newResult[i].Description;
                            }

                            if(newResult[i].Visable == "false")
                            {
                                newResult[i].Visable = "Not Visible";
                            }
                            else
                            {
                                newResult[i].Visable = "Visible";
                            }
                        }
                        if (addon)
                            MyItemList.AddRange(newResult);
                        else
                            MyItemList = newResult;
                        return;
                    }
                   OfflineService.Write<ObservableCollection<mUserItem>>(result, Strings.MyItems_Offline_fileName, null);

                }
                else
                {
                    result = await OfflineService.Read<ObservableCollection<mUserItem>>(Strings.MyItems_Offline_fileName, null);
                }


                if (result != null)
                {
                    var newResult = await BindDelete(result);

                    for(var i=0;i< newResult.Count; i++)
                    {
                        newResult[i].Thumbnail = newResult[i].Images[0];
                        newResult[i].CreatedDate = newResult[i].CreatedDate.Split('T')[0];
                    }
                    if (addon)
                        MyItemList.AddRange(newResult);
                    else
                        MyItemList = newResult;
                    return;
                }

                if (addon)
                {
                    isDone = true;
                }
                else
                {
                    noItems = true;
                    MyItemList = new ObservableCollection<mUserItem>();
                }
           
            }
            catch (Exception ex)
            {
                DialogService.ShowError(Strings.SomethingWrong);
                Debug.WriteLine(Keys.TAG + ex);
                Crashes.TrackError(ex);
            }
            finally { IsListRefereshing = false; IsBusy = false; }

        }

        private async Task<ObservableCollection<mUserItem>> BindDelete(ObservableCollection<mUserItem> result)
        {
            for (var i = 0; i < result.Count; i++)
            {
                result[i].DeleteItemCommand = new Command(itemName => DeleteItemAction(itemName));
            }
            return result;
        }

        private async void DeleteItemAction(object itemName)
        {
            var sure = await DialogService.DisplayAlert("Yes", "No", "Deleting Item", "Are you sure you want to delete the item");
            if (!sure) return;

            try
            {
                mUserItem listitem = (from itm in MyItemList
                                      where itm.Name == itemName.ToString()
                                      select itm)
                                        .FirstOrDefault<mUserItem>();

                MyItemList.Remove(listitem);
                OnPropertyChanged("RevMyItems");
                var result = await ItemService.Instance.DeleteItem(listitem.Id);

                if (result == "true")
                {
                    DialogService.ShowToast("Item Deleted");
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
                Crashes.TrackError(ex);
            }
        }
    }
}
