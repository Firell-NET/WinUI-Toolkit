using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Markup;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Media.Imaging;

namespace Firell.Toolkit.WinUI.Controls;

[TemplatePart(Name = nameof(SwapImageHiddenImagePart))]
[TemplatePart(Name = nameof(SwapImageDisplayedImagePart))]
[TemplatePart(Name = nameof(ImageShimmerPart))]
[ContentProperty(Name = nameof(Source))]
public class SwapImage : Control
{
    protected const string SwapImageHiddenImagePart = "PART_HiddenImage";
    protected const string SwapImageDisplayedImagePart = "PART_DisplayedImage";
    protected const string ImageShimmerPart = "PART_ImageShimmer";

    private Image? _hiddenImage;
    private Image? _displayedImage;

    public event RoutedEventHandler? ImageOpened;
    public event ExceptionRoutedEventHandler? ImageFailed;

    public SwapImage()
    {
        DefaultStyleKey = typeof(SwapImage);
    }

    public ImageSource Source
    {
        get => (ImageSource)GetValue(SourceProperty);
        set => SetValue(SourceProperty, value);
    }

    public Stretch Stretch
    {
        get => (Stretch)GetValue(StretchProperty);
        set => SetValue(StretchProperty, value);
    }

    public Thickness Ninegrid
    {
        get => (Thickness)GetValue(NinegridProperty);
        set => SetValue(NinegridProperty, value);
    }

    public bool IsOpened
    {
        get => (bool)GetValue(IsOpenedProperty);
        private set => SetValue(IsOpenedProperty, value);
    }

    public static readonly DependencyProperty SourceProperty =
        DependencyProperty.Register(nameof(Source), typeof(ImageSource), typeof(SwapImage), new PropertyMetadata(new BitmapImage()));

    public static readonly DependencyProperty StretchProperty =
        DependencyProperty.Register(nameof(Stretch), typeof(Stretch), typeof(SwapImage), new PropertyMetadata(Stretch.Uniform));

    public static readonly DependencyProperty NinegridProperty =
        DependencyProperty.Register(nameof(Ninegrid), typeof(Thickness), typeof(SwapImage), new PropertyMetadata(new Thickness(0)));

    public static readonly DependencyProperty IsOpenedProperty =
        DependencyProperty.Register(nameof(IsOpened), typeof(bool), typeof(SwapImage), new PropertyMetadata(false));

    protected override void OnApplyTemplate()
    {
        if (_hiddenImage != null)
        {
            _hiddenImage.ImageOpened -= HiddenImage_ImageOpened;
            _hiddenImage.ImageFailed -= HiddenImage_ImageFailed;
        }
        
        _hiddenImage = GetTemplateChild(SwapImageHiddenImagePart) as Image;
        _displayedImage = GetTemplateChild(SwapImageDisplayedImagePart) as Image;

        if (_hiddenImage != null)
        {
            _hiddenImage.ImageOpened += HiddenImage_ImageOpened;
            _hiddenImage.ImageFailed += HiddenImage_ImageFailed;
        }
    }

    private void HiddenImage_ImageOpened(object sender, RoutedEventArgs e)
    {
        ImageOpened?.Invoke(sender, e);

        if (_displayedImage != null && _hiddenImage != null)
        {
            _displayedImage.Source = _hiddenImage.Source;
            IsOpened = true;
        }
    }

    private void HiddenImage_ImageFailed(object sender, ExceptionRoutedEventArgs e)
    {
        ImageFailed?.Invoke(sender, e);

        if (_displayedImage != null)
        {
            _displayedImage.Source = null;
            IsOpened = false;
        }
    }
}
