using System;
using System.Threading.Tasks;

using CommunityToolkit.WinUI;

using Firell.Toolkit.Common.Dispatching;

namespace Firell.Toolkit.WinUI;

/// <summary>
/// An implementation of <see cref="Microsoft.UI.Dispatching.DispatcherQueue"/> that conforms to the <see cref="IDispatcherQueue"/> interface.
/// </summary>
public sealed class DispatcherQueue : IDispatcherQueue
{
    private readonly Microsoft.UI.Dispatching.DispatcherQueue _dispatcherQueue;

    public DispatcherQueue()
    {
        _dispatcherQueue = Microsoft.UI.Dispatching.DispatcherQueue.GetForCurrentThread();
    }

    public bool HasThreadAccess { get => _dispatcherQueue.HasThreadAccess; }

    public bool Enqueue(Action action, DispatcherQueuePriority priority = DispatcherQueuePriority.Normal)
    {
        return _dispatcherQueue.TryEnqueue(ConvertDispatcherQueuePriority(priority), () => action.Invoke());
    }

    public Task EnqueueAsync(Action action, DispatcherQueuePriority priority = DispatcherQueuePriority.Normal)
    {
        return _dispatcherQueue.EnqueueAsync(action, ConvertDispatcherQueuePriority(priority));
    }

    public Task EnqueueAsync(Func<Task> function, DispatcherQueuePriority priority = DispatcherQueuePriority.Normal)
    {
        return _dispatcherQueue.EnqueueAsync(function, ConvertDispatcherQueuePriority(priority));
    }

    public Task<T> EnqueueAsync<T>(Func<T> function, DispatcherQueuePriority priority = DispatcherQueuePriority.Normal)
    {
        return _dispatcherQueue.EnqueueAsync(function, ConvertDispatcherQueuePriority(priority));
    }

    private static Microsoft.UI.Dispatching.DispatcherQueuePriority ConvertDispatcherQueuePriority(DispatcherQueuePriority priority)
    {
        switch (priority)
        {
            case DispatcherQueuePriority.Low:
            {
                return Microsoft.UI.Dispatching.DispatcherQueuePriority.Low;
            }

            case DispatcherQueuePriority.Normal:
            {
                return Microsoft.UI.Dispatching.DispatcherQueuePriority.Normal;
            }

            case DispatcherQueuePriority.High:
            {
                return Microsoft.UI.Dispatching.DispatcherQueuePriority.High;
            }

            default:
            {
                return Microsoft.UI.Dispatching.DispatcherQueuePriority.Normal;
            }
        }
    }
}
