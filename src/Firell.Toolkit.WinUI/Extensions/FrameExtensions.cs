using System;

using Firell.Toolkit.Common.Helpers;

using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media.Animation;
using Microsoft.UI.Xaml.Navigation;

namespace Firell.Toolkit.WinUI.Extensions;

public static class FrameExtensions
{
    private static object? _lastNavigationParameter;

    public static event EventHandler<NavigationEventArgs>? Navigated;

    public static bool NavigateToPage(this Frame frame, NavigationViewItem navigationItem)
    {
        return NavigateToPage(frame, navigationItem, null, null);
    }

    public static bool NavigateToPage(this Frame frame, NavigationViewItem navigationItem, object? parameter)
    {
        return NavigateToPage(frame, navigationItem, parameter, null);
    }

    public static bool NavigateToPage(this Frame frame, NavigationViewItem navigationItem, object? parameter, NavigationTransitionInfo? transitionInfo)
    {
        if (navigationItem.Tag is not string tag)
        {
            return false;
        }

        return NavigateToPage(frame, tag, parameter, transitionInfo);
    }

    public static bool NavigateToPage(this Frame frame, string pageType)
    {
        return NavigateToPage(frame, pageType, null, null);
    }

    public static bool NavigateToPage(this Frame frame, string pageType, object? parameter)
    {
        return NavigateToPage(frame, pageType, parameter, null);
    }

    public static bool NavigateToPage(this Frame frame, string pageType, object? parameter, NavigationTransitionInfo? transitionInfo)
    {
        if (string.IsNullOrWhiteSpace(pageType))
        {
            return false;
        }

        Type? type = AssemblyHelper.GetType(pageType);
        if (type == null)
        {
            return false;
        }

        return NavigateToPage(frame, type, parameter, transitionInfo);
    }

    public static bool NavigateToPage<TPage>(this Frame frame) where TPage : Page
    {
        return NavigateToPage(frame, typeof(TPage), null, null);
    }

    public static bool NavigateToPage<TPage>(this Frame frame, object? parameter) where TPage : Page
    {
        return NavigateToPage(frame, typeof(TPage), parameter, null);
    }

    public static bool NavigateToPage<TPage>(this Frame frame, object? parameter, NavigationTransitionInfo? transitionInfo) where TPage : Page
    {
        return NavigateToPage(frame, typeof(TPage), parameter, transitionInfo);
    }

    public static bool NavigateToPage(this Frame frame, Type pageType)
    {
        return NavigateToPage(frame, pageType, null, null);
    }

    public static bool NavigateToPage(this Frame frame, Type pageType, object? parameter)
    {
        return NavigateToPage(frame, pageType, parameter, null);
    }

    public static bool NavigateToPage(this Frame frame, Type pageType, object? parameter, NavigationTransitionInfo? transitionInfo)
    {
        if (frame.Content?.GetType() == pageType && (parameter?.Equals(_lastNavigationParameter) ?? true))
        {
            return false;
        }

        frame.Navigated += Frame_Navigated;
        bool result = frame.Navigate(pageType, parameter, transitionInfo);
        if (result)
        {
            _lastNavigationParameter = parameter;
        }

        return result;
    }

    private static void Frame_Navigated(object sender, NavigationEventArgs e)
    {
        if (sender is Frame frame)
        {
            Navigated?.Invoke(frame, e);
            frame.Navigated -= Frame_Navigated;
        }
    }
}
