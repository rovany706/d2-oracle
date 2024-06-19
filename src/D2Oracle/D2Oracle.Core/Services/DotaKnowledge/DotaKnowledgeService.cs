using D2Oracle.Core.Resources;
using D2Oracle.Core.Services.DotaKnowledge.Models;

namespace D2Oracle.Core.Services.DotaKnowledge;

public class DotaKnowledgeService : IDotaKnowledgeService
{
    private Dictionary<string, uint>? items;
    private Dictionary<string, string>? heroes;

    /// <inheritdoc />
    public IReadOnlyDictionary<string, uint> Items
    {
        get
        {
            this.items ??= GetItemsDictionary();

            return this.items;
        }
    }

    /// <inheritdoc />
    public IReadOnlyDictionary<string, string> Heroes
    {
        get
        {
            this.heroes ??= GetHeroesDictionary();

            return this.heroes;
        }
    }

    private static Dictionary<string, uint> GetItemsDictionary()
    {
        return DeserializeDictionary<ItemDescription, string, uint>(
            "items.json",
            item => item.Name,
            item => (uint)item.Cost
        );
    }

    private static Dictionary<string, string> GetHeroesDictionary()
    {
        return DeserializeDictionary<HeroDescription, string, string>(
            "heroes.json",
            hero => hero.Name,
            hero => hero.LocalizedName
        );
    }

    private static Dictionary<TKey, TValue> DeserializeDictionary<TSource, TKey, TValue>(string resourceFileName,
        Func<TSource, TKey> keySelector, Func<TSource, TValue> valueSelector) where TKey : notnull
    {
        var sourceList = JsonResourceDeserializer.DeserializeResource<List<TSource>>(resourceFileName);

        return sourceList.ToDictionary(keySelector, valueSelector);
    }
}