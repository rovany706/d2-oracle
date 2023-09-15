using System;
using System.Linq;
using Dota2GSI;
using Dota2GSI.Nodes.Events;

namespace D2Oracle.Services.Roshan;

public class RoshanTimerService : IRoshanTimerService
{
    private TimeSpan? roshanLastDeathTime;
    private bool isNotifiedAboutMinRoshanRespawnTime;
    private bool isNotifiedAboutMaxRoshanRespawnTime;

    public RoshanTimerService(IDotaGSIService dotaGsiService)
    {
        dotaGsiService.GameStateObservable.Subscribe(ProcessGameState);
    }

    public event EventHandler? MinRoshanRespawnTimeReached;

    public event EventHandler? MaxRoshanRespawnTimeReached;
    
    public TimeSpan? MinRoshanRespawnTime => this.roshanLastDeathTime?.Add(TimeSpan.FromMinutes(8));

    public TimeSpan? MaxRoshanRespawnTime => this.roshanLastDeathTime?.Add(TimeSpan.FromMinutes(11));

    public void ProcessGameState(GameState gameState)
    {
        if (gameState.Map is null)
        {
            return;
        }
        
        var roshanDeathEvent = GetRoshanDeathEvent(gameState);

        if (roshanDeathEvent is not null)
        {
            RegisterRoshanDeath(roshanDeathEvent.GameTime);

            return;
        }

        var currentTime = TimeSpan.FromSeconds(gameState.Map.GameTime);

        CheckAndNotifyAboutMinRespawn(currentTime);
        CheckAndNotifyAboutMaxRespawn(currentTime);
    }

    private void CheckAndNotifyAboutMaxRespawn(TimeSpan currentTime)
    {
        if (this.isNotifiedAboutMaxRoshanRespawnTime && currentTime > MaxRoshanRespawnTime)
        {
            MaxRoshanRespawnTimeReached?.Invoke(this, EventArgs.Empty);
            this.isNotifiedAboutMaxRoshanRespawnTime = true;
        }
    }

    private void CheckAndNotifyAboutMinRespawn(TimeSpan currentTime)
    {
        if (!this.isNotifiedAboutMinRoshanRespawnTime && currentTime > MinRoshanRespawnTime)
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
        this.roshanLastDeathTime = TimeSpan.FromSeconds(deathTime);
        this.isNotifiedAboutMinRoshanRespawnTime = this.isNotifiedAboutMaxRoshanRespawnTime = false;
    }
}