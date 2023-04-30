using System;

using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Data;

namespace Firell.Toolkit.WinUI.Converters;

public class VisibilityNegationConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, string language)
    {
        if (value is not Visibility visibility)
        {
            return value;
        }

        return visibility == Visibility.Visible ? Visibility.Collapsed : Visibility.Visible;
    }

    public object ConvertBack(object value, Type targetType, object parameter, string language)
    {
        throw new NotImplementedException();
    }
}
