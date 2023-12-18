using D2Oracle.Core.Extensions;
using Dota2GSI;
using Dota2GSI.Nodes;

namespace D2Oracle.Core.Tests.Extensions;

public class GameStateExtensionsTests
{
    [Test]
    public void IsInGame_WhenGameStateIsNull_ReturnFalse()
    {
        GameState? gameState = null;

        var actual = gameState.IsInGame();

        Assert.That(actual, Is.False);
    }

    [Test]
    public void IsInGame_WhenMapIsNull_ReturnFalse()
    {
        var gameState = GameStateTestHelper.CreateDefaultGameState() with { Map = null };

        var actual = gameState.IsInGame();

        Assert.That(actual, Is.False);
    }

    [Test]
    [TestCase(DotaGameState.Undefined, false)]
    [TestCase(DotaGameState.DOTA_GAMERULES_STATE_INIT, false)]
    [TestCase(DotaGameState.DOTA_GAMERULES_STATE_LAST, false)]
    [TestCase(DotaGameState.DOTA_GAMERULES_STATE_POST_GAME, false)]
    [TestCase(DotaGameState.DOTA_GAMERULES_STATE_WAIT_FOR_PLAYERS_TO_LOAD, false)]
    [TestCase(DotaGameState.DOTA_GAMERULES_STATE_CUSTOM_GAME_SETUP, false)]
    [TestCase(DotaGameState.DOTA_GAMERULES_STATE_WAIT_FOR_MAP_TO_LOAD, false)]
    [TestCase(DotaGameState.DOTA_GAMERULES_STATE_TEAM_SHOWCASE, true)]
    [TestCase(DotaGameState.DOTA_GAMERULES_STATE_GAME_IN_PROGRESS, true)]
    [TestCase(DotaGameState.DOTA_GAMERULES_STATE_HERO_SELECTION, true)]
    [TestCase(DotaGameState.DOTA_GAMERULES_STATE_PRE_GAME, true)]
    [TestCase(DotaGameState.DOTA_GAMERULES_STATE_STRATEGY_TIME, true)]
    [TestCase(DotaGameState.DOTA_GAMERULES_STATE_DISCONNECT, true)]
    public void IsInGame_ReturnExpected(DotaGameState dotaGameState, bool expected)
    {
        var map = GameStateTestHelper.CreateDefaultMap() with { GameState = dotaGameState };
        var gameState = GameStateTestHelper.CreateDefaultGameState() with { Map = map };

        var actual = gameState.IsInGame();

        Assert.That(actual, Is.EqualTo(expected));
    }
}