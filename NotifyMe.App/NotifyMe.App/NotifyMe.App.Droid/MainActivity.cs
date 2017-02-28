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
    [Activity(MainLauncher = true, Label = "NotifyMe!", Icon = "@drawable/icon", Theme = "@style/MainTheme", ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity, IAuthenticate
    {
        private MobileServiceUser user;

        public async Task<bool> Authenticate()
        {
            var success = false;
            try
            {
                user = await MobileServiceClientWrapper.Instance.Client.LoginAsync(this,
                    MobileServiceAuthenticationProvider.Facebook);

                if (user != null)
                {
                    await Task.Delay(100);
                    success = true;
                }
            }
            catch (Exception ex)
            {
                // do something
            }

            return success;
        }

        protected override void OnCreate(Bundle bundle)
        {
            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;

            base.OnCreate(bundle);

            App.Init((IAuthenticate)this);
            Microsoft.WindowsAzure.MobileServices.CurrentPlatform.Init();

            global::Xamarin.Forms.Forms.Init(this, bundle);
            ImageCircleRenderer.Init();
            LoadApplication(new App());
            Window.SetStatusBarColor(Android.Graphics.Color.Argb(255, 0, 51, 56));

            GalaSoft.MvvmLight.Messaging.Messenger.Default.Register<RegistrationMessage>(this, MessageHandlerExecute);
        }

        private void MessageHandlerExecute(RegistrationMessage obj)
        {
            try
            {
                GcmClient.CheckDevice(this);
                GcmClient.CheckManifest(this);

                System.Diagnostics.Debug.WriteLine("Registering...");
                GcmClient.Register(this, PushHandlerBroadcastReceiver.SENDER_IDS);
            }
            catch (Java.Net.MalformedURLException)
            {
                // Do something
            }
            catch (Exception e)
            {
                // Do something
            }
        }
    }
}

