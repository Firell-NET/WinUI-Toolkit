using System;
using System.Collections.Generic;

using Microsoft.UI.Xaml;

namespace Firell.Toolkit.WinUI.Helpers;

public static class WindowHelper
{
    private static readonly List<Window> _activeWindows = new List<Window>();
    public static List<Window> ActiveWindows { get => _activeWindows; }

    public static TWindow CreateWindow<TWindow>() where TWindow : Window
    {
        TWindow window = Activator.CreateInstance<TWindow>();
        TrackWindow(window);

        return window;
    }

    public static Window CreateWindow()
    {
        return CreateWindow<Window>();
    }

    public static void TrackWindow(Window window)
    {
        window.Closed += (sender, args) => {
            _activeWindows.Remove(window);
        };

        _activeWindows.Add(window);
    }

    public static Window? GetWindowForElement(UIElement element)
    {
        if (element.XamlRoot == null)
        {
            return null;
        }

        foreach (Window window in ActiveWindows)
        {
            if (element.XamlRoot == window.Content.XamlRoot)
            {
                return window;
            }
        }

        return null;
    }
}
