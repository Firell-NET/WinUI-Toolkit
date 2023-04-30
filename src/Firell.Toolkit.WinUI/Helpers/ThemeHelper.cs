using System;

using Firell.Toolkit.Common.Extensions;

using Microsoft.UI.Xaml;

using Windows.Storage;

using WinUIEx;

namespace Firell.Toolkit.WinUI.Helpers;

public static class ThemeHelper
{
    private const string SelectedApplicationThemeKey = "SelectedApplicationTheme";

    public static event EventHandler<ElementTheme>? ThemeChanged;

    public static ElementTheme CurrentTheme
    {
        get
        {
            foreach (Window window in WindowHelper.ActiveWindows)
            {
                // Verify whether the Window is of type WindowEx since it uses WindowContent instead.
                if (window is WindowEx windowEx)
                {
                    if (windowEx.WindowContent is FrameworkElement element)
                    {
                        return element.RequestedTheme;
                    }
                }
                else
                {
                    if (window.Content is FrameworkElement element)
                    {
                        return element.RequestedTheme;
                    }
                }
            }

            return ElementTheme.Default;
        }
        set
        {
            foreach (Window window in WindowHelper.ActiveWindows)
            {
                // Verify whether the Window is of type WindowEx since it uses WindowContent instead.
                if (window is WindowEx windowEx)
                {
                    if (windowEx.WindowContent is FrameworkElement element)
                    {
                        element.RequestedTheme = value;
                    }
                }
                else
                {
                    if (window.Content is FrameworkElement element)
                    {
                        element.RequestedTheme = value;
                    }
                }
            }

#if !UNPACKAGED
            ApplicationData.Current.LocalSettings.Values[SelectedApplicationThemeKey] = value.ToString();
#endif
            ThemeChanged?.Invoke(null, value);
        }
    }

    public static bool IsDarkTheme
    {
        get
        {
            if (CurrentTheme == ElementTheme.Default)
            {
                return Application.Current.RequestedTheme == ApplicationTheme.Dark;
            }

            return CurrentTheme == ElementTheme.Dark;
        }
    }

    public static void Initialize()
    {
#if !UNPACKAGED
        string? selectedTheme = ApplicationData.Current.LocalSettings.Values[SelectedApplicationThemeKey]?.ToString();
        if (selectedTheme != null)
        {
            CurrentTheme = selectedTheme.ToEnum<ElementTheme>();
        }
#endif
    }

    public static void ToggleTheme()
    {
        switch (CurrentTheme)
        {
            case ElementTheme.Default:
            {
                switch (Application.Current.RequestedTheme)
                {
                    case ApplicationTheme.Light:
                    {
                        CurrentTheme = ElementTheme.Dark;
                        break;
                    }

                    case ApplicationTheme.Dark:
                    {
                        CurrentTheme = ElementTheme.Light;
                        break;
                    }
                }

                break;
            }

            case ElementTheme.Light:
            {
                CurrentTheme = ElementTheme.Dark;
                break;
            }

            case ElementTheme.Dark:
            {
                CurrentTheme = ElementTheme.Light;
                break;
            }
        }
    }
}
