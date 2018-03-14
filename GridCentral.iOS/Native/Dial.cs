using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Foundation;
using GridCentral.Interfaces;
using GridCentral.iOS.Native;
using UIKit;

[assembly: Xamarin.Forms.Dependency(typeof(Dial))]
namespace GridCentral.iOS.Native
{
    class Dial : IDial
    {
        public void Dial_Phone(string number)
        {
            var url = new NSUrl("tel:" + number);

            if (!UIApplication.SharedApplication.OpenUrl(url))
            {
                var av = new UIAlertView("Not supported",
                  "Scheme 'tel:' is not supported on this device",
                  null,
                  "OK",
                  null);
                av.Show();
            };

        }
    }
}