using System;

using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using Android.Graphics.Drawables;
using Xamarin.Forms.Platform.Android;
using Xamarin.Forms;
using System.Threading.Tasks;

using UXDivers.Artina.Shared;
using UXDivers.Artina.Shared.Droid;

using FFImageLoading.Forms.Droid;
using Acr.UserDialogs;
using GridCentral.Elements;
using CarouselView.FormsPlugin.Abstractions;
using GridCentral.Models;
using Gcm.Client;
using PhoneCall.Forms.Plugin.Droid;
using HockeyApp.Android;
using HockeyApp.Android.Metrics;
using CarouselView.FormsPlugin.Android;
using XLabs.Forms;

namespace GridCentral.Droid
{
	//https://developer.android.com/guide/topics/manifest/activity-element.html
	[Activity(
		Label = "GridCentral",
		Icon = "@drawable/grid_ic",
		Theme = "@style/Theme.Splash",
	 	MainLauncher = true,
		LaunchMode = LaunchMode.SingleTask,
		ConfigurationChanges = ConfigChanges.Orientation | ConfigChanges.ScreenSize,
        ScreenOrientation = ScreenOrientation.Portrait
		)
	]
	public class MainActivity : FormsAppCompatActivity
    {
     

        protected override void OnCreate(Bundle bundle)
		{
			// Changing to App's theme since we are OnCreate and we are ready to 
			// "hide" the splash
			base.Window.RequestFeature(WindowFeatures.ActionBar);
			base.SetTheme(Resource.Style.AppTheme);


			FormsAppCompatActivity.ToolbarResource = Resource.Layout.Toolbar;
			FormsAppCompatActivity.TabLayoutResource = Resource.Layout.Tabs;

			base.OnCreate(bundle);

            //CrashManager.Register(this, GridCentral.Helpers.Keys.HockeyId_Android);
            //MetricsManager.Register(Application, GridCentral.Helpers.Keys.HockeyId_Android);

            //Initializing FFImageLoading
            CachedImageRenderer.Init();

			global::Xamarin.Forms.Forms.Init(this, bundle);
            CarouselViewRenderer.Init();
            UXDivers.Artina.Shared.GrialKit.Init(this, "GridCentral.Droid.GrialLicense");
            UserDialogs.Init(() => (Activity)Forms.Context);
            PhoneCallImplementation.Init();

			FormsHelper.ForceLoadingAssemblyContainingType(typeof(UXDivers.Effects.Effects));

            mPushNotify paramValue = new mPushNotify();

            paramValue.Messgae = Intent.GetStringExtra("message");
            paramValue.Objecter = Intent.GetStringExtra("objecter");
            paramValue.Type = Intent.GetStringExtra("type");
            paramValue.Why = Intent.GetStringExtra("why");

            if (!String.IsNullOrEmpty(paramValue.Messgae))
            {
                LoadApplication(new App(paramValue));
            }
            else
            {
                LoadApplication(new App());

                //LoadApplication(UXDivers.Gorilla.Droid.Player.CreateApplication(
                //    this,
                //    new UXDivers.Gorilla.Config("Good Gorilla").RegisterAssembliesFromTypes<UXDivers.Artina.Shared.CircleImage,
                //    GrialShapesFont, BindablePicker, XLabs.Forms.Controls.CheckBox, CarouselViewControl>()));
            }

            try
            {
                // Check to ensure everything's set up right
                GcmClient.CheckDevice(this);
                GcmClient.CheckManifest(this);

                // Register for push notifications
                System.Diagnostics.Debug.WriteLine("Registering...");
                GcmClient.Register(this, PushHandlerBroadcastReceiver.SENDER_IDS);
            }
            catch (Java.Net.MalformedURLException)
            {
                CreateAndShowDialog("There was an error creating the client. Verify the URL.", "Error");
            }
            catch (Exception e)
            {
                CreateAndShowDialog(e.Message, "Error");
            }

            CheckForUpdates();
        }

        private void CheckForUpdates()
        {
            // Remove this for store builds!
            //UpdateManager.Register(this, GridCentral.Helpers.Keys.HockeyId_Android);
        }

        private void UnregisterManagers()
        {
            UpdateManager.Unregister();
        }

        protected override void OnDestroy()
        {
            App.Current.MainPage = new ContentPage();
            base.OnDestroy();
            UnregisterManagers();
            GCMService.mIsInForegroundMode = false;

        }

        protected override void OnResume()
        {
            base.OnResume();
            GCMService.mIsInForegroundMode = true;
            

        }

        protected override void OnPause()
        {
            base.OnPause();
            UnregisterManagers();
            GCMService.mIsInForegroundMode = false;
        }
        public void CreateAndShowDialog(String message, String title)
        {
            AlertDialog.Builder builder = new AlertDialog.Builder(this);

            builder.SetMessage(message);
            builder.SetTitle(title);
            //builder.SetNeutralButton("Ok", null);
            builder.Create().Show();
        }

        public override void OnConfigurationChanged(Android.Content.Res.Configuration newConfig)
		{
			base.OnConfigurationChanged(newConfig);

			DeviceOrientationLocator.NotifyOrientationChanged();
		}

	}

}

