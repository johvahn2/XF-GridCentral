using GridCentral.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace GridCentral.Views.Home.Template
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class InspiredItemTemplate : ContentView
    {
        public InspiredItemTemplate()
        {
            InitializeComponent();
        }

        private void TapGestureRecognizer_Tapped(object sender, EventArgs e)
        {
            DialogService.ShowToast("Tap Inspired");
        }
    }
}
