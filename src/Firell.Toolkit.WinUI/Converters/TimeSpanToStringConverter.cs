using System;

using Microsoft.UI.Xaml.Data;

namespace Firell.Toolkit.WinUI.Converters
{
    public class TimeSpanToStringConverter : IValueConverter
    {
        public object? Convert(object value, Type targetType, object parameter, string language)
        {
            if (value is not TimeSpan timeSpanValue)
            {
                return $"N/A";
            }

            if (parameter is string format)
            {
                return timeSpanValue.ToString(format);
            }

            if (timeSpanValue.Days > 0)
            {
                return timeSpanValue.ToString("dd\\:hh\\:mm\\:ss");
            }
            else if (timeSpanValue.Hours > 0)
            {
                return timeSpanValue.ToString("hh\\:mm\\:ss");
            }
            else
            {
                return timeSpanValue.ToString("mm\\:ss");
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            if (value is not string stringValue)
            {
                return TimeSpan.Zero;
            }

            return TimeSpan.Parse(stringValue);
        }
    }
}
