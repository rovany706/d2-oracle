using System.Diagnostics;
using System.Reactive.Linq;

namespace D2Oracle.Core.Services;

public class DotaProcessLocator : IDotaProcessLocator
{
    private static bool IsDotaProcessRunning()
    {
        var dotaProcesses = Process.GetProcessesByName("dota2");
        
        return dotaProcesses.Any();
    }

    public IObservable<bool> IsDotaProcessRunningObservable { get; } = 
        Observable.Timer(TimeSpan.Zero, TimeSpan.FromSeconds(5))
        .Select(_ => IsDotaProcessRunning());
}