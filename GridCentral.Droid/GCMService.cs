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
using Gcm.Client;
using GridCentral.Models;
using Android.Media;
using Android.Support.V4.App;
using Com.Bumptech.Glide;
using Com.Bumptech.Glide.Request.Target;
using Android.Graphics.Drawables;
using Android.Graphics;
using System.Net;

[assembly: Permission(Name = "@PACKAGE_NAME@.permission.C2D_MESSAGE")]
[assembly: UsesPermission(Name = "@PACKAGE_NAME@.permission.C2D_MESSAGE")]
[assembly: UsesPermission(Name = "com.google.android.c2dm.permission.RECEIVE")]
[assembly: UsesPermission(Name = "android.permission.INTERNET")]
[assembly: UsesPermission(Name = "android.permission.WAKE_LOCK")]
//GET_ACCOUNTS is only needed for android versions 4.0.3 and below
[assembly: UsesPermission(Name = "android.permission.GET_ACCOUNTS")]

namespace GridCentral.Droid
{

    [BroadcastReceiver(Permission = Gcm.Client.Constants.PERMISSION_GCM_INTENTS)]
    [IntentFilter(new string[] { Gcm.Client.Constants.INTENT_FROM_GCM_MESSAGE }, Categories = new string[] { "@PACKAGE_NAME@" })]
    [IntentFilter(new string[] { Gcm.Client.Constants.INTENT_FROM_GCM_REGISTRATION_CALLBACK }, Categories = new string[] { "@PACKAGE_NAME@" })]
    [IntentFilter(new string[] { Gcm.Client.Constants.INTENT_FROM_GCM_LIBRARY_RETRY }, Categories = new string[] { "@PACKAGE_NAME@" })]
    public class PushHandlerBroadcastReceiver : GcmBroadcastReceiverBase<GCMService>
    {
        public static string[] SENDER_IDS = new string[] { "563755815769" };
    }

    [Service]
    public class GCMService : GcmServiceBase
    {
        public static bool mIsInForegroundMode;

        string type; string why; string objecter;string imgUrl;string title;

        protected override void OnError(Context context, string errorId)
        {
            //
        }

        protected override void OnMessage(Context context, Intent intent)
        {
            //FCM Message Catcher
            var FCMmsg = intent.Extras.GetString("gcm.notification.body");
            title = intent.Extras.GetString("gcm.notification.title");
            //Server Message Catcher
            type = intent.Extras.GetString("type");
            why = intent.Extras.GetString("why");
            objecter = intent.Extras.GetString("objecter");
            imgUrl = intent.Extras.GetString("img");
            //title = intent.Extras.GetString("title");

            mPushNotify noti = new mPushNotify()
            {
                Messgae = FCMmsg,
                Why = why,
                Objecter = objecter,
                Type = type,
                Title = title,
                ImgUrl = null
            };
            if (!String.IsNullOrEmpty(imgUrl))
            {
                noti.ImgUrl = imgUrl;
            }

            createNotification(FCMmsg.ToString(), noti);

        }

        protected override void OnRegistered(Context context, string registrationId)
        {
            System.Diagnostics.Debug.WriteLine("GRID---|" + registrationId);

            GridCentral.Services.AccountService.Instance.getToken(registrationId);
        }

        protected override void OnUnRegistered(Context context, string registrationId)
        {
            //
        }


        void createNotification(string body, mPushNotify info)
        {
            var intent = new Intent(this, typeof(MainActivity));

            intent.PutExtra("type", info.Type); intent.PutExtra("why", info.Why); intent.PutExtra("objecter", info.Objecter); intent.PutExtra("message", info.Messgae);

            intent.AddFlags(ActivityFlags.ClearTop);
            var pendingIntent = PendingIntent.GetActivity(this, 0, intent, PendingIntentFlags.OneShot);

            var defaultSoundUri = RingtoneManager.GetDefaultUri(RingtoneType.Notification);

            #region Priority Numbers

            //
            // Summary:
            //     ///
            //     Lowest Android.App.Notification.Priority; these items might not be shown to the
            //     user except under special /// circumstances, such as detailed notification logs.
            //     ///
            //     ///
            // Min = -2,
            //
            // Summary:
            //     ///
            //     Lower Android.App.Notification.Priority, for items that are less important. The
            //     UI may choose to show these /// items smaller, or at a different position in
            //     the list, compared with your app's /// Android.App.Notification.PriorityDefault
            //     items. ///
            //     ///
            //Low = -1,
            //
            // Summary:
            //     ///
            //     Default notification Android.App.Notification.Priority. If your application does
            //     not prioritize its own /// notifications, use this value for all notifications.
            //     ///
            //     ///
            //Default = 0,
            //
            // Summary:
            //     ///
            //     Higher Android.App.Notification.Priority, for more important notifications or
            //     alerts. The UI may choose to /// show these items larger, or at a different position
            //     in notification lists, compared with /// your app's Android.App.Notification.PriorityDefault
            //     items. ///
            //     ///
            // High = 1,
            //
            // Summary:
            //     ///
            //     Highest Android.App.Notification.Priority, for your application's most important
            //     items that require the /// user's prompt attention or input. ///
            //     ///
            //Max = 2
            #endregion
            var notificationBuilder = new NotificationCompat.Builder(this);

            if (!String.IsNullOrEmpty(info.ImgUrl))
            {
                var imageBitmap = GetImageBitmapFromUrl(info.ImgUrl);



                notificationBuilder = new NotificationCompat.Builder(this)
                   .SetPriority(1)
                   .SetSmallIcon(Resource.Drawable.ic_logo)
                   .SetContentTitle(info.Title)
                   .SetContentText(body)
                   .SetAutoCancel(true)
                   .SetStyle(new NotificationCompat.BigPictureStyle().BigPicture(imageBitmap).SetBigContentTitle(body))
                   .SetSound(defaultSoundUri)
                   .SetContentIntent(pendingIntent);

            }
            else {

                notificationBuilder = new NotificationCompat.Builder(this)
                   .SetPriority(1)
                   .SetSmallIcon(Resource.Drawable.ic_logo)
                   .SetContentTitle("Grid Central")
                   .SetContentText(body)
                   .SetAutoCancel(true)
                   .SetStyle(new NotificationCompat.BigTextStyle().BigText(body))
                   .SetSound(defaultSoundUri)
                   .SetVibrate(new long[] { 1000, 1000})
                   .SetContentIntent(pendingIntent);

            }



                //var notify = new NotificationCompat.BigTextStyle(notificationBuilder).BigText(body);




            var notificationManger = NotificationManager.FromContext(this);
            notificationManger.Notify(0, notificationBuilder.Build());
        }

        private Bitmap GetImageBitmapFromUrl(string url)
        {
            Bitmap imageBitmap = null;

            using (var webClient = new WebClient())
            {
                var imageBytes = webClient.DownloadData(url);
                if (imageBytes != null && imageBytes.Length > 0)
                {
                    imageBitmap = BitmapFactory.DecodeByteArray(imageBytes, 0, imageBytes.Length);
                }
            }

            return imageBitmap;
        }
    }
}