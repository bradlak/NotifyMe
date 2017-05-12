using System;
using System.Collections.Generic;
using System.Linq;

using Foundation;
using UIKit;
using ImageCircle.Forms.Plugin.iOS;
using NotifyMe.App.Infrastructure;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.MobileServices;
using NotifyMe.App.Models;
using WindowsAzure.Messaging;
using NotifyMe.App.Infrastructure.Messages;

namespace NotifyMe.App.iOS
{
	// The UIApplicationDelegate for the application. This class is responsible for launching the 
	// User Interface of the application, as well as listening (and optionally responding) to 
	// application events from iOS.
	[Register("AppDelegate")]
	public partial class AppDelegate : global::Xamarin.Forms.Platform.iOS.FormsApplicationDelegate
	{
		//
		// This method is invoked when the application has loaded and is ready to run. In this 
		// method you should instantiate the window, load the UI into it and then make the window
		// visible.
		//
		// You have 17 seconds to return from this method, or iOS will terminate your application.
		//
		public override bool FinishedLaunching(UIApplication app, NSDictionary options)
		{
			Microsoft.WindowsAzure.MobileServices.CurrentPlatform.Init();

			global::Xamarin.Forms.Forms.Init();
			ImageCircleRenderer.Init();
			LoadApplication(new App());

			GalaSoft.MvvmLight.Messaging.Messenger.Default.Register<RegistrationMessage>(this, MessageHandlerExecute);
			return base.FinishedLaunching(app, options);
		}

		public override void RegisteredForRemoteNotifications(UIApplication application, NSData deviceToken)
		{
			var Hub = new SBNotificationHub("","");

			Hub.UnregisterAllAsync(deviceToken, (error) =>
			{
				if (error != null)
				{
					Console.WriteLine("Error calling Unregister: {0}", error.ToString());
					return;
				}

				NSSet tags = new NSSet(MobileServiceClientWrapper.Instance.CurrentUser.Id);
				Hub.RegisterNativeAsync(deviceToken, tags, (errorCallback) =>
				{
					if (errorCallback != null)
						Console.WriteLine("RegisterNativeAsync error: " + errorCallback.ToString());
				});
			});
		}

		public override void ReceivedRemoteNotification(UIApplication application, NSDictionary userInfo)
		{
			ProcessNotification(userInfo, false);
		}

		void ProcessNotification(NSDictionary options, bool fromFinishedLaunching)
		{
			if (null != options && options.ContainsKey(new NSString("aps")))
			{
				NSDictionary aps = options.ObjectForKey(new NSString("aps")) as NSDictionary;

				string subject = string.Empty;
				string message = string.Empty;

				if (aps.ContainsKey(new NSString("subject")) && aps.ContainsKey(new NSString("message")))
				{
					subject = (aps[new NSString("subject")] as NSString).ToString();
					message = (aps[new NSString("message")] as NSString).ToString();

					//If this came from the ReceivedRemoteNotification while the app was running,
					// we of course need to manually process things like the sound, badge, and alert.
					if (!fromFinishedLaunching)
					{
						//Manually show an alert
						if (!string.IsNullOrEmpty(subject) && !string.IsNullOrEmpty(message))
						{
							UIAlertView avAlert = new UIAlertView(subject, message, null, "OK", null);
							avAlert.Show();
						}
					}
				}
			}
		}

		private void MessageHandlerExecute(RegistrationMessage obj)
		{
			if (UIDevice.CurrentDevice.CheckSystemVersion(8, 0))
			{
				var pushSettings = UIUserNotificationSettings.GetSettingsForTypes(
					   UIUserNotificationType.Alert | UIUserNotificationType.Badge | UIUserNotificationType.Sound,
					   new NSSet());

				UIApplication.SharedApplication.RegisterUserNotificationSettings(pushSettings);
				UIApplication.SharedApplication.RegisterForRemoteNotifications();
			}
			else
			{
				UIRemoteNotificationType notificationTypes = UIRemoteNotificationType.Alert | UIRemoteNotificationType.Badge | UIRemoteNotificationType.Sound;
				UIApplication.SharedApplication.RegisterForRemoteNotificationTypes(notificationTypes);
			}
		}
	}
}
