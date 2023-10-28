using System;
using System.Threading.Tasks;
using Avalonia.Threading;
using D2Oracle.Core.Services;

namespace D2Oracle.Avalonia.Services;

public class AvaloniaDispatcherService : IDispatcherService
{
    public void Post(Action action)
    {
        Dispatcher.UIThread.Post(action);
    }

    public async Task InvokeAsync(Action action)
    {
        await Dispatcher.UIThread.InvokeAsync(action);
    }

    public async Task<TResult> InvokeAsync<TResult>(Func<TResult> action)
    {
        return await Dispatcher.UIThread.InvokeAsync(action);
    }
}