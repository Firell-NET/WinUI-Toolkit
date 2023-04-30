using System;

using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media.Animation;

namespace Firell.Toolkit.WinUI.Navigation;

public class NavigationStackEntry
{
    public NavigationStackEntry(Frame frame, Type pageType, object? parameter, NavigationTransitionInfo? transitionInfo)
    {
        Frame = frame;
        PageType = pageType;
        Parameter = parameter;
        TransitionInfo = transitionInfo;
    }

    public Frame Frame { get; set; }

    public Type PageType { get; set; }

    public object? Parameter { get; set; }

    public NavigationTransitionInfo? TransitionInfo { get; set; }
}
