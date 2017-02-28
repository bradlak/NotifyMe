using System;
using System.Globalization;
using Xamarin.Forms;

namespace NotifyMe.App.Converters
{
    public class SelectedToBackgroundColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is bool)
            {
                return (bool)value ? Color.Red : Color.White;
            }
            else throw new ArgumentException("The value for converting should be boolean.");
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
