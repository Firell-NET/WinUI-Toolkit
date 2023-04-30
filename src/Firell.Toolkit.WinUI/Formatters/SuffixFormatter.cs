using System;

using Humanizer;

using Windows.Globalization.NumberFormatting;

namespace Firell.Toolkit.WinUI.Formatters;

public class SuffixFormatter : INumberFormatter2, INumberParser
{
    public string Suffix { get; set; } = string.Empty;

    public bool UseHumanizer { get; set; }

    public string FormatDouble(double value)
    {
        // Rounding is due to precision floating point bug.
        // https://github.com/microsoft/microsoft-ui-xaml/issues/3959
        return HumanizeDouble(Math.Round(value, 6)).Trim();
    }

    public string FormatInt(long value)
    {
        return HumanizeInt(value).Trim();
    }

    public string FormatUInt(ulong value)
    {
        return HumanizeUInt(value).Trim();
    }

    public double? ParseDouble(string text)
    {
        return double.TryParse(text.Split(" ")[0], out double result) ? result : 0;
    }

    public long? ParseInt(string text)
    {
        return long.TryParse(text.Split(" ")[0], out long result) ? result : 0;
    }

    public ulong? ParseUInt(string text)
    {
        return ulong.TryParse(text.Split(" ")[0], out ulong result) ? result : 0;
    }

    private string HumanizeDouble(double value)
    {
        return UseHumanizer ? Suffix.ToQuantity(value) : $"{value} {Suffix}";
    }

    private string HumanizeInt(long value)
    {
        return UseHumanizer ? Suffix.ToQuantity(value) : $"{value} {Suffix}";
    }

    private string HumanizeUInt(ulong value)
    {
        return UseHumanizer ? Suffix.ToQuantity(value) : $"{value} {Suffix}";
    }
}
