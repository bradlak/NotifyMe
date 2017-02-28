using Xamarin.Forms;

namespace NotifyMe.App.Resources
{
    public static class Colors
    {
        static Colors()
        {
            Primary = Color.FromHex("#FFBF65");
            SecondPrimary = Color.FromHex("#FF9705");
            Background = Color.FromHex("#003338");
            LighterBackground = Color.FromHex("#486669");
        }

        public static Color Primary { get; set; }

        public static Color SecondPrimary { get; set; }

        public static Color Background { get; set; }

        public static Color LighterBackground { get; set; }
    }
}
