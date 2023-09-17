using System;
using System.Linq;
using D2Oracle.Extensions;
using Dota2GSI;
using Dota2GSI.Nodes.Events;

namespace D2Oracle.Services.Roshan;

public class RoshanTimerService : IRoshanTimerService
{
    private const int MinRoshanRespawnTimeInMinutes = 8;
    private const int MaxRoshanRespawnTimeInMinutes = 11;

    private TimeSpan? roshanLastDeathClockTime;
    private bool isNotifiedAboutMinRoshanRespawnTime;
    private bool isNotifiedAboutMaxRoshanRespawnTime;

    public RoshanTimerService(IDotaGsiService dotaGsiService)
    {
        dotaGsiService.GameStateObservable.Subscribe(ProcessGameState);
    }

    public event EventHandler? MinRoshanRespawnTimeReached;

    public event EventHandler? MaxRoshanRespawnTimeReached;

    public event EventHandler? RoshanKilled;

    public TimeSpan? MinRoshanRespawnClockTime => this.roshanLastDeathClockTime?
        .Add(TimeSpan.FromMinutes(MinRoshanRespawnTimeInMinutes));

    public TimeSpan? MaxRoshanRespawnClockTime => this.roshanLastDeathClockTime?
        .Add(TimeSpan.FromMinutes(MaxRoshanRespawnTimeInMinutes));

    public bool IsRoshanAlive => roshanLastDeathClockTime is null;

    private void ProcessGameState(GameState? gameState)
    {
        if (gameState?.Map is null || !gameState.IsInGame())
        {
            roshanLastDeathClockTime = null;
            
            return;
        }

        var roshanDeathEvent = GetRoshanDeathEvent(gameState);

        if (roshanDeathEvent is not null && IsRoshanAlive)
        {
            var gameTimeClockTimeDifference = Math.Abs(gameState.Map.GameTime - gameState.Map.ClockTime);
            RegisterRoshanDeath(roshanDeathEvent.GameTime - gameTimeClockTimeDifference);
            RoshanKilled?.Invoke(this, EventArgs.Empty);

            return;
        }

        var currentTime = TimeSpan.FromSeconds(gameState.Map.ClockTime);

        CheckAndNotifyAboutMinRespawn(currentTime);
        CheckAndNotifyAboutMaxRespawn(currentTime);
    }

    private void CheckAndNotifyAboutMaxRespawn(TimeSpan currentTime)
    {
        if (!this.isNotifiedAboutMaxRoshanRespawnTime && currentTime > MaxRoshanRespawnClockTime)
        {
            roshanLastDeathClockTime = null; // Roshan 100% respawned
            MaxRoshanRespawnTimeReached?.Invoke(this, EventArgs.Empty);
            this.isNotifiedAboutMaxRoshanRespawnTime = true;
        }
    }

    private void CheckAndNotifyAboutMinRespawn(TimeSpan currentTime)
    {
        if (!this.isNotifiedAboutMinRoshanRespawnTime && currentTime > MinRoshanRespawnClockTime)
        {
            MinRoshanRespawnTimeReached?.Invoke(this, EventArgs.Empty);
            this.isNotifiedAboutMinRoshanRespawnTime = true;
        }
    }

    private static DotaEvent? GetRoshanDeathEvent(GameState gameState)
    {
        return gameState.Events.SingleOrDefault(x => x.EventType == DotaEventType.RoshanKilled);
    }

    private void RegisterRoshanDeath(int deathTime)
    {
        this.roshanLastDeathClockTime = TimeSpan.FromSeconds(deathTime);
        this.isNotifiedAboutMinRoshanRespawnTime = this.isNotifiedAboutMaxRoshanRespawnTime = false;
    }
}