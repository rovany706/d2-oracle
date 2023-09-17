using System.Collections.Generic;
using System.Linq;
using D2Oracle.Services.DotaKnowledge;
using Dota2GSI.Nodes.Items;

namespace D2Oracle.Services;

public class NetWorthCalculator
{
    private readonly IDotaKnowledgeService dotaKnowledgeService;

    public NetWorthCalculator(IDotaKnowledgeService dotaKnowledgeService)
    {
        this.dotaKnowledgeService = dotaKnowledgeService;
    }

    public uint Calculate(uint gold, IEnumerable<DotaItem> items)
    {
        var itemDescriptions = items.Select(GetItemDescription);

        return gold + (uint) itemDescriptions.Sum(x => x?.Cost ?? 0);
    }

    private ItemDescription? GetItemDescription(DotaItem item)
    {
        return dotaKnowledgeService.Items.SingleOrDefault(description => description.Name.Equals(item.Name));
    }
}