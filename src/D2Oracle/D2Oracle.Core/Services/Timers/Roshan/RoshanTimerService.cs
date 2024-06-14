using System.Diagnostics;
using D2Oracle.Core.Extensions;
using D2Oracle.Core.Services.DotaKnowledge;
using Dota2GSI;

namespace D2Oracle.Core.Services.Timers.Roshan;

public class RoshanTimerService : GameStateObserver, IRoshanTimerService
{
    private readonly IRoshanDeathObserverService roshanDeathObserverService;
    private readonly TimeSpan minRoshanRespawnTime = TimeSpan.FromMinutes(8);
    private readonly TimeSpan maxRoshanRespawnTime = TimeSpan.FromMinutes(11);

    private bool isNotifiedAboutMinRoshanRespawnTime;
    private bool isNotifiedAboutMaxRoshanRespawnTime;

    public RoshanTimerService(IRoshanDeathObserverService roshanDeathObserverService, IDotaGsiService dotaGsiService) : base(dotaGsiService)
    {
        this.roshanDeathObserverService = roshanDeathObserverService;
        this.roshanDeathObserverService.IsRoshanAliveChanged += OnIsRoshanAliveChanged;
    }
    
    public event EventHandler? MinRoshanRespawnTimeReached;

    public event EventHandler? MaxRoshanRespawnTimeReached;
    
    public TimeSpan? MinRoshanRespawnClockTime => this.roshanDeathObserverService.RoshanLastDeathClockTime?
        .Add(this.minRoshanRespawnTime);

    public TimeSpan? MaxRoshanRespawnClockTime => this.roshanDeathObserverService.RoshanLastDeathClockTime?
        .Add(this.maxRoshanRespawnTime);

    private void OnIsRoshanAliveChanged(object? sender, EventArgs e)
    {
        this.isNotifiedAboutMinRoshanRespawnTime = this.isNotifiedAboutMaxRoshanRespawnTime = false;
    }
    
    protected override void OnCurrentMatchIdChanged()
    {
        this.isNotifiedAboutMinRoshanRespawnTime = this.isNotifiedAboutMaxRoshanRespawnTime = false;
    }
    
    protected override void ProcessGameState(GameState? gameState)
    {
        base.ProcessGameState(gameState);

        if (gameState?.Map is null || !gameState.IsInGame())
        {
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
            this.roshanDeathObserverService.IsRoshanAlive = true;  // Roshan 100% respawned
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
}