using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace GridCentral.Views.ObjectViews
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class DescriptionView : ContentPage
    {
        public DescriptionView(string Desc)
        {
            InitializeComponent();

            Description.Text = Desc;
        }

        private void TapGestureRecognizer_Tapped(object sender, EventArgs e)
        {
            Navigation.PopModalAsync();
        }
    }
}
