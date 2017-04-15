using NotifyMe.App.iOS;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;
using NotifyMe.App.Resources;

[assembly: ExportRenderer(typeof(Button), typeof(TransparentButtonRenderer))]
namespace NotifyMe.App.iOS
{
	public class TransparentButtonRenderer : ButtonRenderer
	{
		UIButton button;

		public override void Draw(CoreGraphics.CGRect rect)
		{
			base.Draw(rect);

			button.Layer.MasksToBounds = true;
			button.Layer.BorderColor = Colors.Primary.ToCGColor();
			button.Layer.CornerRadius = 20;
			button.Layer.BorderWidth = 3;
			button.SetTitleColor(Colors.Primary.ToUIColor(), UIControlState.Normal);
			button.TintColor = Colors.Background.ToUIColor();

			button.TouchDown += (sender, e) =>
			{
				button.Layer.BorderColor = Colors.SecondPrimary.ToCGColor();
			};

			button.TouchUpInside += (sender, e) =>
			{
				button.Layer.BorderColor = Colors.Primary.ToCGColor();
			};
		}

		protected override void OnElementChanged(ElementChangedEventArgs<Button> e)
		{
			base.OnElementChanged(e);

			if (Control != null)
			{
				button = (UIButton)Control;
			}
		}
	}
}
