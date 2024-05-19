using System;
using Xamarin.Forms;
using System.Globalization;

namespace Magazine.Converters
{
    public class UriToImageSourceConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is string uri && !string.IsNullOrWhiteSpace(uri))
            {
                return ImageSource.FromUri(new Uri(uri));
            }
            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
