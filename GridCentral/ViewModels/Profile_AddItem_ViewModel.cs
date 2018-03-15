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
    public class Profile_AddItem_ViewModel : Base_ViewModel
    {
        #region Bind Property
        public List<byte[]> ByteList = new List<byte[]>();

        public String SelectedCat;

        public List<string> _CategoryItems = Keys.ItemCategories;

        public List<string> _StateItems = new List<string>() { "New", "Like-New", "Used", "Fair", "Broken" };
        public List<string> StateItems
        {
            get { return _StateItems; }
            set { _StateItems = value; OnPropertyChanged("StateItems"); }
        }

        public List<string> CategoryItems
        {
            get { return _CategoryItems; }
            set { _CategoryItems = value; OnPropertyChanged("CategoryItems");}
        }

        int _categoryindex;

        int _stateindex;
        public int StateIndex
        {
            get { return _stateindex; }
            set { _stateindex = value; OnPropertyChanged("StateIndex"); }
        }
        public int CategoryIndex
        {
            get { return _categoryindex; }
            set { _categoryindex = value; OnPropertyChanged("CategoryIndex");

                string catgeroyTempo = CategoryItems[CategoryIndex];
                SelectedCat = catgeroyTempo;

                if (Keys.CatItems.ContainsKey(catgeroyTempo))
                {
                    SelectedCat = Keys.CatItems[catgeroyTempo];
                }
            }
        }


        string _title;
        string _description;
        string _quantity;
        string _state;
        string _fixedPirce;
        private static ObservableCollection<mCarouselImage> _itemimages = new ObservableCollection<mCarouselImage>();



        public string Title
        {
            get { return _title; }
            set { _title = value; OnPropertyChanged("Title"); }
        }

        public string Description
        {
            get { return _description; }
            set { _description = value; OnPropertyChanged("Description"); }
        }

        public string Quantity
        {
            get { return _quantity; }
            set { _quantity = value; OnPropertyChanged("Quantity"); }
        }

        public string State
        {
            get { return _state; }
            set { _state = value; OnPropertyChanged("State"); }
        }

        public static ObservableCollection<mCarouselImage> ItemImages
        {
            get { return _itemimages; }
            set { _itemimages = value; }
        }

        public string FixedPricer
        {
            get { return _fixedPirce; }
            set { _fixedPirce = value; OnPropertyChanged("FixedPrice"); }
        }



        byte[] _img1 = null;
        byte[] _img2 = null;
        byte[] _img3 = null;
        byte[] _img4 = null;


        public byte[] Img1
        {
            get { return _img1; }
            set { _img1 = value; OnPropertyChanged("Img1"); }
        }

        public byte[] Img2
        {
            get { return _img2; }
            set { _img2 = value; OnPropertyChanged("Img2"); }
        }
        public byte[] Img3
        {
            get { return _img3; }
            set { _img3 = value; OnPropertyChanged("Img3"); }
        }
        public byte[] Img4
        {
            get { return _img4; }
            set { _img4 = value; OnPropertyChanged("Img4"); }
        }



        public ICommand AddItemCommand { get; private set; }
        public ICommand AddImage { get; private set; }
        public ICommand CancelCommand { get; private set; }
        #endregion

        private readonly IPageService _pageService;

        public Profile_AddItem_ViewModel(IPageService pageservice)
        {
            _pageService = pageservice;

            AddItemCommand = new Command(() => AddItemAction());
            CancelCommand = new Command(() => CancelAction());
        }

        private async void CancelAction()
        {
            var result = await DialogService.DisplayAlert("Yes", "No", "Cancel Item", "Are You Sure You Want To Cancel?");

            if (result) await _pageService.PopAsync();

        }

        bool secondtime = false;

        private async void AddItemAction()
        {

            var SelectedCategory =  SelectedCat;//CategoryItems[CategoryIndex];//
            var SelectedState = StateItems[StateIndex];

            if (IsBusy) return;

            IsBusy = true;


            DialogService.ShowLoading("Adding Item");


            try
            {

                if (!secondtime) GetImgBytes();

                if (!(ByteList.Count > 0))
                {
                    DialogService.ShowError("Please Add Atleast one image");
                    return;
                }
                var theimages = ByteList;

                mUserItem item = new mUserItem()
                {
                    Name = Title,
                    Description = Description,
                    Category = SelectedCategory,
                    Quantity = Quantity,
                    State = SelectedState,
                    Price = Convert.ToString(Math.Truncate(Convert.ToDecimal(FixedPricer) * 100) / 100),
                    bImages = theimages,
                    Manufacturer = AccountService.Instance.Current_Account.Email,
                    Displayname = "..."
                };

                if (!String.IsNullOrEmpty(AccountService.Instance.Current_Account.Displayname))
                {
                    item.Displayname = AccountService.Instance.Current_Account.Displayname;
                }
                var result = await ItemService.Instance.Additem(item);
                DialogService.HideLoading();

                if (result == "true")
                {
                    DialogService.ShowSuccess("Item Added");
                    await _pageService.PopAsync();

                }
                else
                {
                    DialogService.ShowError(result);
                    secondtime = true;
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

        private void GetImgBytes()
        {
            if (Img1 != null)
            {
                ByteList.Add(Img1);
            }

            if (Img2 != null)
            {
                ByteList.Add(Img2);
            }

            if (Img3 != null)
            {
                ByteList.Add(Img3);
            }

            if (Img4 != null)
            {
                ByteList.Add(Img4);
            }

        }
    }
}
