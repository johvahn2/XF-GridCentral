using GridCentral.Models;
using GridCentral.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace GridCentral.ViewModels
{
    public class BuySell_ContactSeller_ViewModel : Base_ViewModel
    {

        #region Bind Property
        string _email;
        string _fullname;
        string _phonenumber;

        public string Email
        {
            get { return _email; }
            set { _email = value; OnPropertyChanged("Email"); }
        }

        public string FullName
        {
            get { return _fullname; }
            set { _fullname = value; OnPropertyChanged("Name"); }
        }

        public string PhoneNumber
        {
            get { return _phonenumber; }
            set { _phonenumber = value; OnPropertyChanged("Phone"); }
        }
        #endregion
        public ICommand CallCommand { get; private set; }
        public BuySell_ContactSeller_ViewModel(mAccount sellerInfo)
        {
            CallCommand = new Command(() => CallAction());

            Email = sellerInfo.Email; FullName = sellerInfo.FirstName + " " + sellerInfo.LastName; PhoneNumber = sellerInfo.PhoneNumber;
        }

        private void CallAction()
        {
            DialogService.QCall(PhoneNumber);
        }
    }
}
