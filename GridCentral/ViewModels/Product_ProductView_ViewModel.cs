using Acr.UserDialogs;
using GridCentral.Helpers;
using GridCentral.Interfaces;
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
using System.Windows.Input;
using Xamarin.Forms;

namespace GridCentral.ViewModels
{
    public class Product_ProductView_ViewModel : Base_ViewModel
    {

        #region Bind Property
        #region Question Property

        ObservableCollection<mQuestion> _Questionlist = new ObservableCollection<mQuestion>();
        bool _IsEmpty;
        string _TxtSearched;
        bool _IsBtnEnable = true;
        bool _IsListRefereshing = false;
        bool _ShowSearchBar = false;


        public bool ShowSearchBar
        {
            get { return _ShowSearchBar; }
            set { _ShowSearchBar = value; OnPropertyChanged("ShowSearchBar"); }
        }

        public string TxtSearch
        {
            get { return _TxtSearched; }
            set { _TxtSearched = value; OnPropertyChanged("TxtSearch"); if (string.IsNullOrEmpty(TxtSearch)) IsListRefereshing = true; }
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
                    GetQuestionList(0, 10);
                }
            }
        }

        public bool IsEmpty
        {
            get { return _IsEmpty; }
            set { _IsEmpty = value; OnPropertyChanged("IsEmpty"); }
        }

        public bool IsBtnEnable
        {
            get { return _IsBtnEnable; }
            set { _IsBtnEnable = value; OnPropertyChanged("IsBtnEnable"); }
        }

        public ObservableCollection<mQuestion> Questionlist
        {
            get { return _Questionlist; }
            set { _Questionlist = value; OnPropertyChanged("Questionlist"); OnPropertyChanged("QuestionlistRev"); }
        }

        public ObservableCollection<mQuestion> QuestionlistRev
        {
            get { return new ObservableCollection<mQuestion>(Questionlist.Reverse()); }

        }
        public ICommand SearchTxt { get; private set; }
        public ICommand AskQuestionCommand { get; private set; }

        private string Itemid { get; set; }
        private string Owner { get; set; }
        #endregion

        bool _isDone;
        bool _Desc_isOverLoad = false;
        bool _QuestionMax = false;
        public bool isDone
        {
            get { return _isDone; }
            set { _isDone = value; OnPropertyChanged("isDone"); }
        }

        public bool QuestionMax
        {
            get { return _QuestionMax; }
            set { _QuestionMax = value; OnPropertyChanged("QuestionMax"); }
        }


        public bool Desc_isOverLoad
        {
            get { return _Desc_isOverLoad; }
            set { _Desc_isOverLoad = value; OnPropertyChanged("Desc_isOverLoad"); }
        }

        ObservableCollection<mCarouselImage> _itemimages = new ObservableCollection<mCarouselImage>();
        string _name;
        string _description;
        string _price;
        string _bprice;
        string _ratings;
        string _status;
        string _quantity;
        string _state;
        bool _isdeal = false;
        string _dealprice;
        string _dealpercentage;
        string _manufacturer;
        string _statuscolor;
        string _cquantity = "1";
        int _CarouselPosition = 0;



        public ObservableCollection<mCarouselImage> ItemImages
        {
            get { return _itemimages; }
            set { _itemimages = value; OnPropertyChanged("ItemImages"); }
        }

        public string Name
        {
            get { return _name; }
            set { _name = value; OnPropertyChanged("Name"); }
        }

        public string Manufacturer
        {
            get { return _manufacturer; }
            set { _manufacturer = value; OnPropertyChanged("Manufacturer"); }
        }

        public bool IsDeal
        {
            get { return _isdeal; }
            set { _isdeal = value; OnPropertyChanged("IsDeal"); }
        }

        public string DealPrice
        {
            get { return _dealprice; }
            set { _dealprice = value; OnPropertyChanged("DealPrice"); }
        }

        public string DealPercentage
        {
            get { return _dealpercentage; }
            set { _dealpercentage = value; OnPropertyChanged("DealPercentage"); }
        }

        public string Description
        {
            get { return _description; }
            set { _description = value; OnPropertyChanged("Description"); }
        }
        public string Price
        {
            get { return _price; }
            set { _price = value; OnPropertyChanged("Price"); }
        }

        public string bPrice
        {
            get { return _bprice; }
            set { _bprice = value; OnPropertyChanged("bPrice"); }
        }

        public string Ratings
        {
            get { return _ratings; }
            set { _ratings = value; OnPropertyChanged("Ratings"); }
        }
        public string Status
        {
            get { return _status; }
            set { _status = value; OnPropertyChanged("Status"); }
        }

        public string StatusColor
        {
            get { return _statuscolor; }
            set { _statuscolor = value; OnPropertyChanged("StatusColor"); }
        }

        public string State
        {
            get { return _state; }
            set { _state = value; OnPropertyChanged("State"); }
        }
        public string Quantity
        {
            get { return _quantity; }
            set { _quantity = value; OnPropertyChanged("Quantity"); }
        }

        public string cQuantity
        {
            get { return _cquantity; }
            set { _cquantity = value; OnPropertyChanged("cQuantity"); }
        }

        public int CarouselPosition
        {
            get { return _CarouselPosition; }
            set { _CarouselPosition = value; OnPropertyChanged("CarouselPosition"); }
        }


        #endregion
        public ICommand AddToCartCommand { get; private set; }
        public ICommand DecQuantityCommand { get; private set; }
        public ICommand IncQuantityCommand { get; private set; }
        public ICommand SearchCommand { get; private set; }

        IPageService _pageservice;

        public Product_ProductView_ViewModel(Product product,IPageService pageService)
        {
            Itemid = product.Id;

            _pageservice = pageService;

            
            AddToCartCommand = new Command(async() => await AddToCartActionAsync(product));
            SearchCommand = new Command(() => { ShowSearchBar = !ShowSearchBar; });
            AskQuestionCommand = new Command(() => AskQuestion());
            DecQuantityCommand = new Command(() => QtyPicker("DEC"));
            IncQuantityCommand = new Command(() => QtyPicker("INC"));
            SearchTxt = new Command(txt => SearchTxtAction(txt));
            BindValues(product);
        }

        private void QtyPicker(string type)
        {
            if(type == "INC")
            {                                                                      
                if (Convert.ToInt16(cQuantity) >= Convert.ToInt16(Quantity))
                {
                    //iToast.iMessage("Maximum Quantity Reached");
                    DialogService.ShowToast("Maximum Quantity Reached");
                    return;
                }

                int temp = Convert.ToInt16(cQuantity) + 1;
                cQuantity = temp.ToString();
            }

            if(type == "DEC")
            {
                if (Convert.ToInt16(cQuantity) == 1) return;

                int temp = Convert.ToInt16(cQuantity) - 1;
                cQuantity = temp.ToString();
            }
        }

        private async Task AddToCartActionAsync(Product product)
        {
            if(AccountService.Instance.Current_Account == null)
            {
                AccountService.Instance.autho(null, "Dismiss");
                return; // Should stop catch error
            }

            if (IsBusy) return;

            IsBusy = true;


            try
            {
                DialogService.ShowLoading("Adding Item");

                if(product.Status == "Out Of Stock")
                {
                    DialogService.HideLoading();
                    DialogService.ShowErrorToast("Item Out Of Stock");
                    return;
                }
                mCartS cartItem = new mCartS()
                {
                    ProductId = product.Id,
                    Owner = AccountService.Instance.Current_Account.Email,
                    Quantity = cQuantity
                };

                var result = await CartService.Instance.Additem(cartItem);
                DialogService.HideLoading();
                if(result == "true")
                {
                    DialogService.ShowToast("Item Added To Cart");
                }
                else
                {
                    DialogService.ShowErrorToast("Failed To Add Item");

                }
            }
            catch (Exception ex)
            {
                DialogService.ShowError(Strings.SomethingWrong);
                Debug.WriteLine(Keys.TAG + ex);
            }
            finally { IsBusy = false; }
        }

        private void BindValues(Product item)
        {
            try
            {


                ObservableCollection<mCarouselImage> images = new ObservableCollection<mCarouselImage>();


                for (var i = 0; i < item.Images.Count; i++)
                {
                    images.Add(new mCarouselImage() { Image = item.Images[i] });
                }

                if(item.Status == "In Stock")
                {
                    StatusColor = "Green";
                }else if(item.Status == "Out Of Stock" || item.Status == "Unavailable")
                {
                    StatusColor = "Red";
                }

                if(item.PRating == null || item.PRating == "0")
                {
                    Ratings = "---";
                }

                if (item.IsDeal == "True")
                {
                    IsDeal = true;
                    Price = item.DealPrice;
                    bPrice = item.Price;
                    DealPercentage = item.DealPercentage;
                }
                else
                {
                    Price = item.Price;
                    IsDeal = false;
                }

                ItemImages = images;
                Name = item.Name;
                Ratings = item.PRating;
                State = item.State;
                Quantity = item.Quantity;
                Status = item.Status;
                Manufacturer = item.Manufacturer;

                int max_description_length = 550;

                if(item.Description.Length > max_description_length)
                {
                    Desc_isOverLoad = true;
                    Description = item.Description.Substring(0, max_description_length) + "...";
                }
                else
                {
                    Description = item.Description;
                }

            }
            catch (Exception ex)
            {
                Debug.WriteLine(Keys.TAG + ex);
            }

        }

        public async Task<ObservableCollection<mQuestion>> FetchQuestionAction(string txt, string Itemid, int len, int amount, bool addon = false)
        {
            if (IsBusy) return null;
            try
            {
                IsBusy = true;
                IsEmpty = false;

                ObservableCollection<mQuestion> result = new ObservableCollection<mQuestion>();
                string link = Keys.Url_Main + "item-question/get-question/" + Itemid;

                    result = await QuestionService.Instance.FetchQuestions(link, len, amount, txt);
                    if(result == null)
                    {
                        if (addon)
                        {
                            isDone = true;
                            return null;
                        }
                        IsEmpty = true;
                        QuestionMax = false;
                        return null;
                    }

                if (result == null)
                {
                    if (addon)
                    {
                        isDone = true;
                        return null;
                    }
                    IsEmpty = true;
                    QuestionMax = false;
                    return null;
                }

                if(result.Count >= 5)
                {
                    QuestionMax = true;
                }

                return result;


            }
            catch (Exception ex)
            {
                Debug.WriteLine(Keys.TAG + ex);
                DialogService.ShowError(Strings.SomethingWrong);
                return null;
            }
            finally { IsBusy = false; /*IsListRefereshing = false;*/ }
        }

        public async void UpdateQuestion(mQuestion question)
        {

            if (IsBusy || question.Asked_By != AccountService.Instance.Current_Account.Email) return;

            var result = await DialogService.ShowInputPrompt("Update", "Cancel", "Update Question", "", question.Question, InputType.Default);

            if (string.IsNullOrEmpty(result)) return;

            DialogService.ShowLoading();


            try
            {
                var updatedQuestion = new mQuestion() { Id = question.Id, Question = result };
                var callback = await QuestionService.Instance.EditQuestion(updatedQuestion);

                DialogService.HideLoading();

                if (callback == null) return;

                if (callback == "true")
                {
                    DialogService.ShowToast(Strings.QuestionUpdated);

                    for (var i = 0; i < QuestionlistRev.Count; i++)
                    {
                        if (QuestionlistRev[i] == question)
                        {
                            Questionlist[i].Question = result;//not working
                        }
                    }

                }

            }
            catch (Exception ex)
            {
                Debug.WriteLine(Keys.TAG + ex);
                DialogService.ShowError(Strings.SomethingWrong);
            }


        }


        public async void AskQuestion()
        {
            if (AccountService.Instance.Current_Account == null)
            {
                AccountService.Instance.autho(null, "Dismiss");
                return;
            }

            if (IsBusy || Owner == AccountService.Instance.Current_Account.Email) return;

            var result = await DialogService.ShowInputPrompt("Ask", "Cancel", "Ask Question", "", "", InputType.Default);

            if (string.IsNullOrEmpty(result)) return;

            DialogService.ShowLoading();

            try
            {

                mQuestion newQuestion = new mQuestion() { Question = result, ProductId = Itemid, Asked_By = AccountService.Instance.Current_Account.Email, Displayname = AccountService.Instance.Current_Account.Displayname };

                var callback = await QuestionService.Instance.PostQuestion(newQuestion);
                DialogService.HideLoading();

                if (result == null) return;

                if (callback == "true")
                {
                    DialogService.ShowToast("Question Asked");

                }

            }
            catch (Exception ex)
            {
                Debug.WriteLine(Keys.TAG + ex);
            }

        }

        public async void GetQuestionList(int len, int amount, bool addon = false, string txt = null)
        {
            if (IsBusy) return;

            IsBusy = true; IsEmpty = false;
            try
            {
                string link = Keys.Url_Main + "item-question/get-question/" + Itemid;
                if (txt != null)
                {
                    link += "?Search=" + txt;
                }


                ObservableCollection<mQuestion> result = new ObservableCollection<mQuestion>();
                if (CrossConnectivity.Current.IsConnected)
                {
                    result = await QuestionService.Instance.FetchQuestions(link, len, amount);

                    if (result == null)
                    {
                        if (addon)
                        {
                            isDone = true;
                            return;
                        }
                        IsEmpty = true;
                        return;
                    }
                    OfflineService.Write<ObservableCollection<mQuestion>>(result, Strings.ProductQuestion_Offline_fileName, null);
                }
                else
                {
                    result = await OfflineService.Read<ObservableCollection<mQuestion>>(Strings.ProductQuestion_Offline_fileName, null);

                }


                if (result == null)
                {
                    if (addon)
                    {
                        isDone = true;
                        return;
                    }
                    IsEmpty = true;
                    return;
                }

                if (addon)
                {
                    Questionlist.AddRange(result);

                }
                else
                {
                    Questionlist = result;
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(Keys.TAG + ex);
                DialogService.ShowError(Strings.SomethingWrong);
                return;
            }
            finally { IsBusy = false; IsListRefereshing = false; }
        }

        public async void SearchTxtAction(object txt)
        {

            var questions = await FetchQuestionAction(txt.ToString(), Itemid, 0, 100);

            if(questions == null)
            {
                Questionlist = new ObservableCollection<mQuestion>();
                return;
            }

            Questionlist = questions;

        }
    }
}
