using GridCentral.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace GridCentral.Views.ObjectViews
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ImageView : ContentPage
    {
        ObservableCollection<mCarouselImage> theimages = new ObservableCollection<mCarouselImage>();
        public ImageView(ObservableCollection<mCarouselImage> images)
        {
            InitializeComponent();

            CarouselImages.ItemsSource = theimages;
            CarouselImages.ItemsSource = images;
        }
        private void TapGestureRecognizer_Tapped(object sender, EventArgs e)
        {
            Navigation.PopModalAsync();
        }

    }
}
