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
    public class Profile_EditItem_ViewModel : Base_ViewModel
    {
        #region Bind Property
        public String SelectedCat;
        public List<byte[]> ByteList = new List<byte[]>();

        public List<byte[]> BytesItems = CreateList<byte[]>(4);

        public List<string> _StateItems = new List<string>() { "New", "Like-New", "Used", "Fair", "Broken" };
        public List<string> StateItems
        {
            get { return _StateItems; }
            set { _StateItems = value; OnPropertyChanged("StateItems"); }
        }

        int _stateindex;
        public int StateIndex
        {
            get { return _stateindex; }
            set { _stateindex = value; OnPropertyChanged("StateIndex"); }
        }

        int _ItemTypeRadioPickerIndex = 0;
        public int ItemTypeRadioPickerIndex
        {
            get { return _ItemTypeRadioPickerIndex; }
            set { _ItemTypeRadioPickerIndex = value; OnPropertyChanged("ItemTypeRadioPickerIndex"); }
        }



        public List<string> _CategoryItems = Keys.ItemCategories;
        public List<string> CategoryItems
        {
            get { return _CategoryItems; }
            set { _CategoryItems = value; OnPropertyChanged("CategoryItems"); }
        }

        int _categoryindex;
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

        public List<string> _VisableItems = new List<string>() { "Visible", "Not Visible" };
        public List<string> VisableItems
        {
            get { return _VisableItems; }
            set { _VisableItems = value; OnPropertyChanged("VisableItems"); }
        }

        int _visableindex;
        public int VisableIndex
        {
            get { return _visableindex; }
            set { _visableindex = value; OnPropertyChanged("VisableIndex"); }
        }


        ObservableCollection<mCarouselImage> _itemimages = new ObservableCollection<mCarouselImage>();
        string _name;
        string _price;
        string _quantity;
        string _state;
        string _description;
        string _ItemCreatedAt;

        public DateTime MiniDate
        {
            get { return DateTime.Now.Date; }
        }

        public string Name
        {
            get { return _name; }
            set { _name = value; OnPropertyChanged("Name"); }
        }

        public string Price
        {
            get { return _price; }
            set { _price = value; OnPropertyChanged("Price"); }
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

        public ObservableCollection<mCarouselImage> ItemImages
        {
            get { return _itemimages; }
            set { _itemimages = value; OnPropertyChanged("ItemImages"); }
        }


        public string ItemCreatedAt
        {
            get { return _ItemCreatedAt; }
            set { _ItemCreatedAt = value; OnPropertyChanged("ItemCreatedAt"); }
        }

        string _img1 = null;
        string _img2 = null;
        string _img3 = null;
        string _img4 = null;


        public string Img1
        {
            get { return _img1; }
            set { _img1 = value; OnPropertyChanged("Img1"); }
        }

        public string Img2
        {
            get { return _img2; }
            set { _img2 = value; OnPropertyChanged("Img2"); }
        }
        public string Img3
        {
            get { return _img3; }
            set { _img3 = value; OnPropertyChanged("Img3"); }
        }
        public string Img4
        {
            get { return _img4; }
            set { _img4 = value; OnPropertyChanged("Img4"); }
        }

        byte[] _bimg1 = null;
        byte[] _bimg2 = null;
        byte[] _bimg3 = null;
        byte[] _bimg4 = null;


        public byte[] bImg1
        {
            get { return _bimg1; }
            set { _bimg1 = value; OnPropertyChanged("Img1"); }
        }

        public byte[] bImg2
        {
            get { return _bimg2; }
            set { _bimg2 = value; OnPropertyChanged("Img2"); }
        }
        public byte[] bImg3
        {
            get { return _bimg3; }
            set { _bimg3 = value; OnPropertyChanged("Img3"); }
        }
        public byte[] bImg4
        {
            get { return _bimg4; }
            set { _bimg4 = value; OnPropertyChanged("Img4"); }
        }

        public ICommand UpdateItemCommand { get; private set; }
        public ICommand DeleteItemCommand { get; private set; }
        #endregion

        IPageService _pageService;
        public Profile_EditItem_ViewModel(IPageService pageService, mUserItem item)
        {
            _pageService = pageService;
            ConnectFields(item);

            UpdateItemCommand = new Command(async () => await UpdateItemActionAsync(item));
            DeleteItemCommand = new Command(async () => await DeleteItemAction(item));
        }


        private async Task UpdateItemActionAsync(mUserItem item)
        {
            if (IsBusy) return;

            IsBusy = true;

            DialogService.ShowLoading("Updating item");
            try
            {
                var result = await ItemService.Instance.UpdateItem(FormatItem(item));

                DialogService.HideLoading();

                if (result == "true")
                {
                    DialogService.ShowToast("Item Updated");
                    await _pageService.PopAsync();
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
            finally { IsBusy = false; }
        }

        private async Task DeleteItemAction(mUserItem item)
        {
           var response = await DialogService.DisplayAlert("Yes", "No", "Delete Item", "Are you sure?");

           if (!response) return;

            try
            {
                if (IsBusy) return; IsBusy = true;


                DialogService.ShowLoading("Deleting Item");

                var result = await ItemService.Instance.DeleteItem(item.Id);
                DialogService.HideLoading();
                if (result == "true")
                {
                    DialogService.ShowToast("Item Deleted");
                    return;
                }
                else
                {
                    DialogService.ShowError(result);
                }

            }
            catch(Exception ex)
            {
                Debug.WriteLine(Keys.TAG + ex);
                DialogService.ShowError(Strings.SomethingWrong);
                Crashes.TrackError(ex);
            }
            finally { IsBusy = false; }

        }

        private mUserItem FormatItem(mUserItem item)
        {
            var SelectedCategory = SelectedCat;//CategoryItems[CategoryIndex];
            var SelectedState = StateItems[StateIndex];
            var SelectedVisablity = VisableItems[VisableIndex];


            item.Name = Name;
            item.Category = SelectedCategory;
            item.Price = Convert.ToString(Math.Truncate(Convert.ToDecimal(Price) * 100) / 100);
            item.Description = Description;
            item.Quantity = Quantity;
            item.State = SelectedState;
            if (SelectedVisablity == VisableItems[1])
            {
                item.Visable = "false";
            }
            else
            {
                item.Visable = "true";
            }

            #region CheckingByte

            string[] curr_images = new string[4];

            for (var i = 0; i < item.Images.Count; i++)
            {
                curr_images[i] = item.Images[i];
            }
            if (bImg1 != null)
            {
                if (Img1 == "Remove")
                {
                    curr_images[0] = "Remove";
                }
                else
                {
                    // ByteList.Add(bImg1);
                    BytesItems[0] = bImg1;
                    //item.Images[0] = null;
                    curr_images[0] = null;
                }
            }
            if (bImg2 != null || Img2 == "Remove")
            {
                if (Img2 == "Remove")
                {
                    curr_images[1] = "Remove";
                }
                else
                {
                    BytesItems[1] = bImg2;
                    //item.Images[1] = null;
                    curr_images[1] = null;
                    //ByteList.Add(bImg2);

                }
            }
            if (bImg3 != null || Img3 == "Remove")
            {
                if (Img3 == "Remove")
                {
                    curr_images[2] = "Remove";

                }
                else
                {
                    BytesItems[2] = bImg3;
                    //item.Images[2] = null;
                    curr_images[2] = null;
                    //ByteList.Add(bImg3);

                }
            }
            if (bImg4 != null || Img4 == "Remove")
            {
                if (Img4 == "Remove")
                {
                    curr_images[3] = "Remove";
                }
                else
                {
                    //ByteList.Add(bImg4);
                    BytesItems[3] = bImg4;
                    //item.Images[3] = null;
                    curr_images[3] = null;

                }
            }
            List<string> loopedImages = new List<string>();
            for (var i = 0; i < curr_images.Length; i++)
            {
                loopedImages.Add(curr_images[i]);
            }

            #endregion

            item.Images = loopedImages;
            item.bImages = BytesItems;

            if (item.Images[0] != null)
            {
                if (item.Images[0] != "Remove")
                {
                    item.Thumbnail = new Uri(item.Thumbnail).AbsolutePath.Split('/')[2];
                }

            }

            for (int i = 0; i < item.Images.Count; i++)
            {
                if (item.Images[i] != null)
                {
                    if (item.Images[i] != "Remove")
                    {
                        item.Images[i] = new Uri(item.Images[i]).AbsolutePath.Split('/')[2];
                    }
                }
            }


            return item;

        }



        private void ConnectFields(mUserItem item)
        {
 
            ConnectImages(item.Images);

            Name = item.Name;
            Description = item.Description;
            Quantity = item.Quantity;
            Price = Price = Convert.ToString(Math.Truncate(Convert.ToDecimal(item.Price) * 100) / 100);

            ItemCreatedAt = "Created At: " + item.CreatedDate.Split('T')[0];

            //for (var i = 0; i < Keys.ItemCategories.Count; i++)
            //{
            //    if (item.Category == Keys.ItemCategories[i])
            //    {
            //        CategoryIndex = i;
            //        break;
            //    }
            //}
            for (var i = 0; i < Keys.ItemCategories.Count; i++)
            {
                 if(item.Category == Keys.CatItems[Keys.ItemCategories[i]]){

                    CategoryIndex = i;
                    break;
                }
            }


            for (var i=0; i < _StateItems.Count; i++)
            {
                if(item.State == _StateItems[i])
                {
                    StateIndex = i;
                    break;
                }
            }

            if(item.Visable == _VisableItems[1])
            {
                VisableIndex = 1;
            }
            else
            {
                VisableIndex = 0;
            }


        }

        private void ConnectImages(List<string> images)
        {
            int imgCount = images.Count;

            if (imgCount == 1)
            {
                Img1 = images[0];
            }
            else if (imgCount == 2)
            {
                Img1 = images[0]; Img2 = images[1];
            }
            else if (imgCount == 3)
            {
                Img1 = images[0]; Img2 = images[1]; Img3 = images[2];
            }
            else if (imgCount == 4)
            {
                Img1 = images[0]; Img2 = images[1]; Img3 = images[2]; Img4 = images[3];
            }


        }

        private static List<T> CreateList<T>(int capacity)
        {
            return Enumerable.Repeat(default(T), capacity).ToList();
        }
    }
}
