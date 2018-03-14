using GridCentral.Helpers;
using GridCentral.Services;
using GridCentral.Views.Cart;
using GridCentral.Views.Search.Template;
using GridCentral.Views.Settings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace GridCentral.Views.Search
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CategoryList : ContentPage
    {
        public CategoryList()
        {
            InitializeComponent();

            if(AccountService.Instance.Current_Account != null)
            {
                PopulateCategories(Keys.ItemCategories);

                if (AccountService.Instance.Current_Account.Interests == null || AccountService.Instance.Current_Account.Interests.Count < 1)
                {
                    Checkinteret();
                    return;
                }

                PopulateMyInterests(AccountService.Instance.Current_Account.Interests);


            }

        }

        async void Checkinteret()
        {
            var res = await DisplayAlert("Interests", "you have no interests selected", "Add", "Dismiss");

            if (res)
            {
               await Navigation.PushAsync(new Edit_Interests());
            }

        }

        public void PopulateMyInterests(List<string> Ninterests)
        {
            List<mCats> interests = new List<mCats>();

            for(var i=0; i < Ninterests.Count; i++)
            {
                if (Ninterests[i] == "Health_Beauty")
                {
                    //Ninterests.Remove("Health_Beauty");
                    Ninterests.RemoveAt(i);
                }
                interests.Add(new mCats { Name = Keys.CatItemsRev[Ninterests[i]] });
            }
            var productTapGestureRecognizer = new TapGestureRecognizer();
            productTapGestureRecognizer.Tapped += OnTapped;
            var column = myIntrests;

            for (var i = 0; i < interests.Count; i++)
            {
                var item = new CategoryListTemplate();

                item.BindingContext = interests[i];
                item.GestureRecognizers.Add(productTapGestureRecognizer);
                column.Children.Add(item);

            }


        }


        public void PopulateCategories(List<string> categories)
        {
            List<mCats> cats = new List<mCats>();
            for (var i = 0; i < categories.Count; i++)
            {
                cats.Add(new mCats { Name = categories[i] });
            }
            var productTapGestureRecognizer = new TapGestureRecognizer();
            productTapGestureRecognizer.Tapped += OnTapped;
            var column = allCategories;

            for (var i = 0; i < cats.Count; i++)
            {
                var item = new CategoryListTemplate();

                item.BindingContext = cats[i];
                item.GestureRecognizers.Add(productTapGestureRecognizer);
                column.Children.Add(item);

            }


        }
        private void OnTapped(object sender, EventArgs e)
        {
            var selectedItem = (mCats)((CategoryListTemplate)sender).BindingContext;

            Navigation.PushAsync(new ProductSearch(null, selectedItem.Name));//here
        }

        private void ToolbarItem_Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new Connector());
        }
    }

    public class mCats
    {
        public string Name { get; set; }
    }
}