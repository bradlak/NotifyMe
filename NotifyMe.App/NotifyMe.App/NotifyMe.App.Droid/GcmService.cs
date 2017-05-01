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
using System.Threading.Tasks;

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
            : base(PushHandlerBroadcastReceiver.SenderIds) { }

        protected override void OnRegistered(Context context, string registrationId)
        {
            RegistrationID = registrationId;

            DeviceRegistration registration = new DeviceRegistration();
            registration.Handle = registrationId;
            registration.Tag = MobileServiceClientWrapper.Instance.CurrentUser.Id;
            var serialized = JsonConvert.SerializeObject(registration);

            var jtoken = JToken.Parse(serialized);

            StringContent content = new StringContent(serialized);

			Task.Run(async () => 
			         await MobileServiceClientWrapper.Instance.Client.InvokeApiAsync("notifications/register", jtoken, HttpMethod.Post, null));
        }

        protected override void OnMessage(Context context, Intent intent)
        {
            string message = intent.Extras.GetString("message");
            string subject = intent.Extras.GetString("subject");
            if (!string.IsNullOrEmpty(message))
            {
                CreateNotification(this, subject, message);
                return;
            }
        }

        private void CreateNotification(GcmService instance, string subject, string message)
        {
            var notificationManager = GetSystemService(Context.NotificationService) as NotificationManager;

            var uiIntent = new Intent(instance, typeof(MainActivity));

            NotificationCompat.Builder builder = new NotificationCompat.Builder(instance);
            var notification = builder.SetContentIntent(PendingIntent.GetActivity(instance, 0, uiIntent, PendingIntentFlags.OneShot))
			        .SetSmallIcon(Resource.Drawable.icon)
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

    [BroadcastReceiver(Permission = Constants.PERMISSION_GCM_INTENTS)]
    [IntentFilter(new string[] { Constants.INTENT_FROM_GCM_MESSAGE }, Categories = new string[] { "@PACKAGE_NAME@" })]
    [IntentFilter(new string[] { Constants.INTENT_FROM_GCM_REGISTRATION_CALLBACK }, Categories = new string[] { "@PACKAGE_NAME@" })]
    [IntentFilter(new string[] { Constants.INTENT_FROM_GCM_LIBRARY_RETRY }, Categories = new string[] { "@PACKAGE_NAME@" })]
    public class PushHandlerBroadcastReceiver : GcmBroadcastReceiverBase<GcmService>
    {
        public static string[] SenderIds = new string[] { "<SENDER ID>" };
    }
}