using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;

using Microsoft.UI.Xaml.Data;

namespace Firell.Toolkit.WinUI.Converters;

public class DisplayAttributeConverter : IValueConverter
{
    public object? Convert(object value, Type targetType, object parameter, string language)
    {
        if (value == null)
        {
            return value;
        }

        MemberInfo? property = value.GetType().GetMember(value.ToString() ?? string.Empty).FirstOrDefault();
        if (property == null)
        {
            return value;
        }

        DisplayAttribute? attribute = property.GetCustomAttribute<DisplayAttribute>();
        if (attribute == null)
        {
            return value;
        }

        string attributeProperty = parameter as string ?? "Name";
        return attribute.GetType().GetProperty(attributeProperty)?.GetValue(attribute, null);
    }

    public object ConvertBack(object value, Type targetType, object parameter, string language)
    {
        throw new NotImplementedException();
    }
}
