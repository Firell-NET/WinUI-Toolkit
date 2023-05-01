using System;
using System.Threading.Tasks;

using Firell.Toolkit.WinUI.Controls;
using Firell.Toolkit.WinUI.Helpers;

using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;

namespace Firell.Toolkit.WinUI.Dialogs;

public abstract class BaseContentDialog : ContentDialog
{
    public BaseContentDialog(XamlRoot xamlRoot)
    {
        XamlRoot = xamlRoot;
        Style = (Style)Application.Current.Resources["DefaultContentDialogStyle"];
        RequestedTheme = ThemeHelper.CurrentTheme;
        ManipulationMode = ManipulationModes.All;

        HorizontalContentAlignment = HorizontalAlignment.Stretch;
        VerticalContentAlignment = VerticalAlignment.Stretch;

        Loaded += BaseContentDialog_Loaded;
    }

    public BaseContentDialog(XamlRoot xamlRoot, bool isMovable) : this(xamlRoot)
    {
        IsMoveable = isMovable;
    }

    public string Glyph
    {
        get => (string)GetValue(GlyphProperty);
        set => SetValue(GlyphProperty, value);
    }

    public Brush GlyphForeground
    {
        get => (Brush)GetValue(GlyphForegroundProperty);
        set => SetValue(GlyphForegroundProperty, value);
    }

    public string BackgroundGlyph
    {
        get => (string)GetValue(BackgroundGlyphProperty);
        set => SetValue(BackgroundGlyphProperty, value);
    }

    public Brush GlyphBackground
    {
        get => (Brush)GetValue(GlyphBackgroundProperty);
        set => SetValue(GlyphBackgroundProperty, value);
    }

    public new string Title
    {
        get => (string)GetValue(TitleProperty);
        set => SetValue(TitleProperty, value);
    }

    public string? Subtitle
    {
        get => (string?)GetValue(SubtitleProperty);
        set => SetValue(SubtitleProperty, value);
    }

    public Brush ContentBackground
    {
        get => (Brush)GetValue(ContentBackgroundProperty);
        set => SetValue(ContentBackgroundProperty, value);
    }

    public new double MinWidth
    {
        get => (double)GetValue(MinWidthProperty);
        set => SetValue(MinWidthProperty, value);
    }

    public new double MaxWidth
    {
        get => (double)GetValue(MaxWidthProperty);
        set => SetValue(MaxWidthProperty, value);
    }

    public new double MinHeight
    {
        get => (double)GetValue(MinHeightProperty);
        set => SetValue(MinHeightProperty, value);
    }

    public new double MaxHeight
    {
        get => (double)GetValue(MaxHeightProperty);
        set => SetValue(MaxHeightProperty, value);
    }

    private bool _isMoveable;
    public bool IsMoveable
    {
        get => _isMoveable;
        set
        {
            if (_isMoveable = value)
            {
                ManipulationDelta += BaseContentDialog_ManipulationDelta;
            }
            else
            {
                ManipulationDelta -= BaseContentDialog_ManipulationDelta;
            }
        }
    }

    protected ContentDialogResult Result { get; private set; }

    public static readonly DependencyProperty GlyphProperty =
        DependencyProperty.Register(nameof(Glyph), typeof(string), typeof(BaseContentDialog), new PropertyMetadata(string.Empty));

    public static readonly DependencyProperty GlyphForegroundProperty =
        DependencyProperty.Register(nameof(GlyphForeground), typeof(Brush), typeof(BaseContentDialog), new PropertyMetadata((Brush)Application.Current.Resources["TextFillColorPrimaryBrush"]));

    public static readonly DependencyProperty BackgroundGlyphProperty =
        DependencyProperty.Register(nameof(BackgroundGlyph), typeof(string), typeof(BaseContentDialog), new PropertyMetadata(string.Empty));

    public static readonly DependencyProperty GlyphBackgroundProperty =
        DependencyProperty.Register(nameof(GlyphBackground), typeof(Brush), typeof(BaseContentDialog), new PropertyMetadata((Brush)Application.Current.Resources["TextFillColorSecondaryBrush"]));

    public new static readonly DependencyProperty TitleProperty =
        DependencyProperty.Register(nameof(Title), typeof(string), typeof(BaseContentDialog), new PropertyMetadata(string.Empty));

    public static readonly DependencyProperty SubtitleProperty =
        DependencyProperty.Register(nameof(Subtitle), typeof(string), typeof(BaseContentDialog), new PropertyMetadata(string.Empty));

    public static readonly DependencyProperty ContentBackgroundProperty =
        DependencyProperty.Register(nameof(ContentBackground), typeof(Brush), typeof(BaseContentDialog), new PropertyMetadata(null));

    public new static readonly DependencyProperty MinWidthProperty =
        DependencyProperty.Register(nameof(MinWidth), typeof(double), typeof(BaseContentDialog), new PropertyMetadata(0d));

    public new static readonly DependencyProperty MaxWidthProperty =
        DependencyProperty.Register(nameof(MaxWidth), typeof(double), typeof(BaseContentDialog), new PropertyMetadata(double.PositiveInfinity));

