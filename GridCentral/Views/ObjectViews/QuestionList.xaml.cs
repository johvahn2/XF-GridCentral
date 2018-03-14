using GridCentral.Models;
using GridCentral.Services;
using GridCentral.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace GridCentral.Views.ObjectViews
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class QuestionList : ContentPage
    {
        private mUserItem Item { get; set; }
        public QuestionList(mUserItem item)
        {
            Item = item;

            viewModel = new BuySell_ItemView_ViewModel(new PageService(Navigation),item,true);
            InitializeComponent();
            //viewModel.GetQuestionList(0, 10);
            viewModel.IsListRefereshing = true;

            listView.ItemSelected += ListView_ItemSelected;

            listView.ItemAppearing += (sender, e) =>
            {
                if (viewModel.IsBusy || viewModel.Questionlist.Count < 4 || viewModel.isDone) return;

                if (e.Item == viewModel.Questionlist[viewModel.Questionlist.Count - 1])
                {
                    viewModel.GetQuestionList(viewModel.Questionlist.Count, 10,true);// add loader for viewmore on view
                }
            };
        }

        private void ListView_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            var item = e.SelectedItem as mQuestion;

            if (Item.Manufacturer == AccountService.Instance.Current_Account.Email)
            {
                if (item != null)
                {
                    if (item.Answer_By == AccountService.Instance.Current_Account.Email)
                    {
                        if (String.IsNullOrEmpty(item.Answer_By) && item.Answer == "*No Answer Yet*")
                        {
                            viewModel.UpdateAnswer(item);
                        }

                    }
                    else
                    {
                        viewModel.AnswerQuestion(item);
                    }

                    listView.SelectedItem = null;

                    return;
                }
            }

            if (item != null)
            {
                if (item.Asked_By == AccountService.Instance.Current_Account.Email)
                {
                    if (String.IsNullOrEmpty(item.Answer_By))
                    {
                        viewModel.UpdateQuestion(item);
                        listView.SelectedItem = null;

                    }

                    return;

                }
            }
        }

        public BuySell_ItemView_ViewModel viewModel
        {
            get { return BindingContext as BuySell_ItemView_ViewModel; }
            set { BindingContext = value; }
        }


    }
}
