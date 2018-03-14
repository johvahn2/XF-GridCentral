using GridCentral.Helpers;
using GridCentral.Models;
using GridCentral.Services;
using GridCentral.ViewModels;
using GridCentral.Views.Cart;
using GridCentral.Views.ObjectViews.Template;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace GridCentral.Views.ObjectViews
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ProductView : ContentPage
    {
        public Product _product = new Product();
        public ProductView(Product product)
        {
            _product = product;
            viewModel = new Product_ProductView_ViewModel(product, new PageService(Navigation));
            InitializeComponent(); 
            SetStrings();
            GetList(product);
            GoCartBtn.Clicked += GoCartBtn_Clicked;
            changeposit();
        }

        private void SetStrings()
        {
            AskBtn.Text = Strings.Ask;
            NoQtnLbl.Text = Strings.No_Questions;
            SeeMorelbl.Text = Strings.See_More;
            AddCartBtn.Text = Strings.Add_To_Cart;

            if (Device.OS == TargetPlatform.iOS)
            {
                GoCartBtn.Icon = "Cart-20x20.png";

            }
            else
            {
                GoCartBtn.Icon = "ic_shopping_cart_w.png";
            }
        }

        private async void GetList(Product product)
        {
            PopulateQuestionList(await viewModel.FetchQuestionAction(null, product.Id, 0, 5));
            getAds1();
        }

        private void GoCartBtn_Clicked(object sender, EventArgs e)
        {
            if (AccountService.Instance.Current_Account == null)
            {
                AccountService.Instance.autho(null, "Dismiss");
                return;
            }
            else
            {
                Navigation.PushAsync(new Connector());
            }
        }

        async void changeposit()
        {
            while (1 > 0)
            {
                for (var i = 0; i < viewModel.ItemImages.Count; i++)
                {
                    CarouselImgs.Position = i;
                    await Task.Delay(2500);
                }
            }
        }

        public void PopulateQuestionList(ObservableCollection<mQuestion> questions)
        {

            if (questions == null)
            {
                NoQtnLbl.IsVisible = true;
                return;
            }

            var productTapGestureRecognizer = new TapGestureRecognizer();
            productTapGestureRecognizer.Tapped += OnQuestionTapped;
            var column = QuestionList;

            for (var i = 0; i < questions.Count; i++)
            {
                if (i == 5) break;

                var item = new ItemQuestionTemplate();

                item.BindingContext = questions[i];
                item.GestureRecognizers.Add(productTapGestureRecognizer);
                column.Children.Add(item);

            }
        }

        private void OnQuestionTapped(object sender, EventArgs e)
        {
            if (AccountService.Instance.Current_Account == null)
            {
                return;
            }

            var selectedItem = (mQuestion)((ItemQuestionTemplate)sender).BindingContext;

            if(selectedItem.Asked_By == AccountService.Instance.Current_Account.Email)
            {
                if (String.IsNullOrEmpty(selectedItem.Answer_By) && selectedItem.Answer == "*No Answer Yet*")
                {
                    viewModel.UpdateQuestion(selectedItem);
                }
            }
        }

        private void Question_ViewMore(object sender, EventArgs e)
        {
          Navigation.PushAsync(new pQuestion(_product));
        }

        public Product_ProductView_ViewModel viewModel
        {
            get { return BindingContext as Product_ProductView_ViewModel; }
            set { BindingContext = value; }
        }

        private void ImageTapped(object sender, EventArgs e)
        {
            Navigation.PushModalAsync(new ImageView(viewModel.ItemImages));
        }


        #region Get Ads
        int ad1 = 0;
        private async void getAds1()
        {
            try
            {
                var result = await AdService.Instance.FetchAds("productview-Feature");
                if (result == null) return;
                ad1 = result.Count;
                ObservableCollection<mCarouselImage> car = new ObservableCollection<mCarouselImage>();
                for (var i = 0; i < result.Count; i++)
                {
                    if (result[i].Show == "true")
                    {
                        car.Add(new mCarouselImage() { Image = result[i].Image, Description = result[i].Description });
                    }
                    else
                    {
                        ad1--;
                    }

                }
                CarouselImages1.ItemsSource = car;
                changeposit1();

            }
            catch (Exception ex)
            {
                Debug.WriteLine(Keys.TAG + ex);
            }
        }

        async void changeposit1()
        {
            if (ad1 < 2) return;

            while (1 > 0)
            {
                for (var i = 0; i < ad1; i++)
                {
                    CarouselImages1.Position = i;
                    await Task.Delay(2500);
                }
            }
        }
        #endregion

        private void ReadMore_Tapped(object sender, EventArgs e)
        {
            if (!viewModel.Desc_isOverLoad) return;

            Navigation.PushModalAsync(new DescriptionView(_product.Description));

        }
    }
}
