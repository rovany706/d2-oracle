namespace D2Oracle.Core.Services;

public interface IDotaProcessLocator
{
    IObservable<bool> IsDotaProcessRunningObservable { get; }
}