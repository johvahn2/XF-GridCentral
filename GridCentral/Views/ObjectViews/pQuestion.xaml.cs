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
    public partial class pQuestion : ContentPage
    {

        Product _product;
        public pQuestion(Product product)
        {
            _product = product;
            viewModel = new Product_ProductView_ViewModel(product, new PageService(Navigation));
            InitializeComponent();

            viewModel.IsListRefereshing = true;

            listView.ItemSelected += ListView_ItemSelected;

            listView.ItemAppearing += (sender, e) =>
            {
                if (viewModel.IsBusy || viewModel.Questionlist.Count < 4 || viewModel.isDone) return;

                if (e.Item == viewModel.Questionlist[viewModel.Questionlist.Count - 1])
                {
                    viewModel.GetQuestionList(viewModel.Questionlist.Count, 10, true);// add loader for viewmore on view
                }
            };
        }

        private void ListView_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            var item = e.SelectedItem as mQuestion;

            if (item != null)
            {
                if (item.Asked_By == AccountService.Instance.Current_Account.Email)
                {
                    if(String.IsNullOrEmpty(item.Answer_By) && item.Answer == "*No Answer Yet*")
                    {
                        viewModel.UpdateQuestion(item);
                        listView.SelectedItem = null;

                    }


                    return;

                }
            }
        }

        public Product_ProductView_ViewModel viewModel
        {
            get { return BindingContext as Product_ProductView_ViewModel; }
            set { BindingContext = value; }
        }
    }
}
