using GridCentral.Helpers;
using GridCentral.Models;
using GridCentral.Services;
using Microsoft.AppCenter.Crashes;
using Plugin.Connectivity;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GridCentral.ViewModels
{
    public class BuySell_BuySellSearch_ViewModel : Base_ViewModel
    {
        #region Bind Property

        ObservableCollection<mUserItem> _ItemList = new ObservableCollection<mUserItem>();

        public ObservableCollection<mUserItem> ItemList
        {
            get { return _ItemList; }
            set
            {
                _ItemList = value;
                OnPropertyChanged("ItemList");
            }
        }

        public List<string> _CategoryItems = new List<string>() { "All", "Appliances", "Art", "Baby", "Books", "Cars", "Clothing", "Electronics", "Furniture", "Hair", "Home Supplies", "Personal Care", "Jewelry", "Makeup Beauty", "Other", "Toys Games" };
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
            set
            {
                _categoryindex = value; OnPropertyChanged("CategoryIndex");
                catgeroyTempo = CategoryItems[CategoryIndex];
                string catgeroyTemp = catgeroyTempo;

                if (CatItems.ContainsKey(catgeroyTempo))
                {
                    catgeroyTemp = CatItems[catgeroyTempo];
                }
                if (String.IsNullOrEmpty(SearchTxt))
                {
                    //GetCategoryItems(CategoryItems[CategoryIndex], 0, 10, false);
                }
                else
                {
                    GetSearch(SearchTxt, catgeroyTemp, 10, 0, false);
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

        public BuySell_BuySellSearch_ViewModel(string txtSearched, string category)
        {
            isRandom = false;
            SearchTxt = txtSearched;
            checker(txtSearched, category, 10, 0);
        }

        public async void GetCategoryItems(string category, int len, int amount, bool addon)//Not In USE
        {
            //if (IsBusy) return;

            //IsBusy = true; noItems = false;

            //try
            //{
            //    var result = await ProductService.Instance.FetchCategory(category, amount, len);

            //    if (result == null)
            //    {
            //        isDone = true;
            //        if (SearchList.Count < 1)
            //            noItems = true;
            //        return;
            //    }

            //    if (addon)
            //        SearchList.AddRange(formData(result));
            //    else
            //        SearchList = formData(result);
            //}
            //catch (Exception ex)
            //{
            //    Debug.WriteLine(Keys.TAG + ex);
            //    DialogService.ShowError(Strings.SomethingWrong);
            //}
            //finally { IsBusy = false; }
        }

        public async void GetSearch(string txtSearched, string category, int amount, int len, bool addon)
        {
            if (IsBusy || txtSearched.Length < 1) return;

            IsBusy = true; noItems = false;

            try
            {
                string link = "";

                if (category != null)
                {
                    if (category != CategoryItems[0])
                        link = Keys.Url_Main + "item/search/" + txtSearched + "?Category=" + category + "&amount=" + amount.ToString() + "&len=" + len.ToString();
                    else
                        link = Keys.Url_Main + "item/search/" + txtSearched + "?amount=" + amount.ToString() + "&len=" + len.ToString();

                }
                else
                {

                    return;
                }

                var result = await ItemService.Instance.FetchSearch(link);



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

                };
                if (addon)
                {
                    SearchList.AddRange(formData(result));
                    ItemList.AddRange(result);
                }
                else
                {
                    SearchList = formData(result);
                    ItemList = result;

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

        private ObservableCollection<mSearchProduct> formData(ObservableCollection<mUserItem> result)
        {
            ObservableCollection<mSearchProduct> search = new ObservableCollection<mSearchProduct>();
            for (var i = 0; i < result.Count; i++)
            {
                search.Add(new mSearchProduct
                {
                    Id = result[i].Id,
                    Price = result[i].Price,
                    State = result[i].State,
                    Quantity = result[i].Quantity,
                    Manufacturer = result[i].Displayname,
                    Thumbnail = result[i].Images[0]
                });

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

                search[i].CreateDate = result[i].CreatedDate.Split('T')[0];

                if (result[i].State == "New")
                {
                    search[i].StateColor = "Green";
                }
                else
                {
                    search[i].StateColor = "Red";
                }
            }
            return search;
        }

        public void checker(string searchTxt, string category, int amount, int len)
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
                CategoryIndex = CategoryItems.IndexOf(category);
                //GetCategoryItems(category, len, amount);
            }
        }
    }
}
