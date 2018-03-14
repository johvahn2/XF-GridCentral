using GridCentral.Helpers;
using GridCentral.Models;
using GridCentral.Services;
using GridCentral.ViewModels;
using GridCentral.Views.Navigation;
using GridCentral.Views.ObjectViews;
using GridCentral.Views.Order;
using GridCentral.Views.Rate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace GridCentral.Views.Notifaction
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Notify : ContentPage
    {
        public Notify()
        {
            if (AccountService.Instance.Current_Account == null)
            {
                AccountService.Instance.autho(new RootPage(false));
                return;
            }

            viewModel = new Notifaction_Notify_ViewModel(new PageService(Navigation));
            InitializeComponent();
            BindIcons();
            SetStrings();

            listView.ItemSelected += ListView_ItemSelected;
        }

        private void BindIcons()
        {
            if (Device.OS == TargetPlatform.iOS)
            {
                broomIcon.Icon = "broom-20x20.png";
            }
            else
            {
                broomIcon.Icon = "ic_broom.png";
            }
        }

        private void SetStrings()
        {
            Emptylbl.Text = Strings.Empty;
        }

        private async void ListView_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            var item = e.SelectedItem as mNotify;

            if( item != null)
            {
                if (item.Why == Keys.NotifyWhys[9])//rate
                {
                    DialogService.ShowLoading("Please wait");
                    mOrder order = await OrderService.Instance.FetchOrder(item.Objecter);
                    DialogService.HideLoading();

                    if (order == null) return;

                    await Navigation.PushAsync(new RateList(order));


                } else if(item.Type == Keys.NotifyTypes[2])//order
                {
                    DialogService.ShowLoading("Please wait");
                    mOrder order = await OrderService.Instance.FetchOrder(item.Objecter);
                    DialogService.HideLoading();

                    if (order == null) return;

                    await Navigation.PushAsync(new OrderDetail(order));
                }
                else if (item.Why == Keys.NotifyWhys[2])//answer-question
                {
                    mQuestion question2 = await QuestionService.Instance.FetchQuestion(item.Objecter);

                    var response = await DialogService.DisplayAlert("View item", "Dismiss", "Q:" + question2.Question, "A:" + question2.Answer);

                    listView.SelectedItem = null;
                    if (!response) return;

                    DialogService.ShowLoading("Relocating");
                    if (question2.IsItem == "True")
                    {
                        var Titem = await ItemService.Instance.FetchItem(question2.ProductId);
                        DialogService.HideLoading();

                        await Navigation.PushAsync(new ItemView(Titem));

                    }
                    else
                    {
                        var Tproduct = await ProductService.Instance.FetchProduct(question2.ProductId);
                        DialogService.HideLoading();

                        await Navigation.PushAsync(new ProductView(Tproduct));
                    }
                }
                else
                {
                    viewModel.NotifyAction(item);
                }
            }

            listView.SelectedItem = null;
        }

        public Notifaction_Notify_ViewModel viewModel
        {
            get { return BindingContext as Notifaction_Notify_ViewModel; }
            set { BindingContext = value; }
        }
    }
}