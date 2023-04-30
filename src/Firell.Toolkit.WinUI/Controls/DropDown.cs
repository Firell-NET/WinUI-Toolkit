using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;

namespace Firell.Toolkit.WinUI.Controls;

[TemplatePart(Name = DropDownFlyoutPart)]
public class DropDown : ListViewBase
{
    protected const string DropDownFlyoutPart = "PART_DropDownFlyout";

    public DropDown()
    {
        DefaultStyleKey = typeof(DropDown);
        SelectionChanged += DropDown_SelectionChanged;
    }

    public object Content
    {
        get => (object)GetValue(ContentProperty);
        set => SetValue(ContentProperty, value);
    }

    public string SelectedText
    {
        get => (string)GetValue(SelectedTextProperty);
        private set => SetValue(SelectedTextProperty, value);
    }

    public static readonly DependencyProperty ContentProperty =
        DependencyProperty.Register(nameof(Content), typeof(object), typeof(DropDown), new PropertyMetadata(null));

    public static readonly DependencyProperty SelectedTextProperty =
        DependencyProperty.Register(nameof(SelectedText), typeof(string), typeof(DropDown), new PropertyMetadata("None"));

    private void DropDown_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if (GetTemplateChild(DropDownFlyoutPart) is Flyout flyout)
        {
            SelectedText = SelectedItem?.GetType()?.GetProperty(DisplayMemberPath)?.GetValue(SelectedItem, null)?.ToString() ?? "None";
            flyout.Hide();
        }
    }
}
