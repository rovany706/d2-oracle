using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace D2Oracle.Core.Services.DotaKnowledge;

public class DotaKnowledgeService : IDotaKnowledgeService
{
    private readonly JsonSerializer serializer = CreateSerializer();
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

    private Dictionary<string, uint> GetItemsDictionary()
    {
        return DeserializeDictionary<ItemDescription, string, uint>(
            "items.json",
            item => item.Name,
            item => (uint)item.Cost
        );
    }

    private Dictionary<string, string> GetHeroesDictionary()
    {
        return DeserializeDictionary<HeroDescription, string, string>(
            "heroes.json",
            hero => hero.Name,
            hero => hero.LocalizedName
        );
    }

    private Dictionary<TKey, TValue> DeserializeDictionary<TSource, TKey, TValue>(string resourceFileName,
        Func<TSource, TKey> keySelector, Func<TSource, TValue> valueSelector) where TKey : notnull
    {
        var sourceList = DeserializeResource<List<TSource>>(this.serializer, resourceFileName);

        return sourceList.ToDictionary(keySelector, valueSelector);
    }

    private static JsonSerializer CreateSerializer()
    {
        var serializer = JsonSerializer.Create(new JsonSerializerSettings
        {
            ContractResolver = new DefaultContractResolver
            {
                NamingStrategy = new SnakeCaseNamingStrategy()
            }
        });

        return serializer;
    }

    private static T DeserializeResource<T>(JsonSerializer serializer, string resourceFileName)
    {
        var resourceFilePath = GetResourceFilePath(resourceFileName);
        using var streamReader = File.OpenText(resourceFilePath);

        return (T)serializer.Deserialize(streamReader, typeof(T))!;
    }

    private static string GetResourceFilePath(string resourceFileName)
    {
        return Path.Combine(Constants.ResourcesFolderPath, resourceFileName);
    }
}