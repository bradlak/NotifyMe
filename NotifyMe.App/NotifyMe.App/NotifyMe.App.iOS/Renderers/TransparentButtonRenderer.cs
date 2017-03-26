using NotifyMe.App.iOS;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;
using NotifyMe.App.Resources;
using System;

[assembly: ExportRenderer(typeof(Button), typeof(TransparentButtonRenderer))]
namespace NotifyMe.App.iOS
{
	public class TransparentButtonRenderer : ButtonRenderer
	{
		UIButton button;
		Button formsButton;

		public override void Draw(CoreGraphics.CGRect rect)
		{
			base.Draw(rect);

			button.Layer.MasksToBounds = true;
			button.Layer.BorderColor = Colors.Primary.ToCGColor();
			button.Layer.BorderWidth = (nfloat)formsButton.BorderWidth;
			button.SetTitleColor(Colors.Primary.ToUIColor(), UIControlState.Normal);
		}

		protected override void OnElementChanged(ElementChangedEventArgs<Button> e)
		{
			base.OnElementChanged(e);

			if (Control != null)
			{
				formsButton = e.NewElement;
				button = (UIButton)Control;
			}
		}
	}
}
