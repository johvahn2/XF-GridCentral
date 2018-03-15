using GridCentral.Helpers;
using GridCentral.Interfaces;
using GridCentral.Models;
using GridCentral.Services;
using GridCentral.Views.ObjectViews;
using GridCentral.Views.Rate;
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
    public class Notifaction_Notify_ViewModel : Base_ViewModel
    {

        #region Bind Property

        ObservableCollection<mNotify> _NotifyList = new ObservableCollection<mNotify>();

        bool _IsRefresing;
        bool _IsEmpty;


        public ObservableCollection<mNotify> NotifyList
        {
            get { return _NotifyList; }
            set { _NotifyList = value; OnPropertyChanged("NotifyList"); OnPropertyChanged("NotifyListRev"); }
        }

        public ObservableCollection<mNotify> NotifyListRev
        {
            get { return new ObservableCollection<mNotify>(NotifyList.Reverse()); }
        }

        public bool IsEmpty
        {
            get { return _IsEmpty; }
            set { _IsEmpty = value; OnPropertyChanged("IsEmpty"); }
        }

        public bool IsRefresing
        {
            get { return _IsRefresing; }
            set { _IsRefresing = value; OnPropertyChanged("IsRefresing"); if (IsRefresing) GetMyNotfiy(AccountService.Instance.Current_Account.Email); }
        }


        public ICommand ClearCommand { get; private set; }
        #endregion
        private bool QuestionAnswered = false;

        IPageService _pageService;
        public Notifaction_Notify_ViewModel(IPageService pageService)
        {
            IsRefresing = true;

            ClearCommand = new Command(() => ClearNotfiy());
        }

        private async void ClearNotfiy()
        {
            if(NotifyList.Count < 1)
            {
                DialogService.ShowToast(Strings.No_Item_To_Clear);
                return;
            }

            var verify = await DialogService.DisplayAlert(Strings.Yes, Strings.No, Strings.Clear_List,Strings.Are_You_Sure_To_Clear);

            if (!verify) return;

            try
            {

                var result = await NotifyService.Instance.DeleteNotify(AccountService.Instance.Current_Account.Email);

                if (!result) return;

                NotifyList = new ObservableCollection<mNotify>();

                DialogService.ShowToast(Strings.List_Cleared);

            }
            catch (Exception ex)
            {
                Debug.WriteLine(Keys.TAG + ex);
                DialogService.ShowError(Strings.SomethingWrong);
                Crashes.TrackError(ex);
            }
        }

        private async void GetMyNotfiy(string email)
        {
            if (IsBusy) return;


            try
            {
                IsBusy = true;
                IsEmpty = false;

                if (CrossConnectivity.Current.IsConnected)
                {
                    var result = await NotifyService.Instance.FetchNotfiy(email);

                    if (result == null)
                    {
                        IsEmpty = true;
                        NotifyList = new ObservableCollection<mNotify>();
                        return;
                    }

                    NotifyList = formatData(result);

                    OfflineService.Write<ObservableCollection<mNotify>>(result,Strings.Notify_Offline_fileName, null);

                }
                else
                {
                    var result = await OfflineService.Read<ObservableCollection<mNotify>>(Strings.Notify_Offline_fileName, null);

                    if (result == null)
                    {
                        IsEmpty = true;
                        NotifyList = new ObservableCollection<mNotify>();
                        return;
                    }

                    NotifyList = formatData(result);

                }

            }
            catch (Exception ex)
            {
                Debug.WriteLine(Keys.TAG + ex);
                DialogService.ShowError(Strings.SomethingWrong);
                Crashes.TrackError(ex);
            }
            finally { IsBusy = false; IsRefresing = false; }
        }

        private ObservableCollection<mNotify> formatData(ObservableCollection<mNotify> result)
        {
            for (int i = 0; i < result.Count; i++)
            {
                if(result[i].State == "good")
                {
                    result[i].IconBg = Strings.Green;
                    result[i].Icon = GrialShapesFont.Check;

                }
                else if(result[i].State == "warning")
                {
                    result[i].IconBg = Strings.Yellow;
                    result[i].Icon = GrialShapesFont.Warning;

                }
                else if(result[i].State == "bad")
                {
                    result[i].IconBg = Strings.Red;
                    result[i].Icon = GrialShapesFont.Close;

                }
                else if(result[i].State == "info")
                {
                    result[i].IconBg = Strings.Blue;
                    result[i].Icon = GrialShapesFont.Notifications;
                }
            }

            return result;
        }


        public async Task NotifyAction(mNotify item)
        {
            if (item.Why == Keys.NotifyWhys[1])// new-question
            {

                mQuestion question = await QuestionService.Instance.FetchQuestion(item.Objecter);

                if (question == null || question.Answer != Strings.No_Answer || QuestionAnswered)
                    return;

                var result = await DialogService.DisplayAlert(Strings.Answer, Strings.Close, Strings.Answer_Question,question.Question);

                if (!result) return;

                var answer = await DialogService.ShowInputPrompt(Strings.Answer, Strings.Close, Strings.Answer_Question, "Q: " + question.Question, "", Acr.UserDialogs.InputType.Default);

                if (string.IsNullOrEmpty(answer)) return;

                AnswerQuestion(answer,question);


            }
        }



        private async void AnswerQuestion(string answer, mQuestion question)
        {
            DialogService.ShowLoading();

            mQuestion newAnswer = new mQuestion() { Answer = answer, Id = question.Id, Answer_By = AccountService.Instance.Current_Account.Email, ProductId = question.ProductId };

            var callback = await QuestionService.Instance.AnswerQuestion(newAnswer);
            DialogService.HideLoading();

            if (callback == "true")
            {
                DialogService.ShowToast(Strings.Answer_Question);
                QuestionAnswered = true;
            }
        }
    }
}
