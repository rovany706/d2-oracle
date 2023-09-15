using System;

namespace D2Oracle.Services.Roshan;

public interface IRoshanTimerService : IDotaGameStateObserver
{
    event EventHandler MinRoshanRespawnTimeReached;

    event EventHandler MaxRoshanRespawnTimeReached;
    
    TimeSpan? MinRoshanRespawnTime { get; }

    TimeSpan? MaxRoshanRespawnTime { get; }
}