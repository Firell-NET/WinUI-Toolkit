using System.Numerics;

using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;

namespace Firell.Toolkit.WinUI.Controls;

[TemplatePart(Name = nameof(CardBodyPart))]
public sealed class Card : ContentControl
{
    private const string CardBodyPart = "PART_CardBody";

    public Card()
    {
        DefaultStyleKey = typeof(Card);
    }

    public string Header
    {
        get => (string)GetValue(HeaderProperty);
        set => SetValue(HeaderProperty, value);
    }

    public int ShadowDepth
    {
        get => (int)GetValue(ShadowDepthProperty);
        set => SetValue(ShadowDepthProperty, value);
    }

    public bool IsShadowVisible
    {
        get => (bool)GetValue(IsShadowVisibleProperty);
        set => SetValue(IsShadowVisibleProperty, value);
    }

    public static readonly DependencyProperty HeaderProperty =
        DependencyProperty.Register(nameof(Header), typeof(string), typeof(Card), new PropertyMetadata(string.Empty));

    public static readonly DependencyProperty ShadowDepthProperty =
        DependencyProperty.Register(nameof(ShadowDepth), typeof(int), typeof(Card), new PropertyMetadata(10, ShadowDepth_PropertyChanged));

    public static readonly DependencyProperty IsShadowVisibleProperty =
        DependencyProperty.Register(nameof(IsShadowVisible), typeof(bool), typeof(Card), new PropertyMetadata(false, IsShadowVisible_PropertyChanged));

    protected override void OnApplyTemplate()
    {
        ApplyShadowDepth();
    }

    private static void ShadowDepth_PropertyChanged(DependencyObject obj, DependencyPropertyChangedEventArgs e)
    {
        Card control = (Card)obj;

        control.ApplyShadowDepth();
    }

    private static void IsShadowVisible_PropertyChanged(DependencyObject obj, DependencyPropertyChangedEventArgs e)
    {
        Card control = (Card)obj;

        control.ApplyShadowDepth();
    }

    private void ApplyShadowDepth()
    {
        if (GetTemplateChild(CardBodyPart) is Border body)
        {
            body.Translation = IsShadowVisible ? new Vector3(0, 0, ShadowDepth) : new Vector3(0);
        }
    }
 }
