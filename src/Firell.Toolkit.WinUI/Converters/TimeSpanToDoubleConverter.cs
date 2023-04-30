using System;

using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Data;

namespace Firell.Toolkit.WinUI.Converters;

public enum TimeUnit { Milliseconds, Seconds, Minutes, Hours, Days }

public class TimeSpanToDoubleConverter : DependencyObject, IValueConverter
{
    public TimeUnit TimeUnit { get; set; }

    public object Convert(object value, Type targetType, object parameter, string language)
    {
        if (value is not TimeSpan timeSpanValue)
        {
            return 0d;
        }

        switch (TimeUnit)
        {
            case TimeUnit.Milliseconds:
            {
                return timeSpanValue.TotalMilliseconds;
            }

            case TimeUnit.Seconds:
            {
                return timeSpanValue.TotalSeconds;
            }

            case TimeUnit.Minutes:
            {
                return timeSpanValue.TotalMinutes;
            }

            case TimeUnit.Hours:
            {
                return timeSpanValue.TotalHours;
            }

            case TimeUnit.Days:
            {
                return timeSpanValue.TotalDays;
            }

            default:
            {
                return timeSpanValue.TotalSeconds;
            }
        }
    }

    public object ConvertBack(object value, Type targetType, object parameter, string language)
    {
        if (value is not double doubleValue)
        {
            return TimeSpan.Zero;
        }

        switch (TimeUnit)
        {
            case TimeUnit.Milliseconds:
            {
                return TimeSpan.FromMilliseconds(doubleValue);
            }

            case TimeUnit.Seconds:
            {
                return TimeSpan.FromSeconds(doubleValue);
            }

            case TimeUnit.Minutes:
            {
                return TimeSpan.FromMinutes(doubleValue);
            }

            case TimeUnit.Hours:
            {
                return TimeSpan.FromHours(doubleValue);
            }

            case TimeUnit.Days:
            {
                return TimeSpan.FromDays(doubleValue);
            }

            default:
            {
                return TimeSpan.FromSeconds(doubleValue);
            }
        }
    }
}
