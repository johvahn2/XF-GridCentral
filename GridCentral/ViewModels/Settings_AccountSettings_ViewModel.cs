using GridCentral.Helpers;
using GridCentral.Models;
using GridCentral.Services;
using Plugin.ImageResizer;
using Plugin.Media;
using Plugin.Media.Abstractions;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace GridCentral.ViewModels
{
    public class Settings_AccountSettings_ViewModel : Base_ViewModel
    {

        #region Bind Property
        List<string> _gender = new List<string>() { "Male", "Female" };
        int _GenderTypeIndex;
        string _newpassword;
        string _renewpassword;
        string _currpassword;
        string _firstname;
        string _lastname;
        string _phonenumber;
        string _ProfileImage;
        string _Displayname;
        bool _Notfiy = true;
        DateTime _bDay = DateTime.Now.Date;

        byte[] _bProfileImage = null;


        public byte[] bProfileImage
        {
            get { return _bProfileImage; }
            set { _bProfileImage = value; OnPropertyChanged("bProfileImage"); }
        }

        public string Displayname
        {
            get { return _Displayname; }
            set
            {
                _Displayname = value; OnPropertyChanged("Displayname");
            }
        }


        public string ProfileImage
        {
            get { return _ProfileImage; }
            set
            {
                _ProfileImage = value; OnPropertyChanged("ProfileImage");
            }

        }
        public bool Notfiy
        {
            get { return _Notfiy; }
            set
            {
                _Notfiy = value; OnPropertyChanged("Notfiy");
            }
        }

        public string NewPassword
        {
            get { return _newpassword; }
            set
            {
                _newpassword = value; OnPropertyChanged("NewPassword");
            }
        }

        public string ReNewPassword
        {
            get { return _renewpassword; }
            set
            { _renewpassword = value; OnPropertyChanged("ReNewPassword"); }
        }

        public string CurrPassword
        {
            get { return _currpassword; }
            set
            {
                _currpassword = value; OnPropertyChanged("CurrPassword");
            }
        }

        public string FirstName
        {
            get { return _firstname; }
            set { _firstname = value; OnPropertyChanged("FirstName"); }
        }

        public string LastName
        {
            get { return _lastname; }
            set { _lastname = value; OnPropertyChanged("LastName"); }
        }

        public string PhoneNumber
        {
            get { return _phonenumber; }
            set
            {
                _phonenumber = value; OnPropertyChanged("PhoneNumber");
            }
        }

        public DateTime bDay
        {
            get { return _bDay; }
            set { _bDay = value; OnPropertyChanged("bDay"); }
        }

        public List<String> Gender
        {
            get { return _gender; }
        }

        public int GenderTypeIndex
        {
            get { return _GenderTypeIndex; }
            set { _GenderTypeIndex = value; OnPropertyChanged("GenderTypeIndex"); }
        }
        #endregion

        public ICommand UpdateCommand { get; private set; }
        public Settings_AccountSettings_ViewModel()
        {
            UpdateCommand = new Command(() => UpdateAccount());

            mAccount Curr_Acc = AccountService.Instance.Current_Account;

            ProfileImage = Curr_Acc.ProfileImage;
            FirstName = Curr_Acc.FirstName; LastName = Curr_Acc.LastName; PhoneNumber = Curr_Acc.PhoneNumber; bDay = Curr_Acc.bDay;
            Displayname = Curr_Acc.Displayname;
            GenderTypeIndex = Gender.IndexOf(Curr_Acc.Gender);

            if (Curr_Acc.Notify)
            {
                Notfiy = true;
            }
            else
            {
                Notfiy = false;
            }
        }

        private async void UpdateAccount()
        {
            if (IsBusy) return;

            if (String.IsNullOrEmpty(FirstName) || String.IsNullOrEmpty(LastName) || String.IsNullOrEmpty(PhoneNumber) || String.IsNullOrEmpty(Displayname) || String.IsNullOrEmpty(bDay.ToString()))
            {
                DialogService.ShowError(Strings.Enter_All_Fields);
                return;
            }

            mAccount updateAccount = new mAccount();

            if (!String.IsNullOrEmpty(NewPassword))
            {
                if (NewPassword != ReNewPassword)
                {
                    DialogService.ShowError("New Passwords Dont Match");
                    return;
                }

                if (!String.IsNullOrEmpty(NewPassword) && String.IsNullOrEmpty(CurrPassword))
                {
                    DialogService.ShowError("Enter Current Password");
                    return;
                }

                updateAccount = new mAccount()
                {
                    FirstName = FirstName,
                    LastName = LastName,
                    bDay = bDay,
                    Displayname = Displayname,
                    Gender = Gender[GenderTypeIndex],
                    PhoneNumber = PhoneNumber,
                    NewPassword = NewPassword,
                    Password = CurrPassword,
                    Interests = AccountService.Instance.Current_Account.Interests,
                    Email = AccountService.Instance.Current_Account.Email,
                    Notify = Notfiy,
                    bProfileImage = bProfileImage
                };
            }
            else
            {
                updateAccount = new mAccount()
                {
                    FirstName = FirstName,
                    LastName = LastName,
                    Displayname = Displayname,
                    bDay = bDay,
                    Gender = Gender[GenderTypeIndex],
                    PhoneNumber = PhoneNumber,
                    Password = "",
                    Interests = AccountService.Instance.Current_Account.Interests,
                    Email = AccountService.Instance.Current_Account.Email,
                    Notify = Notfiy,
                    bProfileImage = bProfileImage
                };
            }

            try
            {
                IsBusy = true;

                DialogService.ShowLoading(Strings.Updating_Account);

                var result = await AccountService.Instance.UpdateAccount(updateAccount);
                DialogService.HideLoading();
                if (result == "true")
                {
                    AccountService.Instance.Current_Account = await UserService.Instance.FetchUser(updateAccount.Email);

                    DialogService.ShowToast(Strings.Account_Updated);

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
            finally { IsBusy = false; }
        }


        public async Task<byte[]> GetProfileImage()
        {
            await CrossMedia.Current.Initialize();

            if (!CrossMedia.Current.IsCameraAvailable || !CrossMedia.Current.IsTakePhotoSupported)
            {
                await DialogService.DisplayAlert(Strings.Ok, null, Strings.No_Camera,Strings.Camera_Not_Available);
                return null;
            }

            MediaFile file = null;

            var action = await DialogService.DisplayActionSheet("Image Options", "Cancel", null, "Selected Picture", "Take Picture");

            if (action == "Selected Picture")
            {
                file = await CrossMedia.Current.PickPhotoAsync(new Plugin.Media.Abstractions.PickMediaOptions
                {
                    CompressionQuality = 50,
                    PhotoSize = PhotoSize.Medium
                });

            }
            else if (action == "Take Picture")
            {
                // DialogService.ShowToast("Not Available");
                file = await CrossMedia.Current.TakePhotoAsync(new StoreCameraMediaOptions
                {
                    CompressionQuality = 100,
                    PhotoSize = PhotoSize.Full
                });
            }

            if (file == null) return null;

            byte[] resizedImage = await CrossImageResizer.Current.ResizeImageWithAspectRatioAsync(file.GetStream(), 1000, 1000);
            bProfileImage = resizedImage;
            return resizedImage;

            //var stream = file.GetStream();
            //byte[] buffer = new byte[stream.Length];
            //using (MemoryStream ms = new MemoryStream())
            //{
            //    stream.CopyTo(ms);
            //    buffer = ms.ToArray();
            //    file.Dispose();

            //    bProfileImage = buffer;
            //    return buffer;
            //}
        }
    }
}
