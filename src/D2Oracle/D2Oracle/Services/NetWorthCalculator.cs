using System.Linq;
using D2Oracle.Services.DotaKnowledge;
using Dota2GSI;
using Dota2GSI.Nodes.Items;

namespace D2Oracle.Services;

public class NetWorthCalculator
{
    private readonly IDotaKnowledgeService dotaKnowledgeService;

    public NetWorthCalculator(IDotaKnowledgeService dotaKnowledgeService)
    {
        this.dotaKnowledgeService = dotaKnowledgeService;
    }

    public uint Calculate(GameState? gameState)
    {
        if (gameState?.Items is null || gameState.Player is null)
        {
            return 0;
        }

        var items = gameState.Items.MainItems.Union(gameState.Items.StashItems);
        var gold = gameState.Player.Gold;
        
        var itemDescriptions = items.Select(GetItemDescription);

        return gold + (uint) itemDescriptions.Sum(x => x?.Cost ?? 0);
    }

    private ItemDescription? GetItemDescription(DotaItem item)
    {
        return dotaKnowledgeService.Items.SingleOrDefault(description => description.Name.Equals(item.Name));
    }
}