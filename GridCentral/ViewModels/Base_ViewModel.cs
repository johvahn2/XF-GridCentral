using GridCentral.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GridCentral.ViewModels
{
    public class Base_ViewModel : INotifyPropertyChanged
    {

        public static bool usingMain;

        private bool isBusy;

        public event PropertyChangedEventHandler PropertyChanged;

        public bool IsBusy
        {
            get { return isBusy; }
            set { SetProperty(ref isBusy, value, "IsBusy"); }
        }

        protected void SetProperty<T>(ref T backingStore, T value, string propertyName, Action onChanged = null, Action<T> onChanging = null)
        {
            if (EqualityComparer<T>.Default.Equals(backingStore, value))
                return;

            backingStore = value;

            if (onChanged != null)
                onChanged();

            OnPropertyChanged(propertyName);
        }

        public void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged == null)
                return;

            PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }


        public ObservableCollection<Product> CalculatePercentages(ObservableCollection<Product> products)
        {
            for (int i = 0; i < products.Count; i++)
            {
                if (products[i].IsDeal == "True")
                {
                    var offset = (Convert.ToDouble(products[i].DealPrice) / Convert.ToDouble(products[i].Price)) * 100;


                    products[i].DealPercentage = (100 - Convert.ToInt16(offset)).ToString();
                }
            }

            return products;
        }
    }
}
