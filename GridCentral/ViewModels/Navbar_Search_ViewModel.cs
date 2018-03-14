using GridCentral.Interfaces;
using GridCentral.Views.Search;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace GridCentral.ViewModels
{
    public class Navbar_Search_ViewModel : Base_ViewModel
    {

        #region Bind Property
        string _searchtxt;
        public static string  tempo;

        public string SearchTxt
        {
            get { return _searchtxt; }
            set { _searchtxt = value; OnPropertyChanged("SearchTxt"); tempo = value; }
        }

        public ICommand SearchTxtCommand { get; private set; }

        IPageService _pageService;
        #endregion
        public Navbar_Search_ViewModel(IPageService pageService)
        {
            _pageService = pageService;

            SearchTxtCommand = new Command(txt => SearchTxtAction(txt));
        }

        private void SearchTxtAction(object txt)
        {
            if (usingMain)
                _pageService.PushAsync(new ProductSearch(txt.ToString(), Product_ProductSearch_ViewModel.catgeroyTempo));
            else
                _pageService.PushAsync(new BuySellSearch(txt.ToString(), Product_ProductSearch_ViewModel.catgeroyTempo));
        }   
    }
}
