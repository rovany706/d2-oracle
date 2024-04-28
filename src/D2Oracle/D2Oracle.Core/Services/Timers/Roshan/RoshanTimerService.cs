using D2Oracle.Core.Extensions;
using Dota2GSI;
using Dota2GSI.Nodes.Events;

namespace D2Oracle.Core.Services.Timers.Roshan;

public class RoshanTimerService : GameStateObserver, IRoshanTimerService
{
    private readonly TimeSpan minRoshanRespawnTime = TimeSpan.FromMinutes(8);
    private readonly TimeSpan maxRoshanRespawnTime = TimeSpan.FromMinutes(11);

    private bool isNotifiedAboutMinRoshanRespawnTime;
    private bool isNotifiedAboutMaxRoshanRespawnTime;
    private TimeSpan? roshanLastDeathClockTime;
    private int deathCount = 0;

    public RoshanTimerService(IDotaGsiService dotaGsiService) : base(dotaGsiService)
    {
    }

    public event EventHandler? MinRoshanRespawnTimeReached;

    public event EventHandler? MaxRoshanRespawnTimeReached;

    public event EventHandler? RoshanLastDeathClockTimeChanged;

    public TimeSpan? MinRoshanRespawnClockTime => RoshanLastDeathClockTime?
        .Add(this.minRoshanRespawnTime);

    public TimeSpan? MaxRoshanRespawnClockTime => RoshanLastDeathClockTime?
        .Add(this.maxRoshanRespawnTime);

    public TimeSpan? RoshanLastDeathClockTime
    {
        get => this.roshanLastDeathClockTime;
        private set
        {
            if (this.roshanLastDeathClockTime.EqualsWithPrecision(value, TimeSpan.FromSeconds(2)))
            {
                return;
            }
            
            this.roshanLastDeathClockTime = value;
            this.deathCount++;
            this.isNotifiedAboutMinRoshanRespawnTime = this.isNotifiedAboutMaxRoshanRespawnTime = false;
            RoshanLastDeathClockTimeChanged?.Invoke(this, EventArgs.Empty);
        }
    }

    public bool IsRoshanAlive => RoshanLastDeathClockTime is null;

    protected override void ProcessGameState(GameState? gameState)
    {
        base.ProcessGameState(gameState);
        
        if (gameState?.Map is null || !gameState.IsInGame())
        {
            return;
        }
        
        var roshanDeathEvent = GetRoshanDeathEvent(gameState);

        if (roshanDeathEvent is not null)
        {
            var gameTimeClockTimeDifference = Math.Abs(gameState.Map.GameTime - gameState.Map.ClockTime);
            RoshanLastDeathClockTime = TimeSpan.FromSeconds(roshanDeathEvent.GameTime - gameTimeClockTimeDifference);

            return;
        }

        var currentTime = TimeSpan.FromSeconds(gameState.Map.ClockTime);

        CheckAndNotifyAboutMinRespawn(currentTime);
        CheckAndNotifyAboutMaxRespawn(currentTime);
    }

    protected override void OnCurrentMatchIdChanged()
    {
        RoshanLastDeathClockTime = null;
        this.deathCount = 0;
    }

    private void CheckAndNotifyAboutMaxRespawn(TimeSpan currentTime)
    {
        if (!this.isNotifiedAboutMaxRoshanRespawnTime && currentTime > MaxRoshanRespawnClockTime)
        {
            RoshanLastDeathClockTime = null; // Roshan 100% respawned
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
}