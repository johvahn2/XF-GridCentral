using GridCentral.Helpers;
using GridCentral.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace GridCentral.Views.Contact
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ContactUs : ContentPage
    {
        string pNumber1 = Strings.Grid_Central_PhoneNumber1; string pNumber2 = Strings.Grid_Central_PhoneNumber2;
        public ContactUs()
        {
            InitializeComponent();
            SetStrings();

            FedbackBtn.Clicked += (sender, e) =>
            {
                 Navigation.PushAsync(new Feedback());
                //throw new Exception();
            };
        }

        private void SetStrings()
        {
            emaillbl.Text = Strings.Grid_Central_Email;
            phonenumberlbl.Text = pNumber1 + " / " + pNumber2;
            facebooklbl.Text = Strings.Grid_Central_FaceBook;
            twitterlbl.Text = Strings.Grid_Central_Twitter;
            instagramlbl.Text = Strings.Grid_Central_Instagram;
            FedbackBtn.Text = Strings.Send_Feedback;
        }

        private async void Phone_Tapped(object sender, EventArgs e)
        {
           var res = await DialogService.DisplayActionSheet("Contact Us", "Close", null, pNumber1, pNumber2);

            if(res == pNumber1)
            {
                DialogService.QCall(pNumber1);
            }else if(res == pNumber2)
            {
                DialogService.QCall(pNumber2);
            }
        }
    }
}
