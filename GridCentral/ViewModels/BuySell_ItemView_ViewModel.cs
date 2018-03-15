using Acr.UserDialogs;
using GridCentral.Helpers;
using GridCentral.Interfaces;
using GridCentral.Models;
using GridCentral.Services;
using GridCentral.Views.ObjectViews;
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
    public class BuySell_ItemView_ViewModel : Base_ViewModel
    {
        #region Bind Property
        #region Question Property

        ObservableCollection<mQuestion> _Questionlist = new ObservableCollection<mQuestion>();
        bool _IsEmpty;
        string _TxtSearched;
        bool _IsBtnEnable = true;
        bool _IsListRefereshing = false;
        bool _QuestionMax = false;

        public string TxtSearch
        {
            get { return _TxtSearched; }
            set { _TxtSearched = value; OnPropertyChanged("TxtSearch"); if (string.IsNullOrEmpty(TxtSearch)) IsListRefereshing=true; }
        }

        public bool QuestionMax
        {
            get { return _QuestionMax; }
            set { _QuestionMax = value; OnPropertyChanged("QuestionMax"); }
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

        ObservableCollection<mCarouselImage> _itemimages = new ObservableCollection<mCarouselImage>();
        string _name;
        string _description;
        string _price;
        string _status;
        string _quantity;
        string _state;
        string _statecolor;
        string _createdat;
        string _createdby;
        int _CarouselPosition = 0;
        bool _ShowSearchBar = false;
        bool _isDone;
        bool _Desc_isOverLoad = false;

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

        public string CreatedBy
        {
            get { return _createdby; }
            set { _createdby = value; OnPropertyChanged("CreatedBy"); }
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
        public string Status
        {
            get { return _status; }
            set { _status = value; OnPropertyChanged("Status"); }
        }

        public string StateColor
        {
            get { return _statecolor; }
            set { _statecolor = value; OnPropertyChanged("StateColor"); }
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


        public int CarouselPosition
        {
            get { return _CarouselPosition; }
            set { _CarouselPosition = value; OnPropertyChanged("CarouselPosition"); }
        }

        public bool ShowSearchBar
        {
            get { return _ShowSearchBar; }
            set { _ShowSearchBar = value; OnPropertyChanged("ShowSearchBar"); }
        }

        public bool isDone
        {
            get { return _isDone; }
            set { _isDone = value; OnPropertyChanged("isDone"); }
        }

        public bool Desc_isOverLoad
        {
            get { return _Desc_isOverLoad; }
            set { _Desc_isOverLoad = value; OnPropertyChanged("Desc_isOverLoad"); }
        }

        public ICommand SearchCommand { get; private set; }
        public ICommand ContactCommand { get; private set; }
        #endregion

        IPageService _pageService;

        public BuySell_ItemView_ViewModel(IPageService pageSerivce,mUserItem item,bool isList=false)
        {
            _pageService = pageSerivce;

            if(!isList)
                BindContent(item);

            if(AccountService.Instance.Current_Account != null)
            {
                if (item.Manufacturer == AccountService.Instance.Current_Account.Email)
                    IsBtnEnable = false;
            }
            Itemid = item.Id;
            Owner = item.Manufacturer;


            SearchTxt = new Command(txt => SearchTxtAction(txt));
            SearchCommand = new Command(() => { ShowSearchBar = !ShowSearchBar; });
            AskQuestionCommand = new Command(() => AskQuestion());
            ContactCommand = new Command(() => ContactSeller());

        }

        private async void ContactSeller()
        {
            if (IsBusy) return;

            IsBusy = true;

            try
            {
                DialogService.ShowLoading("Fetching Seller's Info");

                mAccount Seller = await UserService.Instance.FetchUser(Owner);
                DialogService.HideLoading();

                if (Seller == null) return;

               await _pageService.PushModalAsync(new ContactSeller(Seller));
                

            }catch(Exception ex)
            {
                Debug.WriteLine(Keys.TAG + ex);
                DialogService.ShowError(Strings.SomethingWrong);
                Crashes.TrackError(ex);
            }
            finally { IsBusy = false; }
        }

        public async void GetQuestionList(int len, int amount,bool addon= false,string txt= null)
        {
            if (IsBusy) return;

            IsBusy = true;IsEmpty = false;
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
                    OfflineService.Write<ObservableCollection<mQuestion>>(result, Strings.ItemQuestion_Offline_fileName, null);
                }
                else
                {
                    result = await OfflineService.Read<ObservableCollection<mQuestion>>(Strings.ItemQuestion_Offline_fileName, null);

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
                Crashes.TrackError(ex);
                return;
            }
            finally { IsBusy = false; IsListRefereshing = false; }
        }

        private void BindContent(mUserItem item)
        {
            try
            {

                ObservableCollection<mCarouselImage> images = new ObservableCollection<mCarouselImage>();


                for (var i = 0; i < item.Images.Count; i++)
                {
                    images.Add(new mCarouselImage() { Image = item.Images[i] });
                }

                if (item.State == "New")
                {
                    StateColor = "Green";
                }
                else
                {
                    StateColor = "Red";
                }

                ItemImages = images;
                Name = item.Name;
                Price = item.Price;
                CreatedBy = item.Displayname;
                State = item.State;               
                Quantity = item.Quantity;
                Status = item.Status;

                int max_description_length = 550;

                if (item.Description.Length > max_description_length)
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

        public async void SearchTxtAction(object txt)
        {

            var questions =  await FetchQuestionAction(txt.ToString(), Itemid, 0, 100);

            if (questions == null)
            {
                Questionlist = new ObservableCollection<mQuestion>();
                return;
            }
            Questionlist = questions;
            
        }


        public async Task<ObservableCollection<mQuestion>> FetchQuestionAction(string txt, string Itemid,int len, int amount,bool addon=false)
        {
            if (IsBusy) return null;
            try
            {
                IsBusy = true;
                IsEmpty = false;

                string link = Keys.Url_Main + "item-question/get-question/" + Itemid;
                //if (txt != null)
                //{
                //    link += "?Search=" + txt;
                //}

                var result = await QuestionService.Instance.FetchQuestions(link,len,amount,txt);

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
                Crashes.TrackError(ex);
                return null;
            }
            finally { IsBusy = false;IsListRefereshing = false; }
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
                Crashes.TrackError(ex);
            }


        }

        public async void UpdateAnswer(mQuestion question)
        {
            if (IsBusy || question.Answer_By != AccountService.Instance.Current_Account.Email) return;

            var result = await DialogService.ShowInputPrompt("Update", "Cancel", "Update Answer", "", question.Answer, InputType.Default);

            if (string.IsNullOrEmpty(result)) return;

            DialogService.ShowLoading();


            try
            {
                var updatedQuestion = new mQuestion() { Id = question.Id, Answer = result };
                var callback = await QuestionService.Instance.EditAnswer(updatedQuestion);

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
                Crashes.TrackError(ex);
            }


        }

        public async void AnswerQuestion(mQuestion question)
        {
            if (IsBusy || Owner != AccountService.Instance.Current_Account.Email) return;

            var result = await DialogService.ShowInputPrompt("Answer", "Cancel", "Answer Question", question.Question, "", InputType.Default);

            if (string.IsNullOrEmpty(result)) return;

            DialogService.ShowLoading();

            try
            {

                mQuestion newAnswer = new mQuestion() { Answer = result, Id = question.Id, Answer_By = AccountService.Instance.Current_Account.Email, ProductId = question.ProductId };

                var callback = await QuestionService.Instance.AnswerQuestion(newAnswer);
                DialogService.HideLoading();

                if (result == null) return;

                if (callback == "true")
                {
                    DialogService.ShowToast(Strings.QuestionAnswered);

                    for (var i = 0; i < Questionlist.Count; i++)
                    {
                        if (Questionlist[i] == question)
                        {
                            Questionlist[i].Answer = result;
                        }
                    }

                }

            }
            catch (Exception ex)
            {
                Debug.WriteLine(Keys.TAG + ex);
                Crashes.TrackError(ex);
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

                mQuestion newQuestion = new mQuestion() { Question = result, ProductId = Itemid ,Asked_By = AccountService.Instance.Current_Account.Email, IsItem = "True", Displayname = AccountService.Instance.Current_Account.Displayname };

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
                Crashes.TrackError(ex);
            }

        }


    }
}
