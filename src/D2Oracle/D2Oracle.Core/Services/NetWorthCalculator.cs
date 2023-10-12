using D2Oracle.Core.Services.DotaKnowledge;
using Dota2GSI;
using Dota2GSI.Nodes.Items;

namespace D2Oracle.Core.Services;

public class NetWorthCalculator
{
    private const string AghanimsShardName = "item_aghanims_shard";
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

        var itemDescriptions = items.Select(GetItemDescription);
        return gold + (uint)itemDescriptions.Sum(x => x?.Cost ?? 0);
    }

    private ItemDescription? GetItemDescription(DotaItem item)
    {
        return GetItemDescription(item.Name);
    }

    private ItemDescription? GetItemDescription(string itemName)
    {
        return dotaKnowledgeService.Items.SingleOrDefault(description => description.Name.Equals(itemName));
    }

    private uint GetAghanimsShardCost(GameState gameState)
    {
        if (gameState.Hero?.AghanimsShard == false)
        {
            return 0;
        }

        return (uint?)GetItemDescription(AghanimsShardName)?.Cost ?? 0;
    }

    /// <summary>
    /// Get aghanims blessing cost if player has buff with no scepter.
    /// Does not determine if buff was received from Alchemist (4200 gold instead of 5800).  
    /// </summary>
    private uint GetAghanimsBlessingCost(GameState gameState)
    {
        var hasPlayerAghanimsBlessing = gameState.Hero?.AghanimsScepter == true // has aghanims upgrade
                                        && gameState.Items?.MainItems.SingleOrDefault(x => // but no scepter
                                            x.Name == "item_ultimate_scepter") == null;

        if (hasPlayerAghanimsBlessing)
        {
            return (uint?)GetItemDescription("item_ultimate_scepter_2")?.Cost ?? 0;
        }

        return 0;
    }
}