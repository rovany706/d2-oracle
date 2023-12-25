using D2Oracle.Core.Services.DotaKnowledge;
using Dota2GSI;
using Dota2GSI.Nodes.Items;

namespace D2Oracle.Core.Services.NetWorth;

public class NetWorthCalculator : INetWorthCalculator
{
    private const string AghanimsShardName = "item_aghanims_shard";
    private const string AghanimsScepterName = "item_ultimate_scepter";
    private const string AghanimsBlessingName = "item_ultimate_scepter_2";

    private readonly IDotaKnowledgeService dotaKnowledgeService;

    public NetWorthCalculator(IDotaKnowledgeService dotaKnowledgeService)
    {
        this.dotaKnowledgeService = dotaKnowledgeService;
    }

    public uint Calculate(GameState? gameState)
    {
        if (gameState?.Items is null || gameState.Player is null || gameState.Hero is null)
        {
            return 0;
        }

        var items = gameState.Items.MainItems.Union(gameState.Items.StashItems);
        var gold = gameState.Player.Gold;
        gold += GetAghanimsShardCost(gameState);
        gold += GetAghanimsBlessingCost(gameState);

        var itemsSum = items.Sum(item => GetItemCost(item.Name));
        return gold + (uint)itemsSum;
    }

    private uint GetItemCost(string itemName)
    {
        var isItemExists = this.dotaKnowledgeService.Items.TryGetValue(itemName, out var cost);

        return isItemExists ? cost : 0;
    }

    private uint GetAghanimsShardCost(GameState gameState)
    {
        return gameState.Hero?.AghanimsShard == true ? GetItemCost(AghanimsShardName) : 0;
    }

    /// <summary>
    /// Get aghanims blessing cost if player has buff with no scepter.
    /// Does not determine if buff was received from Alchemist (4200 gold instead of 5800).  
    /// </summary>
    private uint GetAghanimsBlessingCost(GameState gameState)
    {
        var hasPlayerAghanimsBlessing = gameState.Hero?.AghanimsScepter == true // has aghanims upgrade
                                        && gameState.Items?.MainItems.SingleOrDefault(x => // but no scepter
                                            x.Name == AghanimsScepterName) == null;

        return hasPlayerAghanimsBlessing ? GetItemCost(AghanimsBlessingName) : 0;
    }
}