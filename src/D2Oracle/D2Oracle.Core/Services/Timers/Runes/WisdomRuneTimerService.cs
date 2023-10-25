using D2Oracle.Core.Extensions;
using Dota2GSI;

namespace D2Oracle.Core.Services.Timers.Runes;

public class WisdomRuneTimerService : GameStateObserver, IWisdomRuneTimerService
{
    private readonly TimeSpan timeBeforeNotification = TimeSpan.FromSeconds(45);
    private bool isNotified;

    public WisdomRuneTimerService(IDotaGsiService dotaGsiService) : base(dotaGsiService)
    {
    }

    public int WisdomRuneSpawnTimeMultiplierInMinutes => 7;
    
    public event EventHandler? WisdomRuneSpawnsSoon;
    
    protected override void ProcessGameState(GameState? gameState)
    {
        if (gameState?.Map is null || !gameState.IsInGame())
        {
            return;
        }

        if (gameState.Map.ClockTime < 0)
        {
            return;
        }

        var clockTime = TimeSpan.FromSeconds(gameState.Map.ClockTime);
        var nextWisdomRuneMinute = clockTime.TotalMinutes.ClosestMultipleCeil(WisdomRuneSpawnTimeMultiplierInMinutes);
        var nextWisdomRuneTime = TimeSpan.FromMinutes(nextWisdomRuneMinute);
        var notificationTime = nextWisdomRuneTime - timeBeforeNotification;

        switch (isNotified)
        {
            case false
                when clockTime > notificationTime
                     && clockTime < nextWisdomRuneTime:
                isNotified = true;
                WisdomRuneSpawnsSoon?.Invoke(this, EventArgs.Empty);

                break;
            case true when clockTime < nextWisdomRuneTime
                           && clockTime < notificationTime:
                isNotified = false;

                break;
        }
    }
}