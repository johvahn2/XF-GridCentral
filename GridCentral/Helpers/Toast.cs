using GridCentral.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace GridCentral.Helpers
{
    class iToast
    {
        public static void iMessage(string message)
        {
            DependencyService.Get<IMessage>().Alert(message);
        }
    }
}
