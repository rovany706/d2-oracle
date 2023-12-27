using D2Oracle.Core.Services;
using Dota2GSI;

namespace D2Oracle.Core.ViewModels;

public abstract class GameStateViewModelBase : ViewModelBase
{
    private string lastMatchId;

    protected GameStateViewModelBase(IDotaGsiService dotaGsiService)
    {
        dotaGsiService.GameStateObservable.Subscribe(OnNewGameState);
    }

    protected virtual void OnNewGameState(GameState? gameState)
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