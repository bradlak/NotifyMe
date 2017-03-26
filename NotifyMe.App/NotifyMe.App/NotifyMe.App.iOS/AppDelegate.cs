using System;
using System.Collections.Generic;
using System.Linq;

using Foundation;
using UIKit;
using ImageCircle.Forms.Plugin.iOS;
using NotifyMe.App.Infrastructure;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.MobileServices;

namespace NotifyMe.App.iOS
{
    // The UIApplicationDelegate for the application. This class is responsible for launching the 
    // User Interface of the application, as well as listening (and optionally responding) to 
    // application events from iOS.
    [Register("AppDelegate")]
	public partial class AppDelegate : global::Xamarin.Forms.Platform.iOS.FormsApplicationDelegate, IAuthenticate
    {
		private MobileServiceUser user;

		public async Task<bool> Authenticate()
		{
			bool success = false;
			try
			{

				user = await MobileServiceClientWrapper.Instance.Client.LoginAsync(
					UIApplication.SharedApplication.KeyWindow.RootViewController,
					MobileServiceAuthenticationProvider.Facebook);

				if (user != null)
				{
					await Task.Delay(100);
					success = true;
				}
			}
			catch (Exception ex)
			{

			}

			return success;
		}

		//
		// This method is invoked when the application has loaded and is ready to run. In this 
		// method you should instantiate the window, load the UI into it and then make the window
		// visible.
		//
		// You have 17 seconds to return from this method, or iOS will terminate your application.
		//
		public override bool FinishedLaunching(UIApplication app, NSDictionary options)
        {
			App.Init((IAuthenticate)this);
			Microsoft.WindowsAzure.MobileServices.CurrentPlatform.Init();

            global::Xamarin.Forms.Forms.Init();
            ImageCircleRenderer.Init();
            LoadApplication(new App());

            return base.FinishedLaunching(app, options);
        }
    }
}
