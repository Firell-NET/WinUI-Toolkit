using System;

using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Media;

namespace Firell.Toolkit.WinUI.Extensions;

public static class DependencyObjectExtensions
{
    /// <summary>
    /// Find the last ascendant element of a given type, using a depth-first search.
    /// </summary>
    public static TElement? FindLastAscendant<TElement>(this DependencyObject element) where TElement : notnull, DependencyObject
    {
        return element.FindLastAscendant<TElement>((element) => true);
    }

    /// <summary>
    /// Find the last ascendant element matching a given predicate, using a depth-first search.
    /// </summary>
    public static TElement? FindLastAscendant<TElement>(this DependencyObject element, Predicate<TElement> predicate) where TElement : notnull, DependencyObject
    {
        TElement? lastAscendant = null;
        while (true)
        {
            DependencyObject parent = VisualTreeHelper.GetParent(element);

            if (parent is null)
            {
                return lastAscendant;
            }

            if (parent is TElement result && predicate.Invoke(result))
            {
                lastAscendant = result;
            }

            element = parent;
        }
    }

    /// <summary>
    /// Find the last descendant element of a given type, using a depth-first search.
    /// </summary>
    public static TElement? FindLastDescendant<TElement>(this DependencyObject element) where TElement : notnull, DependencyObject
    {
        return element.FindLastDescendant<TElement>((element) => true);
    }

    /// <summary>
    /// Find the last descendant element matching a given predicate, using a depth-first search.
    /// </summary>
    public static TElement? FindLastDescendant<TElement>(this DependencyObject element, Predicate<TElement> predicate) where TElement : notnull, DependencyObject
    {
        int childrenCount = VisualTreeHelper.GetChildrenCount(element);

        TElement? lastDescendant = null;
        for (int i = 0; i < childrenCount; i++)
        {
            DependencyObject child = VisualTreeHelper.GetChild(element, i);

            if (child is TElement result && predicate.Invoke(result))
            {
                lastDescendant = result;
            }

            TElement? descendant = child.FindLastDescendant(predicate);
            if (descendant is not null)
            {
                return descendant;
            }
        }

        return lastDescendant;
    }

    /// <summary>
    /// Find the last ascendant (or self) element of calling type, using a depth-first search.
    /// </summary>
    public static TElement FindLastAscendantOrSelf<TElement>(this TElement element) where TElement : notnull, DependencyObject
    {
        return element.FindLastAscendant<TElement>() ?? element;
    }

    /// <summary>
    /// Find the last ascendant (or self) element of calling type matching a given predicate, using a depth-first search.
    /// </summary>
    public static TElement FindLastAscendantOrSelf<TElement>(this TElement element, Predicate<TElement> predicate) where TElement : notnull, DependencyObject
    {
        return element.FindLastAscendant(predicate) ?? element;
    }

    /// <summary>
    /// Find the last descendant (or self) element of calling type, using a depth-first search.
    /// </summary>
    public static TElement FindLastDescendantOrSelf<TElement>(this TElement element) where TElement : notnull, DependencyObject
    {
        return element.FindLastDescendant<TElement>() ?? element;
    }

    /// <summary>
    /// Find the last descendant (or self) element of calling type matching a given predicate, using a depth-first search.
    /// </summary>
    public static TElement FindLastDescendantOrSelf<TElement>(this TElement element, Predicate<TElement> predicate) where TElement : notnull, DependencyObject
    {
        return element.FindLastDescendant(predicate) ?? element;
    }
}
