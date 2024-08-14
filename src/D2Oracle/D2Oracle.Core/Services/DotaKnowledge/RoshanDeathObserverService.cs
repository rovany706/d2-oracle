using D2Oracle.Core.Extensions;
using Dota2GSI;
using Dota2GSI.Nodes.Events;

namespace D2Oracle.Core.Services.DotaKnowledge;

public class RoshanDeathObserverService : GameStateObserver, IRoshanDeathObserverService
{
    private bool isRoshanAlive;

    public RoshanDeathObserverService(IDotaGsiService dotaGsiService) : base(dotaGsiService)
    {
    }

    public event EventHandler? IsRoshanAliveChanged;
    
    protected override void OnCurrentMatchIdChanged()
    {
        RoshanDeathCount = 0;
        RoshanLastDeathClockTime = null;
        IsRoshanAlive = true;
    }

    public TimeSpan? RoshanLastDeathClockTime { get; private set; }

    public int RoshanDeathCount { get; private set; }

    public bool IsRoshanAlive
    {
        get => this.isRoshanAlive;
        set
        {
            if (IsRoshanAlive == value)
            {
                return;
            }
            
            this.isRoshanAlive = value;
            IsRoshanAliveChanged?.Invoke(this, EventArgs.Empty);
        }
    }

    protected override void ProcessGameState(GameState? gameState)
    {
        base.ProcessGameState(gameState);

        if (gameState?.Map is null || !gameState.IsInGame())
        {
            return;
        }

        if (!DidRoshanDie(gameState))
        {
            return;
        }

        RoshanDeathCount++;
        RoshanLastDeathClockTime = GetRoshanDeathClockTime(gameState);
        IsRoshanAlive = false;
    }

    private bool DidRoshanDie(GameState gameState)
    {
        var currentRoshanLastDeathClockTime = GetRoshanDeathClockTime(gameState);

        if (currentRoshanLastDeathClockTime is null)
        {
            return false;
        }

        //     After roshan's death Dota sends multiple RoshanDeathEvents for several seconds
        //     Check if they differ by time 
        return !RoshanLastDeathClockTime.EqualsWithPrecision(currentRoshanLastDeathClockTime, TimeSpan.FromSeconds(2));
    }

    private static TimeSpan? GetRoshanDeathClockTime(GameState gameState)
    {
        var roshanDeathEvent = GetRoshanDeathEvent(gameState);
        if (roshanDeathEvent is null) return null;

        var gameTimeClockTimeDifference = Math.Abs(gameState.Map.GameTime - gameState.Map.ClockTime);
        
        return TimeSpan.FromSeconds(roshanDeathEvent.GameTime - gameTimeClockTimeDifference);
    }

    private static DotaEvent? GetRoshanDeathEvent(GameState gameState)
    {
        return gameState.Events.SingleOrDefault(x => x.EventType == DotaEventType.RoshanKilled);
    }
}