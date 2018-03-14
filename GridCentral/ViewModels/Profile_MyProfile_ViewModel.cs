using GridCentral.Helpers;
using GridCentral.Services;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GridCentral.ViewModels
{
    public class Profile_MyProfile_ViewModel : Base_ViewModel
    {
        private static Profile_MyProfile_ViewModel instance;
        public static Profile_MyProfile_ViewModel Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new Profile_MyProfile_ViewModel();
                }

                return instance;
            }
        }

        #region Bind Property

        string _ProfileImage;
        string _fullname;
        string _joinyear;
        bool _toggler;

        public bool toggler
        {
            get { return _toggler; }
            set { _toggler = value; OnPropertyChanged("toggler"); }
        }

        public string ProfileImage
        {
            get { return _ProfileImage; }
            set { _ProfileImage = value; OnPropertyChanged("ProfileImage"); }
        }

        public string JoinYear
        {
            get { return _joinyear; }
            set { _joinyear = value; OnPropertyChanged("JoinYear"); }
        }

        public string FullName
        {
            get { return _fullname; }
            set { _fullname = value; OnPropertyChanged("FullName"); }
        }
        #endregion
        public Profile_MyProfile_ViewModel()
        {
            //GetProfile(AccountService.Instance.Current_Account.Email);
            var curr_acc = AccountService.Instance.Current_Account;
            FullName = curr_acc.FirstName + " " + curr_acc.LastName;
            ProfileImage = curr_acc.ProfileImage;
            JoinYear = curr_acc.createdAt.Split('-')[0];

        }

        private void GetProfile(string email)
        {
            IsBusy = true;

            try
            {


            }catch(Exception ex)
            {
                DialogService.ShowError(Strings.SomethingWrong);
                Debug.WriteLine(Keys.TAG + ex);
            }
            finally { IsBusy = false; }
        }

    }
}
