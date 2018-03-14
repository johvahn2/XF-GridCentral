using Plugin.Settings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace GridCentral.Views.Order
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ChgDeliveryTime : ContentPage
    {
        public ChgDeliveryTime()
        {
            InitializeComponent();

            ChgBtn.Clicked += ChgBtn_Clicked;
        }

        private void ChgBtn_Clicked(object sender, EventArgs e)
        {
            var from = FromTime.Time;
            var to = ToTime.Time;
            CrossSettings.Current.AddOrUpdateValue<string>("FromTime", from.ToString());
            CrossSettings.Current.AddOrUpdateValue<string>("ToTime", to.ToString());

            Navigation.PopModalAsync();
        }

        private void Close(object sender, EventArgs e)
        {
            Navigation.PopModalAsync();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            FromTime.Time = new TimeSpan(11, 0, 0);
            ToTime.Time = new TimeSpan(19, 0, 0);

            //if (!String.IsNullOrEmpty(CrossSettings.Current.GetValueOrDefault<string>("FromTime")) ||
            //    !String.IsNullOrEmpty(CrossSettings.Current.GetValueOrDefault<string>("ToTime")))
            //{
            //    TimeSpan from = CrossSettings.Current.GetValueOrDefault<TimeSpan>("FromTime");
            //    TimeSpan to = CrossSettings.Current.GetValueOrDefault<TimeSpan>("ToTime");

            //    FromTime.Time = from;
            //    ToTime.Time = to;
            //}
            //else
            //{
            //    FromTime.Time = new TimeSpan(11, 0,0);
            //    ToTime.Time = new TimeSpan(7,0, 0);
            //}
        }
    }
}
