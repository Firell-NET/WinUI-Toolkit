using System.Linq;

using CommunityToolkit.WinUI.UI;
using CommunityToolkit.WinUI.UI.Converters;

using Firell.Toolkit.Common;

using Microsoft.UI;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;

namespace Firell.Toolkit.WinUI.Controls;

public class ScrollablePage : Page
{
    public ScrollablePage()
    {
        // It's necessary to set the background, otherwise scrolling doesn't work outside content.
        Background = new SolidColorBrush(Colors.Transparent);

        // Templating a page doesn't work as expected so, it's necessary to manually style it in the loaded event.
        Loaded += BasePage_Loaded;
    }

    public virtual BaseViewModel? ViewModel { get; set; }

    public ScrollViewer? ScrollViewer { get; private set; }

    public bool IsScrollable
    {
        get => (bool)GetValue(IsScrollableProperty);
        set => SetValue(IsScrollableProperty, value);
    }

    public new Thickness Padding
    {
        get => (Thickness)GetValue(PaddingProperty);
        set => SetValue(PaddingProperty, value);
    }

    public static readonly DependencyProperty IsScrollableProperty =
        DependencyProperty.Register(nameof(IsScrollable), typeof(bool), typeof(ScrollablePage), new PropertyMetadata(true));

    public new static readonly DependencyProperty PaddingProperty =
        DependencyProperty.Register(nameof(Padding), typeof(Thickness), typeof(ScrollablePage), new PropertyMetadata(new Thickness(35, 20, 35, 20)));

    private void BasePage_Loaded(object sender, RoutedEventArgs e)
    {
        object pageContent = Content;
        Content = null;

        ContentPresenter pageContentPresenter = new ContentPresenter() {
            Content = pageContent,
        };

        ScrollViewer pageScrollViewer = new ScrollViewer() {
            HorizontalScrollMode = ScrollMode.Disabled,
            HorizontalScrollBarVisibility = ScrollBarVisibility.Disabled,
            Content = pageContentPresenter,
        };

        pageScrollViewer.SetBinding(ScrollViewer.BackgroundProperty, new Binding() {
            Source = this,
            Path = new PropertyPath(nameof(Background)),
            Mode = BindingMode.OneWay,
        });

        pageScrollViewer.SetBinding(ScrollViewer.VerticalScrollModeProperty, new Binding() {
            Source = this,
            Path = new PropertyPath(nameof(IsScrollable)),
            Converter = new BoolToObjectConverter() {
                TrueValue = ScrollMode.Auto,
                FalseValue = ScrollMode.Disabled
            },
            Mode = BindingMode.OneWay,
        });

        pageScrollViewer.SetBinding(ScrollViewer.VerticalScrollBarVisibilityProperty, new Binding() {
            Source = this,
            Path = new PropertyPath(nameof(IsScrollable)),
            Converter = new BoolToObjectConverter() {
                TrueValue = ScrollBarVisibility.Auto,
                FalseValue = ScrollBarVisibility.Disabled
            },
            Mode = BindingMode.OneWay,
        });

        pageScrollViewer.SetBinding(ScrollViewer.PaddingProperty, new Binding() {
            Source = this,
            Path = new PropertyPath(nameof(Padding)),
            Mode = BindingMode.OneWay,
        });

        ScrollViewer = pageScrollViewer;
        Content = pageScrollViewer;
        Loaded -= BasePage_Loaded;
    }

    protected override void OnNavigatedFrom(NavigationEventArgs e)
    {
        base.OnNavigatedFrom(e);
        ViewModel?.Dispose();

        // Dispose any descendant frames, by navigating to an empty page to force OnNavigatedFrom to be called.
        foreach (Frame frame in this.FindDescendants().OfType<Frame>())
        {
            frame.Navigate(typeof(Page));
        }
    }
}
