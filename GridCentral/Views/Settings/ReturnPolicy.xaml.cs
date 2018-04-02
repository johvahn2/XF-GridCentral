using GridCentral.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace GridCentral.Views.Settings
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class ReturnPolicy : ContentPage
	{
		public ReturnPolicy ()
		{
			InitializeComponent ();

            Policylbl.Text = Strings.ReturnPolicy;

        }
	}
}