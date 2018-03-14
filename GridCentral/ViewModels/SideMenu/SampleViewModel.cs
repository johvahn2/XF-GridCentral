using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace GridCentral.ViewModels
{
    public class SamplesViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private Sample _selectedSample;

        public SamplesViewModel(INavigation navigation)
        {
            SamplesCategories = new List<SampleCategory>(SampleDefinition.SamplesCategories.Values);
            AllSamples = SampleDefinition.AllSamples;
            SamplesGroupedByCategory = SampleDefinition.SamplesGroupedByCategory;
        }

        public List<SampleCategory> SamplesCategories { get; set; }

        public List<Sample> AllSamples { get; set; }

        public List<string> Samples { get; set; }

        public List<SampleGroup> SamplesGroupedByCategory { get; set; }

        public Sample SelectedSample
        {
            get
            {
                return _selectedSample;
            }

            set
            {
                if (value != _selectedSample)
                {
                    _selectedSample = value;

                    RaisePropertyChanged("SelectedSample");
                }
            }
        }

        private void RaisePropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}
