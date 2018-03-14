using GridCentral.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace GridCentral.Helpers
{
    public class Dial
    {
        public static void Iphone_Dial(string number)
        {
            DependencyService.Get<IDial>().Dial_Phone(number);
        }
    }
}
