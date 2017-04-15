using Android.Views;
using Xamarin.Forms.Platform.Android;
using Xamarin.Forms;
using NotifyMe.App.Droid.Renderers;
using Android.Graphics.Drawables;
using NotifyMe.App.Resources;

[assembly: ExportRenderer(typeof(Xamarin.Forms.Button), typeof(TransparentButtonRenderer))]
namespace NotifyMe.App.Droid.Renderers
{
    public class TransparentButtonRenderer : ButtonRenderer
    {
        protected override void OnElementChanged(ElementChangedEventArgs<Xamarin.Forms.Button> e)
        {
            base.OnElementChanged(e);

            if(Control != null)
            {
				var borderWidth = 5;
				var cornerRadius = 40;

                var gradient = new GradientDrawable();
                gradient.SetColor(Android.Graphics.Color.Transparent);
				gradient.SetStroke(borderWidth, Colors.Primary.ToAndroid());
				gradient.SetCornerRadius(cornerRadius);

                var states = new StateListDrawable();
                states.AddState(new int[] { }, gradient);
                Control.SetBackground(states);

                Control.Touch += (sender, args) =>
                {
                    if (args.Event.Action == MotionEventActions.Down)
                    {
                        Control.SetTextColor(Colors.SecondPrimary.ToAndroid());
						gradient.SetStroke(borderWidth, Colors.SecondPrimary.ToAndroid());
                    }
                    else if (args.Event.Action == MotionEventActions.Up)
                    {
                        Control.SetTextColor(Colors.Primary.ToAndroid());
						gradient.SetStroke(borderWidth, Colors.Primary.ToAndroid());
                    }
                    args.Handled = false;
                };
            }
        }
    }
}