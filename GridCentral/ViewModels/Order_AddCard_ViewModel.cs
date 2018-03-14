using GridCentral.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GridCentral.ViewModels
{
    public class Order_AddCard_ViewModel : Base_ViewModel
    {

        #region Bind Property
        string _name;
        string _lastname;
        string _cardnumber;
        string _cvv;
        string _expiredate;
        string _address1;
        string _address2;
        string _city;
        string _region;
        string _zipcode;
        int _countryindex;
        List<string> _countries;


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

        public string Expiredate
        {
            get { return _expiredate; }
            set
            {
                _expiredate = value;
                OnPropertyChanged("Expiredate");
            }
        }

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

        #endregion

        IPageService _pageService;

        public Order_AddCard_ViewModel(IPageService pageSerivce)
        {
            _pageService = pageSerivce;


        }
    }
}
