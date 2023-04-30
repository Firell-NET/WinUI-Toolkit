using Microsoft.UI.Xaml;

namespace Firell.Toolkit.WinUI.Dialogs;

public sealed partial class MessageDialog : BaseContentDialog
{
    // TODO: Add different message dialog variants.
    public MessageDialog(XamlRoot xamlRoot) : base(xamlRoot)
    {
        InitializeComponent();
        CloseButtonText = "Close";
    }
}
