using GridCentral.Helpers;
using GridCentral.Services;
using GridCentral.Views.Navigation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace GridCentral.Views.Cart
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Connector : TabbedPage
    {
        public Connector()
        {
            if (AccountService.Instance.Current_Account == null)
            {
                Navigation.PopAsync();
                return;
            }

            InitializeComponent();

            SetStrings();
        }

        private void SetStrings()
        {
            CartTab.Title = Strings.Cart_Tab_Title;
            SaveLaterTab.Title = Strings.SaveLater_Tab_Title;
            if(Device.OS == TargetPlatform.iOS)
            {
                CartTab.Icon = "Cart-20x20.png";
                SaveLaterTab.Icon = "save-20x20.png";
            }
            
        }
    }
}