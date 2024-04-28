using D2Oracle.Core.Resources;
using D2Oracle.Core.Services.DotaKnowledge.Models;

namespace D2Oracle.Core.Services.Timers.Roshan;

public class RoshanDeathItemProvider
{
    private List<RoshanItemDrop>? roshanItemDrops;
    
    public IReadOnlyList<RoshanItemDrop> RoshanItemDrops
    {
        get
        {
            this.roshanItemDrops ??= DeserializeItems();

            return this.roshanItemDrops;
        }
    }

    private static List<RoshanItemDrop> DeserializeItems()
    {
        var items = JsonResourceDeserializer.DeserializeResource<List<RoshanItemDrop>>("roshan_items.json");

        return items;
    }
}