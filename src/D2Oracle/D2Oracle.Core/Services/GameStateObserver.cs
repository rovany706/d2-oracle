using Dota2GSI;

namespace D2Oracle.Core.Services;

public abstract class GameStateObserver
{
    protected GameStateObserver(IDotaGsiService dotaGsiService)
    {
        dotaGsiService.GameStateObservable.Subscribe(ProcessGameState);
    }
    protected abstract void ProcessGameState(GameState? gameState);
}