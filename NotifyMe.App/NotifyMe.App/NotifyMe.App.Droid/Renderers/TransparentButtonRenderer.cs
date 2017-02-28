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
                var button = e.NewElement;

                var gradient = new GradientDrawable();
                gradient.SetColor(Android.Graphics.Color.Transparent);
                gradient.SetStroke((int)button.BorderWidth, Colors.Primary.ToAndroid());
                gradient.SetCornerRadius(button.BorderRadius);

                var states = new StateListDrawable();
                states.AddState(new int[] { }, gradient);
                Control.SetBackground(states);

                Control.Touch += (sender, args) =>
                {
                    if (args.Event.Action == MotionEventActions.Down)
                    {
                        Control.SetTextColor(Colors.SecondPrimary.ToAndroid());
                        gradient.SetStroke((int)button.BorderWidth, Colors.SecondPrimary.ToAndroid());
                    }
                    else if (args.Event.Action == MotionEventActions.Up)
                    {
                        Control.SetTextColor(Colors.Primary.ToAndroid());
                        gradient.SetStroke((int)button.BorderWidth, Colors.Primary.ToAndroid());
                    }
                    args.Handled = false;
                };
            }
        }
    }
}