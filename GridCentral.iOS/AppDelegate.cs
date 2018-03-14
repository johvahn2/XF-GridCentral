using Foundation;
using UIKit;
using Xamarin.Forms;

using FFImageLoading.Forms.Touch;

using UXDivers.Artina.Shared;
using HockeyApp.iOS;
using CarouselView.FormsPlugin.iOS;
using XLabs.Forms;
using XLabs.Platform.Device;
using XLabs.Ioc;
using XLabs.Platform.Services;
using GridCentral.Elements;
using CarouselView.FormsPlugin.Abstractions;
using Firebase.Core;
using UserNotifications;
using System;
using Firebase.CloudMessaging;
using Plugin.Settings;
using ObjCRuntime;

namespace GridCentral.iOS
{
    // The UIApplicationDelegate for the application. This class is responsible for launching the
    // User Interface of the application, as well as listening (and optionally responding) to application events from iOS.
    [Register ("AppDelegate")]
	public class AppDelegate : XFormsApplicationDelegate, IUNUserNotificationCenterDelegate, IMessagingDelegate // global::Xamarin.Forms.Platform.iOS.FormsApplicationDelegate
    {

        public void DidRefreshRegistrationToken(Messaging messaging, string fcmToken)
        {
            throw new NotImplementedException();
        }

        public override bool FinishedLaunching (UIApplication app, NSDictionary options)
		{
            //var manager = BITHockeyManager.SharedHockeyManager;
            //manager.Configure(GridCentral.Helpers.Keys.HockeyId_IOS);
            //manager.StartManager();
            //manager.Authenticator.AuthenticateInstallation();
            var container = new SimpleContainer();
            container.Register<IDevice>(t => AppleDevice.CurrentDevice);
            container.Register<IDisplay>(t => t.Resolve<IDevice>().Display);
            container.Register<INetwork>(t => t.Resolve<IDevice>().Network);

            Resolver.SetResolver(container.GetResolver());

            global::Xamarin.Forms.Forms.Init ();
            CarouselViewRenderer.Init();
            CachedImageRenderer.Init(); // Initializing FFImageLoading


            UXDivers.Artina.Shared.GrialKit.Init(new ThemeColors(), "GridCentral.iOS.GrialLicense");
            iOS11Workaround();

			// Code for starting up the Xamarin Test Cloud Agent
			#if ENABLE_TEST_CLOUD
			Xamarin.Calabash.Start();
			#endif


			FormsHelper.ForceLoadingAssemblyContainingType(typeof(UXDivers.Effects.Effects));
			FormsHelper.ForceLoadingAssemblyContainingType<UXDivers.Effects.iOS.CircleEffect>();

            LoadApplication (new App ());

            //LoadApplication(UXDivers.Gorilla.iOS.Player.CreateApplication(
            //    new UXDivers.Gorilla.Config("Good Gorilla").RegisterAssembliesFromTypes<UXDivers.Artina.Shared.CircleImage,
            //    GrialShapesFont, XLabs.Forms.Controls.CheckBox, XLabs.Forms.Controls.BindableRadioGroup, BindablePicker, CarouselViewControl>()));


            ////////////Notifactions/////////////////////
            // get permission for notification
            if (UIDevice.CurrentDevice.CheckSystemVersion(10, 0))
            {
                // iOS 10
                var authOptions = UNAuthorizationOptions.Alert | UNAuthorizationOptions.Badge | UNAuthorizationOptions.Sound;
                UNUserNotificationCenter.Current.RequestAuthorization(authOptions, (granted, error) =>
                {
                    Console.WriteLine(granted);
                });

                // For iOS 10 display notification (sent via APNS)
                UNUserNotificationCenter.Current.Delegate = this;
            }
            else
            {

                var allNotificationTypes = UIUserNotificationType.Alert | UIUserNotificationType.Badge | UIUserNotificationType.Sound;
                var settings = UIUserNotificationSettings.GetSettingsForTypes(allNotificationTypes, null);


                UIApplication.SharedApplication.RegisterUserNotificationSettings(settings);
            }

            UIApplication.SharedApplication.RegisterForRemoteNotifications();

            // Firebase component initialize
            Firebase.Core.App.Configure();
            //System.Diagnostics.Debug.WriteLine("Token =" + CrossSettings.Current.GetValueOrDefault<string>("Token"));
            GridCentral.Services.AccountService.Instance.getToken(CrossSettings.Current.GetValueOrDefault<string>("Token"));
            Firebase.InstanceID.InstanceId.Notifications.ObserveTokenRefresh((sender, e) =>
            {
                var newToken = Firebase.InstanceID.InstanceId.SharedInstance.Token;
                 // System.Diagnostics.Debug.WriteLine("xToken =" + newToken);
                 GridCentral.Services.AccountService.Instance.getToken(newToken);
                connectFCM();
            });
            //////////////////////////////////////

            //// Handling Push notification when app is closed if App was opened by Push Notification...
            //if (options != null && options.Keys != null && options.Keys.GetCount() != 0 && options.ContainsKey(new NSString("UIApplicationLaunchOptionsRemoteNotificationKey")))
            //{
            //    NSDictionary UIApplicationLaunchOptionsRemoteNotificationKey = options.ObjectForKey(new NSString("UIApplicationLaunchOptionsRemoteNotificationKey")) as NSDictionary;

            //    //ProcessNotification(UIApplicationLaunchOptionsRemoteNotificationKey, false);  //check here
            //}

            return base.FinishedLaunching (app, options);
		}


