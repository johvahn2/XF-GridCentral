using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace GridCentral.Helpers
{
    public static class NavigationPageHelper
    {
        public static NavigationPage Create(Page page)
        {
            return new NavigationPage(page) { BarTextColor = Color.White };
        }
    }
}
