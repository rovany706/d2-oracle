namespace D2Oracle.Core.Services;

public interface IDispatcherService
{
    void Post(Action action);

    Task InvokeAsync(Action action);

    Task<TResult> InvokeAsync<TResult>(Func<TResult> action);
}