        private void iOS11Workaround() // Fix Nav bar item color
        {
            var colors = new ThemeColors();
            var accentColor = colors.GetColor("HeaderAccentColor");

            UIButton.Appearance.TintColor = accentColor;
            UIButton.Appearance.SetTitleColor(accentColor, UIControlState.Normal);

            UITabBar.Appearance.SelectedImageTintColor = UIColor.White;//UIColor.FromRGB(203,34,78); // Tint Tab highlight color
            UITabBar.Appearance.BackgroundColor = UIColor.FromRGB(28, 53, 94); UITabBar.Appearance.BarTintColor = UIColor.FromRGB(28, 53, 94);
            // UIBarButtonItem.Appearance.SetBackButtonTitlePositionAdjustment(new UIOffset(-100, -10), UIBarMetrics.Default);   //Remove Back Button Title

        }


        public override void DidEnterBackground(UIApplication uiApplication)
        {
            Messaging.SharedInstance.Disconnect();
        }

        public override void OnActivated(UIApplication uiApplication)
        {
            connectFCM();
            base.OnActivated(uiApplication);
        }

        public override void RegisteredForRemoteNotifications(UIApplication application, NSData deviceToken)
        {
#if DEBUG
            Firebase.InstanceID.InstanceId.SharedInstance.SetApnsToken(deviceToken, Firebase.InstanceID.ApnsTokenType.Sandbox);
#endif
#if RELEASE
			Firebase.InstanceID.InstanceId.SharedInstance.SetApnsToken(deviceToken, Firebase.InstanceID.ApnsTokenType.Prod);
#endif
        }



        // iOS 9 <=, fire when recieve notification foreground
        public override void DidReceiveRemoteNotification(UIApplication application, NSDictionary userInfo, Action<UIBackgroundFetchResult> completionHandler)
        {
            Messaging.SharedInstance.AppDidReceiveMessage(userInfo);

            // Generate custom event
            NSString[] keys = { new NSString("Event_type") };
            NSObject[] values = { new NSString("Recieve_Notification") };
            var parameters = NSDictionary<NSString, NSObject>.FromObjectsAndKeys(keys, values, keys.Length);

            // Send custom event
            Firebase.Analytics.Analytics.LogEvent("CustomEvent", parameters);

            if (application.ApplicationState == UIApplicationState.Active)
            {
                System.Diagnostics.Debug.WriteLine(userInfo);
                var aps_d = userInfo["aps"] as NSDictionary;
                var alert_d = aps_d["alert"] as NSDictionary;
                var body = alert_d["body"] as NSString;
                var title = alert_d["title"] as NSString;
                debugAlert(title, body);
            }
        }

        // iOS 10, fire when recieve notification foreground
        [Export("userNotificationCenter:willPresentNotification:withCompletionHandler:")]
        public void WillPresentNotification(UNUserNotificationCenter center, UNNotification notification, Action<UNNotificationPresentationOptions> completionHandler)
        {
            var title = notification.Request.Content.Title;
            var body = notification.Request.Content.Body;
            debugAlert(title, body);
        }

        private void connectFCM()
        {
            Messaging.SharedInstance.Connect((error) =>
            {
                if (error == null)
                {
                    //TODO: Change Topic to what is required
                    Messaging.SharedInstance.Subscribe("/topics/all");
                }
                System.Diagnostics.Debug.WriteLine(error != null ? "error occured" : "connect success");
            });
        }

        private void debugAlert(string title, string message)
        {
            var alert = new UIAlertView(title ?? "Title", message ?? "Message", null, "Cancel", "OK");
            alert.Show();
        }



        [Export("application:supportedInterfaceOrientationsForWindow:")]
        public UIInterfaceOrientationMask GetSupportedInterfaceOrientations(UIApplication application, IntPtr forWindow)
        {
            return Plugin.DeviceOrientation.DeviceOrientationImplementation.SupportedInterfaceOrientations;
        }


    }
}