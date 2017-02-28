using Android.App;
using Android.Content;
using Android.Media;
using Android.Support.V4.App;
using Android.Util;
using Gcm.Client;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NotifyMe.App.Infrastructure;
using NotifyMe.App.Models;
using System;
using System.Net.Http;

[assembly: Permission(Name = "@PACKAGE_NAME@.permission.C2D_MESSAGE")]
[assembly: UsesPermission(Name = "@PACKAGE_NAME@.permission.C2D_MESSAGE")]
[assembly: UsesPermission(Name = "com.google.android.c2dm.permission.RECEIVE")]
[assembly: UsesPermission(Name = "android.permission.INTERNET")]
[assembly: UsesPermission(Name = "android.permission.WAKE_LOCK")]
namespace NotifyMe.App.Droid
{
    [Service]
    public class GcmService : GcmServiceBase
    {
        public static string RegistrationID { get; private set; }

        public GcmService()
            : base(PushHandlerBroadcastReceiver.SENDER_IDS) { }

        protected override void OnRegistered(Context context, string registrationId)
        {
            RegistrationID = registrationId;

            DeviceRegistration registration = new DeviceRegistration();
            registration.Handle = registrationId;
            registration.Tag = MobileServiceClientWrapper.Instance.CurrentUser.Id;
            var serialized = JsonConvert.SerializeObject(registration);

            var jtoken = JToken.Parse(serialized);

            StringContent content = new StringContent(serialized);

            try
            {
                var task = MobileServiceClientWrapper.Instance.Client.InvokeApiAsync("notifications/register", jtoken, HttpMethod.Post, null).Result;
            }
            catch (Exception ex)
            {
                // do something
            }
        }

        protected override void OnMessage(Context context, Intent intent)
        {
            string message = intent.Extras.GetString("message");
            string subject = intent.Extras.GetString("subject");
            if (!string.IsNullOrEmpty(message))
            {
                CreateNotification(subject, message);
                return;
            }
        }

        private void CreateNotification(string subject, string message)
        {
            var notificationManager = GetSystemService(Context.NotificationService) as NotificationManager;

            var uiIntent = new Intent(this, typeof(MainActivity));

            NotificationCompat.Builder builder = new NotificationCompat.Builder(this);

            var notification = builder.SetContentIntent(PendingIntent.GetActivity(this, 0, uiIntent, PendingIntentFlags.OneShot))
                    .SetSmallIcon(Android.Resource.Drawable.SymActionEmail)
                    .SetStyle(new NotificationCompat.BigTextStyle().BigText(message))
                    .SetAutoCancel(true)
                    .SetContentTitle(subject)
                    .SetContentText(message)
                    .SetSound(RingtoneManager.GetDefaultUri(RingtoneType.Notification))
                    .SetAutoCancel(true).Build();

            notificationManager.Notify((int)DateTime.Now.Ticks, notification);
        }

        protected override void OnUnRegistered(Context context, string registrationId)
        {
            Log.Error("PushHandlerBroadcastReceiver", "Unregistered RegisterationId : " + registrationId);
        }

        protected override void OnError(Context context, string errorId)
        {
            Log.Error("PushHandlerBroadcastReceiver", "GCM Error: " + errorId);
        }
    }

        [BroadcastReceiver(Permission = Gcm.Client.Constants.PERMISSION_GCM_INTENTS)]
    [IntentFilter(new string[] { Gcm.Client.Constants.INTENT_FROM_GCM_MESSAGE }, Categories = new string[] { "@PACKAGE_NAME@" })]
    [IntentFilter(new string[] { Gcm.Client.Constants.INTENT_FROM_GCM_REGISTRATION_CALLBACK }, Categories = new string[] { "@PACKAGE_NAME@" })]
    [IntentFilter(new string[] { Gcm.Client.Constants.INTENT_FROM_GCM_LIBRARY_RETRY }, Categories = new string[] { "@PACKAGE_NAME@" })]
    public class PushHandlerBroadcastReceiver : GcmBroadcastReceiverBase<GcmService>
    {
        public static string[] SENDER_IDS = new string[] { "" };
    }
}