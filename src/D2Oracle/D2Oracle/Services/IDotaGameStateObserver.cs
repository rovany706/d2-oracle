using Dota2GSI;

namespace D2Oracle.Services;

public interface IDotaGameStateObserver
{
    void ProcessGameState(GameState gameState);
}