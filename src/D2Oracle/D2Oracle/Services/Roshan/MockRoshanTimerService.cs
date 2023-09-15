using System;
using Dota2GSI;

namespace D2Oracle.Services.Roshan;

public class MockRoshanTimerService : IRoshanTimerService
{
    public void ProcessGameState(GameState gameState)
    {
        throw new NotImplementedException();
    }

    public event EventHandler? MinRoshanRespawnTimeReached;

    public event EventHandler? MaxRoshanRespawnTimeReached;

    public TimeSpan? MinRoshanRespawnTime { get; }

    public TimeSpan? MaxRoshanRespawnTime { get; }

    public void RaiseEvent(EventHandler? eventHandler)
    {
        eventHandler?.Invoke(this, EventArgs.Empty);
    }
}