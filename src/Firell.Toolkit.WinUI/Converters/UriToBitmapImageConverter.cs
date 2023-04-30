using System;

using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Media.Imaging;

namespace Firell.Toolkit.WinUI.Converters;

public class UriToBitmapImageConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, string language)
    {
        if (value is Uri uri)
        {
            return new BitmapImage(uri);
        }

        if (value is string stringValue && !string.IsNullOrWhiteSpace(stringValue))
        {
            return new BitmapImage(new Uri(stringValue));
        }

        return new BitmapImage();
    }

    public object ConvertBack(object value, Type targetType, object parameter, string language)
    {
        throw new NotImplementedException();
    }
}
