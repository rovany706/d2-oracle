using Dota2GSI;
using Dota2GSI.Nodes;
using Dota2GSI.Nodes.Abilities;
using Dota2GSI.Nodes.Events;
using Dota2GSI.Nodes.Items;

namespace D2Oracle.Core.Tests;

public static class GameStateTestHelper
{
    public static GameState CreateDefaultGameState(int gameTime = 150, int clockTime = 100)
    {
        return new GameState(
        CreateDefaultProvider(),
        CreateDefaultMap(gameTime, clockTime),
        CreateDefaultPlayer(),
        CreateDefaultAbilities(),
        CreateDefaultHero(),
        CreateDefaultAuth(),
        CreateDefaultItems(),
        CreateDefaultEvents()
    );
    }

    public static List<DotaEvent> CreateDefaultEvents()
    {
        return new List<DotaEvent>();
    }

    public static DotaItems CreateDefaultItems()
    {
        return new DotaItems(new[]
            {
                new DotaItem("item_octarine_core", 0, 1, false, 0, true, 0),
                new DotaItem("item_blink", 0, 1, true, 0, false, 0),
                new DotaItem("item_travel_boots", 0, 1, false, 0, true, 0),
                new DotaItem("item_black_king_bar", 0, 1, true, 0, false, 0),
                new DotaItem("item_pipe", 0, 1, true, 0, false, 0),
                new DotaItem("empty", 0, 0, false, 0, false, 0),
                new DotaItem("item_tango", 0, 1, true, 0, false, 3),
                new DotaItem("item_magic_wand", 0, 1, false, 0, false, 0),
                new DotaItem("item_enchanted_mango", 0, 1, true, 0, false, 1)
            },
            new[]
            {
                new DotaItem("item_heavens_halberd", 0, 1, true, 0, false, 0),
                new DotaItem("item_crimson_guard", 0, 1, true, 0, false, 0),
                new DotaItem("item_lotus_orb", 0, 1, true, 0, false, 0),
                new DotaItem("item_silver_edge", 0, 1, true, 0, false, 0),
                new DotaItem("empty", 0, 0, false, 0, false, 0),
                new DotaItem("empty", 0, 0, false, 0, false, 0)
            },
            new DotaItem("item_tpscroll", 0, 1, false, 62, false, 1),
            new DotaItem("item_timeless_relic", 0, 1, false, 0, true, 0));
    }

    public static Auth CreateDefaultAuth()
    {
        return new Auth("token");
    }

    public static Hero CreateDefaultHero()
    {
        return new Hero(1, 7711, -7878, 100, "npc_dota_hero_tusk", 30, 64400, true, 0, 7929, 0, 3736, 3745, 99, 1047, 1047, 100,
            false, false, false, false, false, false, false, false, false, false, false, true, true, true, true, true,
            true, true, true, 7);
    }

    public static DotaAbilities CreateDefaultAbilities()
    {
        return new DotaAbilities(new[]
        {
            new DotaAbility("tusk_ice_shards", 4, true, false, true, 0, false),
            new DotaAbility("tusk_snowball", 4, true, false, true, 0, false),
            new DotaAbility("tusk_tag_team", 4, true, false, true, 0, false),
            new DotaAbility("tusk_walrus_punch", 3, true, false, true, 0, true),
            new DotaAbility("plus_high_five", 1, true, false, true, 0, false),
            new DotaAbility("plus_guild_banner", 1, true, false, true, 0, false),
            new DotaAbility("seasonal_10th_anniversary_party_hat", 1, true, false, true, 0, false),
        });
    }

    public static Player CreateDefaultPlayer()
    {
        return new Player("11111111111111111", "11111111", "ayylmao", "playing", 0, 0, 0, 1, 0, 0, 122, "radiant", 1, 0,
            70127, 38, 70089, 0, 0, 38, 0, 2372369, 152723);
    }

    public static Map CreateDefaultMap(int gameTime = 150, int clockTime = 100)
    {
        return new Map("start", "7331674997", gameTime, clockTime, true, false, 0, 0, DotaGameState.DOTA_GAMERULES_STATE_GAME_IN_PROGRESS,
            false, PlayerTeam.None, "", 0);
    }

    public static Provider CreateDefaultProvider()
    {
        return new Provider("Dota 2", 570, 47, 1694530163);
    }

    public static DotaEvent CreateRoshanDeathEvent(int deathClockTime)
    {
        return new RoshanKilledEvent(deathClockTime, DotaEventType.RoshanKilled, "radiant", 1);
    }
}