using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Foundation;
using UIKit;
using System.Collections;
using ToastIOS;
using GridCentral.iOS.Native;
using GridCentral.Interfaces;

[assembly: Xamarin.Forms.Dependency(typeof(MessageIOS))]
namespace GridCentral.iOS.Native
{
    class MessageIOS : IMessage
    {
        public void Alert(string message)
        {
            Toast.MakeText(message).Show();
        }
    }
}