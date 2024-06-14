namespace D2Oracle.Core.Services.DotaKnowledge;

public interface IRoshanDeathObserverService
{
    event EventHandler? IsRoshanAliveChanged;

    TimeSpan? RoshanLastDeathClockTime { get; }

    int RoshanDeathCount { get; }
    
    bool IsRoshanAlive { get; set; }
}