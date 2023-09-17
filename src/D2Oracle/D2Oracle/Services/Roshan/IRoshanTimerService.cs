using System;

namespace D2Oracle.Services.Roshan;

public interface IRoshanTimerService
{
    event EventHandler MinRoshanRespawnTimeReached;

    event EventHandler MaxRoshanRespawnTimeReached;

    event EventHandler RoshanKilled;
    
    TimeSpan? MinRoshanRespawnClockTime { get; }

    TimeSpan? MaxRoshanRespawnClockTime { get; }
    
    bool IsRoshanAlive { get; }
}