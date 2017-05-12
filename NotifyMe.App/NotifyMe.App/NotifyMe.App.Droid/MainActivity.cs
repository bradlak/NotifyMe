using System;
using Android.App;
using Android.Content.PM;
using Android.Views;
using Android.OS;
using NotifyMe.App.Infrastructure;
using Microsoft.WindowsAzure.MobileServices;
using System.Threading.Tasks;
using ImageCircle.Forms.Plugin.Droid;
using Gcm.Client;
using NotifyMe.App.Infrastructure.Messages;

namespace NotifyMe.App.Droid
{
    [Activity(MainLauncher = true, Label = "NotifyMe!", Icon = "@drawable/icon", Theme = "@style/MainTheme", WindowSoftInputMode = SoftInput.AdjustResize, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        public static MainActivity Instance;

        protected override void OnCreate(Bundle bundle)
        {
            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;
            Instance = this;

            base.OnCreate(bundle);

            Microsoft.WindowsAzure.MobileServices.CurrentPlatform.Init();

            global::Xamarin.Forms.Forms.Init(this, bundle);
            ImageCircleRenderer.Init();
            LoadApplication(new App());

            if (Build.VERSION.SdkInt > Build.VERSION_CODES.Kitkat)
            {
                Window.SetStatusBarColor(Android.Graphics.Color.Argb(255, 0, 51, 56));
            }

            GalaSoft.MvvmLight.Messaging.Messenger.Default.Register<RegistrationMessage>(this, MessageHandlerExecute);
        }

        private void MessageHandlerExecute(RegistrationMessage obj)
        {
            GcmClient.CheckDevice(this);
            GcmClient.CheckManifest(this);

            GcmClient.Register(this, PushHandlerBroadcastReceiver.SenderIds);
        }
    }
}

