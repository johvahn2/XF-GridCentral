using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using GridCentral.Interfaces;
using Xamarin.Forms;
using HockeyApp;

[assembly:Xamarin.Forms.Dependency(typeof(GridCentral.Droid.Services.MetricsManagerService))]
namespace GridCentral.Droid.Services
{
    public class MetricsManagerService : IMetricsManagerService
    {
        public void TrackEvent(string eventName)
        {
            if(Device.OS == TargetPlatform.Android || Device.OS == TargetPlatform.iOS)
            {
                MetricsManager.TrackEvent(eventName);
            }

        }
    }
}