    public new static readonly DependencyProperty MinHeightProperty =
        DependencyProperty.Register(nameof(MinHeight), typeof(double), typeof(BaseContentDialog), new PropertyMetadata(0d));

    public new static readonly DependencyProperty MaxHeightProperty =
        DependencyProperty.Register(nameof(MaxHeight), typeof(double), typeof(BaseContentDialog), new PropertyMetadata(double.PositiveInfinity));

    protected override void OnApplyTemplate()
    {
        base.OnApplyTemplate();

        // CommandSpace should not be draggable, as it could interfere with the buttons.
        if (GetTemplateChild("CommandSpace") is Grid grid)
        {
            grid.ManipulationMode = ManipulationModes.None;
        }
    }

    private void BaseContentDialog_Loaded(object sender, RoutedEventArgs e)
    {
        object? dialogContent = Content;
        Content = null;

        Grid dialogGrid = new Grid() {
            ManipulationMode = ManipulationModes.None,
            ColumnSpacing = string.IsNullOrWhiteSpace(Glyph) ? 0 : 15,
            RowSpacing = string.IsNullOrWhiteSpace(Title) ? 0 : 15,
            Background = ContentBackground,
            Padding = new Thickness(24),
            Margin = new Thickness(-24),
            MinWidth = MinWidth,
            MaxWidth = MaxWidth,
            MinHeight = MinHeight,
            MaxHeight = MaxHeight,
        };

        dialogGrid.ColumnDefinitions.Add(new ColumnDefinition() {
            Width = GridLength.Auto
        });

        dialogGrid.ColumnDefinitions.Add(new ColumnDefinition() {
            Width = new GridLength(1, GridUnitType.Star)
        });

        dialogGrid.RowDefinitions.Add(new RowDefinition() {
            Height = GridLength.Auto
        });

        dialogGrid.RowDefinitions.Add(new RowDefinition() {
            Height = new GridLength(1, GridUnitType.Star)
        });

        if (!string.IsNullOrWhiteSpace(Title))
        {
            LayeredFontIcon dialogIcon = new LayeredFontIcon() {
                Glyph = Glyph,
                BackgroundGlyph = BackgroundGlyph,
                Foreground = GlyphForeground,
                Background = GlyphBackground,
                FontSize = 32,
            };

            dialogGrid.Children.Add(dialogIcon);
            dialogIcon.SetValue(Grid.ColumnProperty, 0);
            dialogIcon.SetValue(Grid.RowProperty, 0);

            StackPanel dialogStackPanel = new StackPanel() {
                VerticalAlignment = VerticalAlignment.Center,
            };

            TextBlock dialogTitle = new TextBlock() {
                Text = Title,
                Style = (Style)Application.Current.Resources["SubtitleTextBlockStyle"],
            };

            TextBlock dialogSubTitle = new TextBlock() {
                Text = Subtitle,
                Style = (Style)Application.Current.Resources["CaptionTextBlockStyle"],
                Foreground = (Brush)Application.Current.Resources["TextFillColorSecondaryBrush"],
                Visibility = string.IsNullOrWhiteSpace(Subtitle) ? Visibility.Collapsed : Visibility.Visible
            };

            dialogStackPanel.Children.Add(dialogTitle);
            dialogStackPanel.Children.Add(dialogSubTitle);

            dialogGrid.Children.Add(dialogStackPanel);
            dialogStackPanel.SetValue(Grid.ColumnProperty, 1);
            dialogStackPanel.SetValue(Grid.RowProperty, 0);
        }

        ContentPresenter dialogContentPresenter = new ContentPresenter() {
            Content = dialogContent,
            TextWrapping = TextWrapping.Wrap,
            HorizontalContentAlignment = HorizontalContentAlignment,
            VerticalContentAlignment = VerticalContentAlignment,
            ManipulationMode = ManipulationModes.None
        };

        dialogGrid.Children.Add(dialogContentPresenter);
        dialogContentPresenter.SetValue(Grid.ColumnSpanProperty, 2);
        dialogContentPresenter.SetValue(Grid.RowProperty, 1);

        Content = dialogGrid;
        Loaded -= BaseContentDialog_Loaded;
    }

    private void BaseContentDialog_ManipulationDelta(object sender, ManipulationDeltaRoutedEventArgs e)
    {
        if (!e.IsInertial)
        {
            Margin = new Thickness(Margin.Left + e.Delta.Translation.X, Margin.Top + e.Delta.Translation.Y, Margin.Left - e.Delta.Translation.X, Margin.Top - e.Delta.Translation.Y);
        }
    }

    public new async Task<ContentDialogResult> ShowAsync()
    {
        try
        {
            ContentDialogResult result = await base.ShowAsync();
            if (result != ContentDialogResult.None)
            {
                Result = result;
            }

            return Result;
        }
        catch
        {
            return ContentDialogResult.None;
        }
    }

    protected virtual void Hide(ContentDialogResult result)
    {
        Result = result;
        base.Hide();
    }

    public new void Hide()
    {
        Hide(ContentDialogResult.None);
    }
}
