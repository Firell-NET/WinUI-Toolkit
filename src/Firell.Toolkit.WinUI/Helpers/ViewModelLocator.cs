using System;

using Firell.Toolkit.Common.Helpers;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.UI.Xaml;

namespace Firell.Toolkit.WinUI.Helpers;

public static class ViewModelLocator
{
    private static IServiceProvider _serviceProvider = new ServiceCollection().BuildServiceProvider();
    public static IServiceProvider ServiceProvider
    {
        get => _serviceProvider;
        set => _serviceProvider = value;
    }

    public static readonly DependencyProperty InitializeViewModelProperty = DependencyProperty.Register("InitializeViewModel", typeof(bool), typeof(ViewModelLocator), new PropertyMetadata(false, InitializeViewModelChanged));

    public static bool GetInitializeViewModel(UIElement element)
    {
        return (bool)element.GetValue(InitializeViewModelProperty);
    }

    public static void SetInitializeViewModel(UIElement element, bool value)
    {
        element.SetValue(InitializeViewModelProperty, value);
    }

    private static void InitializeViewModelChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if ((bool)e.NewValue)
        {
            InitializeViewModel(d);
        }
    }

    private static void InitializeViewModel(DependencyObject d)
    {
        if (d is FrameworkElement element)
        {
            element.DataContext = FindViewModel(element.GetType());
        }
    }

    public static TViewModel? FindViewModel<TViewModel>(Type viewType)
    {
        return  (TViewModel?)FindViewModel(viewType);
    }

    public static object? FindViewModel(Type viewType)
    {
        Type? viewModelType = AssemblyHelper.GetType($"{viewType.Name}ViewModel");
        if (viewModelType == null)
        {
            return default;
        }

        return ActivatorUtilities.GetServiceOrCreateInstance(ServiceProvider, viewModelType);
    }
}
