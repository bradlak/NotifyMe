using System;
using Android.Graphics;
using NotifyMe.App.Droid;
using NotifyMe.App.Resources;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(Entry), typeof(CustomEntryRenderer))]
namespace NotifyMe.App.Droid
{
	public class CustomEntryRenderer : EntryRenderer
	{
			protected override void OnElementChanged(ElementChangedEventArgs<Entry> e)
			{
				base.OnElementChanged(e);

				if (Control != null)
				{
				Control.SetTextColor(Android.Graphics.Color.White);
				Control.Background.SetColorFilter(Colors.Primary.ToAndroid(), PorterDuff.Mode.SrcAtop);
				}
			}
	}
}
