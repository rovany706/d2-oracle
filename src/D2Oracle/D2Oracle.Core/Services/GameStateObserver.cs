using Dota2GSI;

namespace D2Oracle.Core.Services;

public abstract class GameStateObserver
{
    private string lastMatchId = string.Empty;
    
    protected GameStateObserver(IDotaGsiService dotaGsiService)
    {
        dotaGsiService.GameStateObservable.Subscribe(ProcessGameState);
    }

    protected virtual void ProcessGameState(GameState? gameState)
    {
        var currentMatchId = gameState?.Map?.Matchid;
        
        if (currentMatchId != null && currentMatchId != this.lastMatchId)
        {
            this.lastMatchId = currentMatchId;
            OnCurrentMatchIdChanged();
        }
    }
    
    protected abstract void OnCurrentMatchIdChanged();
}