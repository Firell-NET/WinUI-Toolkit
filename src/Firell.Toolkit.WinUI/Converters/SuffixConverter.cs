using System;

using Humanizer;

using Microsoft.UI.Xaml.Data;

namespace Firell.Toolkit.WinUI.Converters;

public class SuffixConverter : IValueConverter
{
    public string Suffix { get; set; } = string.Empty;

    public bool UseHumanizer { get; set; }

    public bool Pluralize { get; set; }

    public object Convert(object value, Type targetType, object parameter, string language)
    {
        if (parameter is string suffix)
        {
            Suffix = suffix;
        }

        return HumanizeValue(value).Trim();
    }

    public object ConvertBack(object value, Type targetType, object parameter, string language)
    {
        throw new NotImplementedException();
    }

    private string HumanizeValue(object value)
    {
        if (value is int quantity && UseHumanizer)
        {
            return $"{quantity.ToMetric()} {(Pluralize ? Suffix.ToQuantity(quantity, ShowQuantityAs.None) : Suffix)}";
        }

        return $"{value} {Suffix}";
    }
}
