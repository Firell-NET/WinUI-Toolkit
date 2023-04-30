using System;

using Humanizer;

using Microsoft.UI.Xaml.Data;

namespace Firell.Toolkit.WinUI.Converters;

public class HumanizerConverter : IValueConverter
{
    public object? Convert(object value, Type targetType, object parameter, string language)
    {
        if (value is string stringValue)
        {
            return stringValue.Humanize();
        }

        if (value is DateTime dateTimeValue)
        {
            bool useUtcDate = false;
            if (parameter is string stringParameter)
            {
                bool.TryParse(stringParameter, out useUtcDate);
            }

            return dateTimeValue.Humanize(useUtcDate);
        }

        if (value is TimeSpan timeSpanValue)
        {
            return timeSpanValue.Humanize();
        }

        return value;
    }

    public object ConvertBack(object value, Type targetType, object parameter, string language)
    {
        throw new NotImplementedException();
    }
}
