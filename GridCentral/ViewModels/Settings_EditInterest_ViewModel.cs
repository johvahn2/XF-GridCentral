using GridCentral.Helpers;
using GridCentral.Models;
using GridCentral.Services;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace GridCentral.ViewModels
{
    public class Settings_EditInterest_ViewModel : Base_ViewModel
    {

        #region Interest List

        List<string> _interests = new List<string>();
        public List<string> Interests
        {
            get { return _interests; }
            set { _interests = value; OnPropertyChanged("Interests"); }
        }

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

        public ICommand UpdateCommand { get; private set; }
        mAccount Curr_Acc = AccountService.Instance.Current_Account;

        public Settings_EditInterest_ViewModel()
        {
            Check_MyInterests();
            UpdateCommand = new Command(() => UpdateInterest());
        }

        private void Check_MyInterests()
        {
            if(Curr_Acc.Interests != null && Curr_Acc.Interests.Count > 0)
            {
                for (int i = 0; i < Curr_Acc.Interests.Count; i++)
            {

                if (Curr_Acc.Interests[i] == "Appliances")
                {
                        Appliances = true;
                        continue;
                }

                if (Curr_Acc.Interests[i] == "Art")
                {
                        Art = true;
                        continue;
                }
                if (Curr_Acc.Interests[i] == "Baby")
                {
                    Baby = true;
                    continue;
                }
                if (Curr_Acc.Interests[i] == "Books")
                {
                    Books = true;
                    continue;
                }

                if (Curr_Acc.Interests[i] == "Cars")
                {
                    Cars = true;
                    continue;
                }

                if (Curr_Acc.Interests[i] == "Clothing")
                {
                    Clothing = true;
                    continue;
                }

                if (Curr_Acc.Interests[i] == "Electronics")
                {
                    Electronics = true;
                    continue;
                }

                if (Curr_Acc.Interests[i] == "Furniture")
                {
                    Furniture = true;
                    continue;
                }

                if (Curr_Acc.Interests[i] == "Health_Beauty")
                {
                    Health_Beauty = true;
                    continue;
                }

                if (Curr_Acc.Interests[i] == "Personal_Care")
                {
                    Personal_Care = true;
                    continue;
                }

                if (Curr_Acc.Interests[i] == "Home_Supplies")
                {
                        Home_Supplies = true;
                        continue;
                    }

                if (Curr_Acc.Interests[i] == "Makeup_Beauty")
                {
                    Makeup_Beauty = true;
                    continue;
                }

                if (Curr_Acc.Interests[i] == "Jewelry")
                {
                        Jewelry = true;
                        continue;
                }
                if (Curr_Acc.Interests[i] == "Toys_Games")
                {
                    Toys_Games = true;
                    continue;
                }
            }
            }
        }

        private async void UpdateInterest()
        {
            if (IsBusy) return;

            try
            {
                IsBusy = true;

                checkIntrest();
                Curr_Acc.Interests = Interests; Curr_Acc.Password = "";

                DialogService.ShowLoading(Strings.Updating_Interests);

                var result = await AccountService.Instance.UpdateAccount(Curr_Acc);
                DialogService.HideLoading();
                if (result == "true")
                {
                    AccountService.Instance.Current_Account = await UserService.Instance.FetchUser(Curr_Acc.Email);

                    DialogService.ShowToast(Strings.Interests_Updated);

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
