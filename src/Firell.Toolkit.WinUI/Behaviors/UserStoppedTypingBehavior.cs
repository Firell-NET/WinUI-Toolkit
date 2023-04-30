using System;
using System.Windows.Input;

using CommunityToolkit.WinUI.UI.Behaviors;

using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;

namespace Firell.Toolkit.WinUI.Behaviors;

public class UserStoppedTypingBehavior : BehaviorBase<UIElement>
{
    public event EventHandler<object?>? UserStoppedTyping;

    protected DispatcherTimer DispatcherTimer { get; private set; } = new DispatcherTimer();

    public int StoppedTypingTimeThreshold
    {
        get => (int)GetValue(StoppedTypingTimeThresholdProperty);
        set => SetValue(StoppedTypingTimeThresholdProperty, value);
    }

    public int MinimumLengthThreshold
    {
        get => (int)GetValue(MinimumLengthThresholdProperty);
        set => SetValue(MinimumLengthThresholdProperty, value);
    }

    public ICommand Command
    {
        get => (ICommand)GetValue(CommandProperty);
        set => SetValue(CommandProperty, value);
    }

    public object CommandParameter
    {
        get => (object)GetValue(CommandParameterProperty);
        set => SetValue(CommandParameterProperty, value);
    }

    public static readonly DependencyProperty StoppedTypingTimeThresholdProperty =
        DependencyProperty.Register(nameof(StoppedTypingTimeThreshold), typeof(int), typeof(UserStoppedTypingBehavior), new PropertyMetadata(1000, StoppedDraggingTimeThreshold_PropertyChanged));

    public static readonly DependencyProperty MinimumLengthThresholdProperty =
        DependencyProperty.Register(nameof(MinimumLengthThreshold), typeof(int), typeof(UserStoppedTypingBehavior), new PropertyMetadata(0));

    public static readonly DependencyProperty CommandProperty =
        DependencyProperty.Register(nameof(Command), typeof(ICommand), typeof(UserStoppedTypingBehavior), new PropertyMetadata(null));

    public static readonly DependencyProperty CommandParameterProperty =
        DependencyProperty.Register(nameof(CommandParameter), typeof(object), typeof(UserStoppedTypingBehavior), new PropertyMetadata(null));

    private static void StoppedDraggingTimeThreshold_PropertyChanged(DependencyObject obj, DependencyPropertyChangedEventArgs e)
    {
        UserStoppedTypingBehavior behavior = (UserStoppedTypingBehavior)obj;

        behavior.DispatcherTimer.Interval = TimeSpan.FromMilliseconds(behavior.StoppedTypingTimeThreshold);
    }

    protected override void OnAttached()
    {
        base.OnAttached();

        if (AssociatedObject is AutoSuggestBox autoSuggestBox)
        {
            autoSuggestBox.TextChanged += AutoSuggestBox_TextChanged;
        }
        else if (AssociatedObject is PasswordBox passwordBox)
        {
            passwordBox.PasswordChanged += PasswordBox_PasswordChanged;
        }
        else if (AssociatedObject is TextBox textBox)
        {
            textBox.TextChanged += TextBox_TextChanged;
        }

        DispatcherTimer.Interval = TimeSpan.FromMilliseconds(StoppedTypingTimeThreshold);
        DispatcherTimer.Tick += DispatcherTimer_Tick;
    }

    protected override void OnDetaching()
    {
        base.OnDetaching();

        if (AssociatedObject is AutoSuggestBox autoSuggestBox)
        {
            autoSuggestBox.TextChanged -= AutoSuggestBox_TextChanged;
        }
        else if (AssociatedObject is PasswordBox passwordBox)
        {
            passwordBox.PasswordChanged -= PasswordBox_PasswordChanged;
        }
        else if (AssociatedObject is TextBox textBox)
        {
            textBox.TextChanged -= TextBox_TextChanged;
        }

        DispatcherTimer.Tick -= DispatcherTimer_Tick;
    }

    private void AutoSuggestBox_TextChanged(AutoSuggestBox sender, AutoSuggestBoxTextChangedEventArgs args)
    {
        if (sender.Text.Length >= MinimumLengthThreshold && args.Reason == AutoSuggestionBoxTextChangeReason.UserInput)
        {
            DispatcherTimer.Start();
        }
        else
        {
            DispatcherTimer.Stop();
        }
    }

    private void PasswordBox_PasswordChanged(object sender, RoutedEventArgs e)
    {
        if (sender is PasswordBox passwordBox && passwordBox.Password.Length >= MinimumLengthThreshold)
        {
            DispatcherTimer.Start();
        }
        else
        {
            DispatcherTimer.Stop();
        }
    }

    private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
    {
        if (sender is TextBox textBox && textBox.Text.Length >= MinimumLengthThreshold)
        {
            DispatcherTimer.Start();
        }
        else
        {
            DispatcherTimer.Stop();
        }
    }

    private void DispatcherTimer_Tick(object? sender, object e)
    {
        object commandParameter = CommandParameter;
        if (AssociatedObject is AutoSuggestBox autoSuggestBox)
        {
            commandParameter ??= autoSuggestBox.Text;
        }
        else if (AssociatedObject is PasswordBox passwordBox)
        {
            commandParameter ??= passwordBox.Password;
        }
        else if (AssociatedObject is TextBox textBox)
        {
            commandParameter ??= textBox.Text;
        }

        UserStoppedTyping?.Invoke(AssociatedObject, commandParameter);
        if (Command.CanExecute(commandParameter))
        {
            Command.Execute(commandParameter);
        }

        DispatcherTimer.Stop();
    }
}
