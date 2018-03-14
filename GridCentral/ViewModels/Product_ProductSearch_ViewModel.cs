using GridCentral.Helpers;
using GridCentral.Models;
using GridCentral.Services;
using GridCentral.Views.Navigation;
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
    public class Product_ProductSearch_ViewModel : Base_ViewModel
    {

        #region Bind Property

        ObservableCollection<Product> _MyProductList = new ObservableCollection<Product>();

        public ObservableCollection<Product> ProductList
        {
            get { return _MyProductList; }
            set
            {
                _MyProductList = value;
                OnPropertyChanged("ProductList");
            }
        }

        public List<string> _CategoryItems = new List<string>() {"All","Appliances","Art","Baby","Books","Cars","Clothing","Electronics","Furniture","Hair","Home Supplies", "Personal Care", "Jewelry", "Makeup Beauty","Other","Toys Games"};
       public Dictionary<string, string> CatItems = new Dictionary<string, string>()
        {
            {"All","All"},{ "Appliances","Appliances"},{"Art","Art"},{"Baby","Baby"},{"Books","Books"},{"Cars","Cars"},{"Clothing","Clothing"},{"Electronics","Electronics"},{"Furniture","Furniture"},{"Hair","Hair"},{ "Home Supplies","Home_Supplies"},
           { "Personal Care","Personal_Care"},{ "Jewelry","Jewelry"},{ "Makeup Beauty","Makeup_Beauty"},{ "Other","Other"},{ "Toys Games","Toys_Games"}
        };
        public List<string> CategoryItems
        {
            get { return _CategoryItems; }
            set { _CategoryItems = value; OnPropertyChanged("CategoryItems"); }
        }

        int _categoryindex;
        public int CategoryIndex
        {
            get { return _categoryindex; }
            set {
                _categoryindex = value; OnPropertyChanged("CategoryIndex");
                catgeroyTempo = CategoryItems[CategoryIndex];
                string catgeroyTemp = catgeroyTempo;

                if (CatItems.ContainsKey(catgeroyTempo))
                {
                    catgeroyTemp = CatItems[catgeroyTempo];
                }
                if (String.IsNullOrEmpty(SearchTxt))
                {
                    if (CategoryIndex == 0)
                        GetRandomProducts(0, 10, false);
                    else
                        GetCategoryItems(catgeroyTemp, 0, 10,false);
                }
                else
                {
                    GetSearch(SearchTxt, catgeroyTemp, 10, 0,false);
                }
            }
        }


        static public string catgeroyTempo;
        bool _noItems;
        ObservableCollection<mSearchProduct> _SearchList = new ObservableCollection<mSearchProduct>();

        public bool isDone = false;
        public bool noItems
        {
            get { return _noItems; }
            set { _noItems = value; OnPropertyChanged("noItems"); }
        }

        public ObservableCollection<mSearchProduct> SearchList
        {
            get { return _SearchList; }
            set { _SearchList = value; OnPropertyChanged("SearchList"); }
        }

        string _searchtxt;
        public string SearchTxt
        {
            get { return _searchtxt; }
            set { _searchtxt = value; OnPropertyChanged("SearchTxt"); }
        }
        #endregion
        public static bool isRandom = false;
        public Product_ProductSearch_ViewModel(string txtSearched, string category)
        {
            isRandom = false;
            SearchTxt = txtSearched;
            checker(txtSearched,category,10,0);
            #region MyRegion

            //isRandom = false;

            //if (txtSearched != null)
            //{
            //    SearchTxt = txtSearched;

            //    if (category == null)
            //    {
            //        GetSearchAsync(SearchTxt, CategoryItems[0],10,0);

            //    }
            //    else
            //    {
            //        CategoryIndex = CategoryItems.IndexOf(category);
            //        //GetSearchAsync(SearchTxt, category);
            //    }

            //}
            //else
            //{
            //    isRandom = true;
            //    CategoryIndex = CategoryItems.IndexOf(category);
            //    GetCategoryItemsAsync(category,0,10);
            //}
            #endregion

        }

        public async void GetCategoryItems(string category, int len,int amount,bool addon)
        {
            if (IsBusy) return;

            IsBusy = true; noItems = false;

            try
            {
                var result = await ProductService.Instance.FetchCategory(category, amount,len);

                if (result == null)
                {
                    if (addon)
                    {
                        isDone = true;
                    }
                    else
                    {
                        noItems = true;
                        SearchList = new ObservableCollection<mSearchProduct>();
                    }
                    return;
                }
                if (addon)
                {
                    ProductList.AddRange(result);
                    SearchList.AddRange(formData(result));
                }
                else
                {
                    ProductList = result;
                    SearchList = formData(result);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(Keys.TAG + ex);
                DialogService.ShowError(Strings.SomethingWrong);
            }
            finally { IsBusy = false; }
        }

        public async void GetSearch(string txtSearched, string category,int amount,int len,bool addon)
        {
            if (IsBusy || txtSearched.Length < 1) return;

            IsBusy = true; noItems = false;

            try
            {
                string link;

                if (category != null)
                {
                    if(category != CategoryItems[0])
                        link = Keys.Url_Main + "product/search/" + txtSearched + "?Category=" + category + "&amount=" + amount.ToString() + "&len=" + len.ToString();
                    else
                        link = Keys.Url_Main + "product/search/" + txtSearched + "?amount=" + amount.ToString() + "&len=" + len.ToString();

                }
                else
                {

                    link = Keys.Url_Main + "product/search/" + txtSearched + "?amount=" + amount.ToString() + "&len=" + len.ToString();
                }


                var result = await ProductService.Instance.FetchSearch(link);

                if (result == null) {
                    if (addon)
                    {
                        isDone = true;
                    }
                    else
                    {
                        noItems = true;
                        SearchList = new ObservableCollection<mSearchProduct>();
                    }
                    return;

                };
                
                if (addon)
                {
                    SearchList.AddRange(formData(result));
                    ProductList.AddRange(result);
                }
                else
                {
                    SearchList = formData(result);
                    ProductList = result;
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(Keys.TAG + ex);
                DialogService.ShowError(Strings.SomethingWrong);
            }
            finally { IsBusy = false; }
        }


        public async void GetRandomProducts(int len, int amount, bool addon)
        {
            if (IsBusy) return;

            IsBusy = true;


            try
            {

                var result = await ProductService.Instance.FetchRandom(amount, null, null,len);


                if (result == null)
                {
                    if (addon)
                    {
                        isDone = true;
                    }
                    else
                    {
                        noItems = true;
                        SearchList = new ObservableCollection<mSearchProduct>();
                    }
                    return;
                }
                if (addon)
                {
                    ProductList.AddRange(result);
                    SearchList.AddRange(formData(result));
                }
                else
                {
                    ProductList = result;
                    SearchList = formData(result);
                }

            }
            catch (Exception ex)
            {
                Debug.WriteLine(Keys.TAG + ex);
                DialogService.ShowError(Strings.SomethingWrong);
                return;
            }
            finally { IsBusy = false; }
        }

        private ObservableCollection<mSearchProduct> formData(ObservableCollection<Product> result)
        {
            ObservableCollection<mSearchProduct> search = new ObservableCollection<mSearchProduct>();
            for (var i = 0; i < result.Count; i++)
            {
                search.Add(new mSearchProduct
                {
                    Id = result[i].Id,
                    Price = result[i].Price,
                    Status = result[i].Status,
                    PRating = result[i].PRating,
                    Manufacturer = result[i].Manufacturer,
                    Thumbnail = result[i].Images[0],
                    Rating = "0%"
                });

                if(result[i].PRating == null)
                {
                    search[i].Rating = "---";
                }

                int max_description_length = 95;
                int max_Name_Length = 21;

                if (result[i].Description.Length > max_description_length)
                {
                    search[i].Description = result[i].Description.Substring(0, max_description_length) + "...";
                }
                else
                {
                    search[i].Description = result[i].Description;
                }

                search[i].bName = result[i].Name;

                if (result[i].Name.Length > max_Name_Length)
                {
                    search[i].Name = result[i].Name.Substring(0, max_Name_Length) + "...";
                }
                else
                {
                    search[i].Name = result[i].Name;
                }

                if (result[i].Status == "In Stock")
                {
                    search[i].StatusColor = "Green";
                }
                else
                {
                    search[i].StatusColor = "Red";
                }

                search[i].SaveCommand = new Command(async itemName => await SaveAction(itemName));

            }
            return search;
        }

        private async Task SaveAction(object itemName)
        {
            try
            {
                if(AccountService.Instance.Current_Account == null)
                {
                    AccountService.Instance.autho(null,"Dismiss");
                    return;
                }

                DialogService.ShowLoading("Saving Item");

                mSearchProduct listitem = (from itm in SearchList
                                  where itm.Name == itemName.ToString()
                                  select itm)
                                        .FirstOrDefault<mSearchProduct>();

                mSavelater item = new mSavelater()
                {
                    Owner = AccountService.Instance.Current_Account.Email,
                    ProductId = listitem.Id
                };
                var result = await SavelaterService.Instance.Additem(item);
                DialogService.HideLoading();

                if (result == "true")
                {
                    DialogService.ShowToast("Item Saved");
                    return;
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
            }
        }

        public void checker(string searchTxt, string category,int amount,int len)
        {
            if (String.IsNullOrEmpty(searchTxt) && String.IsNullOrEmpty(category)) return;

            if (!String.IsNullOrEmpty(searchTxt) && !String.IsNullOrEmpty(category))//both not empty
            {
                CategoryIndex = CategoryItems.IndexOf(category);
                //GetSearch(searchTxt, category, amount, len,false);
            }
            else if (!String.IsNullOrEmpty(searchTxt) && String.IsNullOrEmpty(category))//category empty
            {
                CategoryIndex = 0;
                //GetSearch(searchTxt, category, amount, len);

            }
            else if (String.IsNullOrEmpty(searchTxt) && !String.IsNullOrEmpty(category))//search text empty
            {
                isRandom = true;
                if(category == CategoryItems[0])
                {
                    GetRandomProducts(0, 10, false);
                }
                else
                {
                    CategoryIndex = CategoryItems.IndexOf(category);

                }
                //GetCategoryItems(category, len, amount);
            }
        }
    }

    public class mSearchProduct
    {

        public ICommand SaveCommand { get; set; }

        public string Id
        {
            get;
            set;
        }

        public string Name
        {
            get;
            set;
        }

        public string bName
        {
            get;
            set;
        }

        public string Price
        {
            get;
            set;
        }

        public string Thumbnail
        {
            get;
            set;
        }

        public string Quantity
        {
            get;
            set;
        }

        public string CreateDate
        {
            get;
            set;
        }

        public string Status
        {
            get;
            set;
        }

        public string State
        {
            get;
            set;
        }

        public string PRating
        {
            get;
            set;
        }

        public string StateColor
        {
            get;
            set;
        }

        public string StatusColor
        {
            get;
            set;
        }
        public string Manufacturer
        {
            get;
            set;
        }

        public string Rating
        {
            get;
            set;
        }

        public string Description
        {
            get;
            set;
        }
    }
}
