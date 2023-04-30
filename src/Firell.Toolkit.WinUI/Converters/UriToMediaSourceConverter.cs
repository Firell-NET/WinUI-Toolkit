using System;

using Microsoft.UI.Xaml.Data;

using Windows.Media.Core;

namespace Firell.Toolkit.WinUI.Converters;

public class UriToMediaSourceConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, string language)
    {
        if (value is Uri uri)
        {
            return MediaSource.CreateFromUri(uri);
        }

        if (value is string stringValue && !string.IsNullOrWhiteSpace(stringValue))
        {
            return MediaSource.CreateFromUri(new Uri(stringValue));
        }

        return value;
    }

    public object ConvertBack(object value, Type targetType, object parameter, string language)
    {
        throw new NotImplementedException();
    }
}
