using GridCentral.Helpers;
using GridCentral.Interfaces;
using GridCentral.Models;
using GridCentral.Services;
using GridCentral.Views.Order;
using Plugin.Settings;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace GridCentral.ViewModels
{
    public class Order_AddCard_ViewModel : Base_ViewModel
    {

        #region Bind Property

        string _name;
        string _lastname;
        string _cardnumber;
        string _cvv;
        DateTime _expiredate = new DateTime();
        //DateTime _miniExpireDate = new DateTime().Date;
        string _address1;
        string _address2;
        string _city;
        string _region;
        string _zipcode;
        int _countryindex;
        List<string> _countries = Keys.Countries;


        public int Countryindex
        {
            get { return _countryindex; }
            set
            {
                _countryindex = value;
                OnPropertyChanged("Countryindex");
            }
        }

        public List<string> Countries
        {
            get { return _countries; }
            set
            {
                _countries = value;
                OnPropertyChanged("Countries");
            }
        }

        public string Name
        {
            get { return _name; }
            set
            {
                _name = value;
                OnPropertyChanged("Name");
            }
        }

        public string Lastname
        {
            get { return _lastname; }
            set
            {
                _lastname = value;
                OnPropertyChanged("Lastname");
            }
        }

        public string Cardnumber
        {
            get { return _cardnumber; }
            set
            {
                _cardnumber = value;
                OnPropertyChanged("Cardnumber");
            }
        }

        public string Cvv
        {
            get { return _cvv; }
            set
            {
                _cvv = value;
                OnPropertyChanged("Cvv");
            }
        }

        public DateTime Expiredate
        {
            get { return _expiredate; }
            set
            {
                _expiredate = value;
                OnPropertyChanged("Expiredate");
            }
        }

        //public DateTime MiniExpireDate
        //{
        //    get { return _miniExpireDate; }
        //    set
        //    {
        //        _miniExpireDate = value;
        //        OnPropertyChanged("MiniExpireDate");
        //    }
        //}

        public string Address1
        {
            get { return _address1; }
            set
            {
                _address1 = value;
                OnPropertyChanged("Address1");
            }
        }

        public string Address2
        {
            get { return _address2; }
            set
            {
                _address2 = value;
                OnPropertyChanged("Address2");
            }
        }

        public string City
        {
            get { return _city; }
            set
            {
                _city = value;
                OnPropertyChanged("City");
            }
        }

        public string Region
        {
            get { return _region; }
            set
            {
                _region = value;
                OnPropertyChanged("Region");
            }
        }
        public string ZipCode
        {
            get { return _zipcode; }
            set
            {
                _zipcode = value;
                OnPropertyChanged("ZipCode");
            }
        }

        public ICommand AddCommand { get; private set; }

        #endregion

        IPageService _pageService;
        ObservableCollection<mCart> _CartList = new ObservableCollection<mCart>();

        public Order_AddCard_ViewModel(IPageService pageService, ObservableCollection<mCart> CartList)
        {
            _CartList = CartList;
            _pageService = pageService;
            AddCommand = new Command(async () => await AddAction());

        }

        private async Task AddAction()
        {
            DialogService.ShowLoading("Proceeding");

            var SelectedCountries = Countries[Countryindex];

            mCard card = new mCard()
            {
                Name = Name,
                Lastname = Lastname,
                Address1 = Address1,
                Address2 = Address2,
                Cardnumber = Cardnumber,
                City = City,
                Cvv = Cvv,
                Expiredate = Expiredate,
                Region = Region,
                ZipCode = ZipCode,
                Country = SelectedCountries
            };

            var CardContent = Newtonsoft.Json.JsonConvert.SerializeObject(card);
            CrossSettings.Current.AddOrUpdateValue<string>("UseCard", CardContent);
            await _pageService.PopAsync();
            //if (_CartList != null)
            //{
            //    await _pageService.PushAsync(new ConfirmOrder(null,_CartList,card));
            //}
            //else
            //{
            //    var CardContent = Newtonsoft.Json.JsonConvert.SerializeObject(card);
            //    CrossSettings.Current.AddOrUpdateValue<string>("UseCard", CardContent);
            //    await _pageService.PopAsync();
            //}
            DialogService.HideLoading();

        }

    }
}
