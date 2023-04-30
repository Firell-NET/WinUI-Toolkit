using System;

using Microsoft.UI.Xaml.Data;

namespace Firell.Toolkit.WinUI.Converters;

public class UriToHostNameConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, string language)
    {
        if (value is Uri uri && uri.IsAbsoluteUri)
        {
            return uri.Host;
        }

        return string.Empty;
    }

    public object ConvertBack(object value, Type targetType, object parameter, string language)
    {
        throw new NotImplementedException();
    }
}
