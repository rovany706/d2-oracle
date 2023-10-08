namespace D2Oracle.Core.Services.Roshan;

public interface IRoshanTimerService
{
    event EventHandler MinRoshanRespawnTimeReached;

    event EventHandler MaxRoshanRespawnTimeReached;

    event EventHandler RoshanKilled;
    
    TimeSpan? MinRoshanRespawnClockTime { get; }

    TimeSpan? MaxRoshanRespawnClockTime { get; }
    
    bool IsRoshanAlive { get; }
}