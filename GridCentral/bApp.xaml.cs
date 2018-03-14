using System;
using System.Collections.Generic;
using System.Linq;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace GridCentral
{
	//[XamlCompilation (XamlCompilationOptions.Skip)]
	public partial class bApp : Application
	{
		public static MasterDetailPage MasterDetailPage;

		public bApp()
		{
			InitializeComponent();

			MainPage = GetMainPage();

			MainPage.SetValue(NavigationPage.BarTextColorProperty, Color.White);
		}

		public static Page GetMainPage()
        {
            return new WelcomeStarterPage();
        }
	}
}
