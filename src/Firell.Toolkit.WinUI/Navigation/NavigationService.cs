using System;
using System.Collections.Generic;
using System.Linq;

using Firell.Toolkit.WinUI.Extensions;

using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Navigation;

namespace Firell.Toolkit.WinUI.Navigation;

public class NavigationService : IDisposable
{
    private static readonly List<NavigationStackEntry> _navigationBackStackEntries = new List<NavigationStackEntry>();
    private static bool _isBackRequested;

    private static NavigationView? _navigationView;
    public static NavigationView NavigationView
    {
        get => _navigationView ?? throw new InvalidOperationException("The navigation view has not been set.");
        set
        {
            UnregisterNavigationViewEvents();
            _navigationView = value;
            RegisterNavigationViewEvents();
        }
    }

    private static Frame? _frame;
    public static Frame Frame
    {
        get => _frame ?? throw new InvalidOperationException("The frame has not been set.");
        set
        {
            UnregisterFrameEvents();
            _frame = value;
            RegisterFrameEvents();
        }
    }

    public static bool CanGoBack
    {
        get => _navigationBackStackEntries.Any();
    }

    public static bool AttemptToGoBack()
    {
        NavigationStackEntry? navigationStack = _navigationBackStackEntries.LastOrDefault();
        if (navigationStack == null)
        {
            return false;
        }

        _isBackRequested = true;
        _navigationBackStackEntries.Remove(navigationStack);
        return navigationStack.Frame.NavigateToPage(navigationStack.PageType, navigationStack.Parameter, navigationStack.TransitionInfo);
    }

    private static void NavigationView_BackRequested(NavigationView sender, NavigationViewBackRequestedEventArgs args)
    {
        AttemptToGoBack();
    }

    private static void NavigationHelper_Navigated(object? sender, NavigationEventArgs e)
    {
        if (sender is not Frame frame)
        {
            return;
        }

        if (!_isBackRequested)
        {
            PageStackEntry? pageBackStack = frame.BackStack.LastOrDefault();
            if (pageBackStack != null)
            {
                _navigationBackStackEntries.Add(new NavigationStackEntry(frame, pageBackStack.SourcePageType, pageBackStack.Parameter, pageBackStack.NavigationTransitionInfo));
            }
        }

        if (frame.Name.Equals(Frame.Name))
        {
            SetSelectedItemToFrameContent();
        }

        _isBackRequested = false;
        NavigationView.IsBackEnabled = CanGoBack;
    }

    private static void SetSelectedItemToFrameContent()
    {
        List<NavigationViewItem> navigationViewItems = GetAllNavigationViewItems();
        NavigationView.SelectedItem = navigationViewItems.FirstOrDefault(x => x.Tag?.Equals(Frame.Content.GetType().Name) ?? false);
    }

    private static List<NavigationViewItem> GetAllNavigationViewItems()
    {
        List<NavigationViewItem> navigationViewItems = new List<NavigationViewItem>();

        TraverseNavigationViewItems(navigationViewItems, NavigationView.MenuItems);
        TraverseNavigationViewItems(navigationViewItems, NavigationView.FooterMenuItems);

        return navigationViewItems;
    }

    private static void TraverseNavigationViewItems(List<NavigationViewItem> navigationViewItems, IList<object> items)
    {
        foreach (object item in items)
        {
            if (item is NavigationViewItem navigationViewItem)
            {
                navigationViewItems.Add(navigationViewItem);

                if (navigationViewItem.MenuItems.Any())
                {
                    TraverseNavigationViewItems(navigationViewItems, navigationViewItem.MenuItems);
                }
            }
        }
    }

    private static void RegisterNavigationViewEvents()
    {
        if (_navigationView != null)
        {
            NavigationView.BackRequested += NavigationView_BackRequested;
        }
    }

    private static void UnregisterNavigationViewEvents()
    {
        if (_navigationView != null)
        {
            NavigationView.BackRequested -= NavigationView_BackRequested;
        }
    }

    private static void RegisterFrameEvents()
    {
        if (_frame != null)
        {
            FrameExtensions.Navigated += NavigationHelper_Navigated;
        }
    }

    private static void UnregisterFrameEvents()
    {
        if (_frame != null)
        {
            FrameExtensions.Navigated -= NavigationHelper_Navigated;
        }
    }

    public void Dispose()
    {
        FrameExtensions.Navigated -= NavigationHelper_Navigated;
        GC.SuppressFinalize(this);
    }
}
