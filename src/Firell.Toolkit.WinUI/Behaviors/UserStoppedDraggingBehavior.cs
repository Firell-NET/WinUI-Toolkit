using System;
using System.Windows.Input;

using CommunityToolkit.WinUI.UI.Behaviors;

using Firell.Toolkit.Common.Extensions;

using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;

namespace Firell.Toolkit.WinUI.Behaviors;

public class UserStoppedDraggingBehavior : BehaviorBase<Slider>
{
    public event EventHandler<object?>? UserStoppedDragging;

    protected DispatcherTimer DispatcherTimer { get; private set; } = new DispatcherTimer();

    public int StoppedDraggingTimeThreshold
    {
        get => (int)GetValue(StoppedDraggingTimeThresholdProperty);
        set => SetValue(StoppedDraggingTimeThresholdProperty, value);
    }

    public double MinimumValueThreshold
    {
        get => (double)GetValue(MinimumValueThresholdProperty);
        set => SetValue(MinimumValueThresholdProperty, value);
    }

    public double MaximumValueThreshold
    {
        get => (double)GetValue(MaximumValueThresholdProperty);
        set => SetValue(MaximumValueThresholdProperty, value);
    }

    public ICommand Command
    {
        get => (ICommand)GetValue(CommandProperty);
        set => SetValue(CommandProperty, value);
    }

    public object CommandParameter
    {
        get => GetValue(CommandParameterProperty);
        set => SetValue(CommandParameterProperty, value);
    }

    public static readonly DependencyProperty StoppedDraggingTimeThresholdProperty =
        DependencyProperty.Register(nameof(StoppedDraggingTimeThresholdProperty), typeof(int), typeof(UserStoppedDraggingBehavior), new PropertyMetadata(250, StoppedDraggingTimeThreshold_PropertyChanged));

    public static readonly DependencyProperty MinimumValueThresholdProperty =
        DependencyProperty.Register(nameof(MinimumValueThreshold), typeof(double), typeof(UserStoppedTypingBehavior), new PropertyMetadata(double.MinValue));

    public static readonly DependencyProperty MaximumValueThresholdProperty =
        DependencyProperty.Register(nameof(MaximumValueThreshold), typeof(double), typeof(UserStoppedTypingBehavior), new PropertyMetadata(double.MaxValue));

    public static readonly DependencyProperty CommandProperty =
        DependencyProperty.Register(nameof(Command), typeof(ICommand), typeof(UserStoppedDraggingBehavior), new PropertyMetadata(null));

    public static readonly DependencyProperty CommandParameterProperty =
        DependencyProperty.Register(nameof(CommandParameter), typeof(object), typeof(UserStoppedDraggingBehavior), new PropertyMetadata(null));

    private static void StoppedDraggingTimeThreshold_PropertyChanged(DependencyObject obj, DependencyPropertyChangedEventArgs e)
    {
        UserStoppedDraggingBehavior behavior = (UserStoppedDraggingBehavior)obj;

        behavior.DispatcherTimer.Interval = TimeSpan.FromMilliseconds(behavior.StoppedDraggingTimeThreshold);
    }

    protected override void OnAttached()
    {
        base.OnAttached();

        AssociatedObject.ValueChanged += AssociatedObject_ValueChanged;
        DispatcherTimer.Interval = TimeSpan.FromMilliseconds(StoppedDraggingTimeThreshold);
        DispatcherTimer.Tick += DispatcherTimer_Tick;
    }

    protected override void OnDetaching()
    {
        base.OnDetaching();

        AssociatedObject.ValueChanged -= AssociatedObject_ValueChanged;
        DispatcherTimer.Tick -= DispatcherTimer_Tick;
    }

    private void AssociatedObject_ValueChanged(object sender, RangeBaseValueChangedEventArgs e)
    {
        if (AssociatedObject.Value.InRange(MinimumValueThreshold, MaximumValueThreshold))
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
        commandParameter ??= AssociatedObject.Value;

        UserStoppedDragging?.Invoke(AssociatedObject, commandParameter);
        if (Command.CanExecute(commandParameter))
        {
            Command.Execute(commandParameter);
        }

        DispatcherTimer.Stop();
    }
}
