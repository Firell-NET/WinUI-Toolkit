using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Markup;

namespace Firell.Toolkit.WinUI.Controls;

[ContentProperty(Name = nameof(Glyph))]
public class LayeredFontIcon : Control
{
    public LayeredFontIcon()
    {
        DefaultStyleKey = typeof(LayeredFontIcon);
    }

    public string Glyph
    {
        get => (string)GetValue(GlyphProperty);
        set => SetValue(GlyphProperty, value);
    }

    public string BackgroundGlyph
    {
        get => (string)GetValue(BackgroundGlyphProperty);
        set => SetValue(BackgroundGlyphProperty, value);
    }

    public static readonly DependencyProperty GlyphProperty =
        DependencyProperty.Register(nameof(Glyph), typeof(string), typeof(LayeredFontIcon), new PropertyMetadata(string.Empty));

    public static readonly DependencyProperty BackgroundGlyphProperty =
        DependencyProperty.Register(nameof(BackgroundGlyph), typeof(string), typeof(LayeredFontIcon), new PropertyMetadata(string.Empty));
}
