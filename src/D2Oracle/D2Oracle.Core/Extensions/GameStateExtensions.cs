using Dota2GSI;
using Dota2GSI.Nodes;

namespace D2Oracle.Core.Extensions;

public static class GameStateExtensions
{
    public static bool IsInGame(this GameState? gameState)
    {
        if (gameState?.Map is null)
        {
            return false;
        }

        var inGameStates = new[]
        {
            DotaGameState.DOTA_GAMERULES_STATE_PRE_GAME,
            DotaGameState.DOTA_GAMERULES_STATE_STRATEGY_TIME,
            DotaGameState.DOTA_GAMERULES_STATE_TEAM_SHOWCASE,
            DotaGameState.DOTA_GAMERULES_STATE_HERO_SELECTION,
            DotaGameState.DOTA_GAMERULES_STATE_GAME_IN_PROGRESS
        };

        return inGameStates.Contains(gameState.Map.GameState);
    }
}