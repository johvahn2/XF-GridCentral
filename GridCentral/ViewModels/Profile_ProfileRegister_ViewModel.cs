using GridCentral.Helpers;
using GridCentral.Interfaces;
using GridCentral.Models;
using GridCentral.Services;
using GridCentral.Views.Auth.ProfileSetup_Pages;
using GridCentral.Views.Navigation;
using Plugin.ImageResizer;
using Plugin.Media;
using Plugin.Media.Abstractions;
using Plugin.Settings;
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
    public class Profile_ProfileRegister_ViewModel : Base_ViewModel
    {
        #region Bind Property

        string _firstname;
        List<string> _interests = new List<string>();
        byte[] _bProfileImage = null;
        string _Displayname = "";


        public byte[] bProfileImage
        {
            get { return _bProfileImage; }
            set { _bProfileImage = value; OnPropertyChanged("bProfileImage"); }
        }

        public string FirstName
        {
            get { return _firstname; }
            set { _firstname = value; OnPropertyChanged("FirstName"); }
        }

        public string Displayname
        {
            get { return _Displayname; }
            set { _Displayname = value; OnPropertyChanged("Displayname"); }
        }

        public List<string> Interests
        {
            get { return _interests; }
            set { _interests = value; OnPropertyChanged("Interests"); }
        }


        #region Interest List
        bool _appliances = false; bool _art = false; bool _baby = false; bool _books = false; bool _cars = false; bool _clothing = false; bool _electronics = false;
        bool _furniture = false; bool _health_beauty = false; bool _toys_games = false; bool _makeup_beauty = false; bool _jewelry = false;
        bool _personal_care = false; bool _home_supplies = false;
        public bool Appliances
        {
            get { return _appliances; }
            set
            { _appliances = value; OnPropertyChanged("Appliances"); }
        }

        public bool Home_Supplies
        {
            get { return _home_supplies; }
            set
            { _home_supplies = value; OnPropertyChanged("Home_Supplies"); }
        }

        public bool Art
        {
            get { return _art; }
            set
            { _art = value; OnPropertyChanged("Art"); }
        }

        public bool Baby
        {
            get { return _baby; }
            set
            { _baby = value; OnPropertyChanged("Baby"); }
        }

        public bool Books
        {
            get { return _books; }
            set
            { _books = value; OnPropertyChanged("Books"); }
        }
        public bool Cars
        {
            get { return _cars; }
            set
            { _cars = value; OnPropertyChanged("Cars"); }
        }

        public bool Clothing
        {
            get { return _clothing; }
            set
            { _clothing = value; OnPropertyChanged("Clothing"); }
        }
        public bool Electronics
        {
            get { return _electronics; }
            set
            { _electronics = value; OnPropertyChanged("Electronics"); }
        }
        public bool Furniture
        {
            get { return _furniture; }
            set
            { _furniture = value; OnPropertyChanged("Furniture"); }
        }

        public bool Health_Beauty
        {
            get { return _health_beauty; }
            set
            { _health_beauty = value; OnPropertyChanged("Health_Beauty"); }
        }

        public bool Personal_Care
        {
            get { return _personal_care; }
            set
            { _personal_care = value; OnPropertyChanged("Personal_Care"); }
        }

        public bool Makeup_Beauty
        {
            get { return _makeup_beauty; }
            set
            { _makeup_beauty = value; OnPropertyChanged("Makeup_Beauty"); }
        }

        public bool Jewelry
        {
            get { return _jewelry; }
            set
            { _jewelry = value; OnPropertyChanged("Jewelry"); }
        }

        public bool Toys_Games
        {
            get { return _toys_games; }
            set
            { _toys_games = value; OnPropertyChanged("Toys_Games"); }
        }
        #endregion

        public ICommand ProfileImage_NextCommand { get; private set; }

        public ICommand Interest_NextCommand { get; private set; }


        public ICommand GetStartedCommand { get; private set; }
        private readonly IPageService _pageService;
        #endregion


        mAccount curr_Account = AccountService.Instance.Current_Account;

        public Profile_ProfileRegister_ViewModel(IPageService pageService)
        {
            _pageService = pageService;

            FirstName = "Welcome " + curr_Account.FirstName;

            ProfileImage_NextCommand = new Command(async () => await ProfileImage_NextAction());

            Interest_NextCommand = new Command(async () => await Interest_NextAction());

            GetStartedCommand = new Command(() =>  GetStartedActionAsync());


        }

        private async Task GetStartedActionAsync()
        {
            DialogService.ShowLoading();

            AccountService.Instance.Current_Account = await UserService.Instance.FetchUser(curr_Account.Email);
            var Usercreds = Newtonsoft.Json.JsonConvert.SerializeObject(AccountService.Instance.Current_Account);
            CrossSettings.Current.AddOrUpdateValue<string>("Current_User", Usercreds);
            DialogService.HideLoading();
            _pageService.ShowMain(new RootPage(false));

            //AccountService.Instance.UpdateCurr_Account(await UserService.Instance.FetchUser(curr_Account.Email));

        }

        private async Task Interest_NextAction()
        {
            if (IsBusy) return;

            IsBusy = true;

            DialogService.ShowLoading("Saving Interests");
            checkIntrest();

            try
            {
                if (Interests.Count < 1)
                {
                    DialogService.HideLoading();
                    DialogService.ShowToast("No options were selected");
                    return;
                }

                mAccount updated_Account = UpdateAccount("Interest");

                var result = await AccountService.Instance.UpdateAccount(updated_Account);
                DialogService.HideLoading();

                if (result == "true")
                {
                    await _pageService.PushModalAsync(new ProfileInfo());
                }
                else
                {
                    DialogService.ShowError(result);
                }



            }
            catch (Exception ex)
            {
                DialogService.ShowError(Strings.SomethingWrong);
                Debug.WriteLine(Keys.TAG + ex);

            }
            finally { IsBusy = false; }
        }


        private async Task ProfileImage_NextAction()
        {
            if (IsBusy) return;

            IsBusy = true;

            DialogService.ShowLoading("Please wait");

            try
            {
                if(bProfileImage == null || bProfileImage.Length < 1)
                {
                    DialogService.HideLoading();

                    var res = await DialogService.DisplayAlert("Yes", "Later", "Profile Image", "Do you want to add a profile image?");

                    if (res)
                    {
                        DialogService.ShowToast("Please Tap the circle image");
                        return;
                    }
                    //DialogService.HideLoading();
                    //DialogService.ShowError("Please Add An Image");return;
                }
                if(Displayname.Length < 1 || Displayname == null)
                {
                    DialogService.HideLoading();
                    DialogService.ShowError("Please Enter Display Name"); return;
                }

                mAccount updated_Account = UpdateAccount("ProfileImage");

                var result = await AccountService.Instance.UpdateAccount(updated_Account);
                DialogService.HideLoading();

                if (result == "true")
                {

                   await _pageService.PushModalAsync(new ProfileInterest());
                }
                else
                {
                    DialogService.ShowError(result);
                }

            }
            catch (Exception ex)
            {
                DialogService.ShowError(Strings.SomethingWrong);
                Debug.WriteLine(Keys.TAG + ex);
            }
            finally { IsBusy = false; }
        }

        public async Task<byte[]> GetProfileImage(string action)
        {
            await CrossMedia.Current.Initialize();


            MediaFile file = null;

            // var action = await DialogService.DisplayActionSheet("Image Options", "Cancel", null, "Selected Picture", "Take Picture");

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
                if (!CrossMedia.Current.IsCameraAvailable || !CrossMedia.Current.IsTakePhotoSupported)
                {
                    await DialogService.DisplayAlert("Ok", "OK", "No Camera", ":( No camera available");
                    return null;
                }
                // DialogService.ShowToast("Not Available");
                file = await CrossMedia.Current.TakePhotoAsync(new StoreCameraMediaOptions
                {
                    CompressionQuality = 100,
                    PhotoSize = PhotoSize.Full
                });
            }

            if (file == null) return null;

            byte[] resizedImage = await CrossImageResizer.Current.ResizeImageWithAspectRatioAsync(file.GetStream(), 1000, 1000);
            file.Dispose();
            bProfileImage = resizedImage;
            return resizedImage;

        }


        private mAccount UpdateAccount(string what)
        {
            mAccount updated_Account = new mAccount()
            {
                Email = curr_Account.Email,
                Gender = curr_Account.Gender,                   
                bDay = curr_Account.bDay,
                FirstName = curr_Account.FirstName,
                LastName = curr_Account.LastName,
                Address = curr_Account.Address,
                Password = "",
                PhoneNumber = curr_Account.PhoneNumber,
                Notify = curr_Account.Notify,
                Interests = curr_Account.Interests,
                bProfileImage = null
            };
            if(what == "ProfileImage")
            {
                updated_Account.bProfileImage = bProfileImage;
                updated_Account.Displayname = Displayname;
            }else if(what == "Interest")
            {
                updated_Account.Interests = Interests;
            }

            return updated_Account;
        }

        private void checkIntrest()
        {
            if (Appliances)
            {
                Interests.Add("Appliances");
            }

            if (Art)
            {
                Interests.Add("Art");
            }

            if (Books)
            {
                Interests.Add("Books");
            }

            if (Baby)
            {
                Interests.Add("Baby");
            }

            if (Cars)
            {
                Interests.Add("Cars");
            }

            if (Clothing)
            {
                Interests.Add("Clothing");
            }

            if (Electronics)
            {
                Interests.Add("Electronics");
            }

            if (Furniture)
            {
                Interests.Add("Furniture");
            }

            if (Home_Supplies)
            {
                Interests.Add("Home_Supplies");
            }

            if (Health_Beauty)
            {
                Interests.Add("Health_Beauty");
            }

            if (Personal_Care)
            {
                Interests.Add("Personal_Care");
            }

            if (Makeup_Beauty)
            {
                Interests.Add("Makeup_Beauty");
            }

            if (Jewelry)
            {
                Interests.Add("Jewelry");
            }

            if (Toys_Games)
            {
                Interests.Add("Toys_Games");
            }
        }

    }
}
