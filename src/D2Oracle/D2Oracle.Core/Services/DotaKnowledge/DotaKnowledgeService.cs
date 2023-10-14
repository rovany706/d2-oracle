using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace D2Oracle.Core.Services.DotaKnowledge;

public class DotaKnowledgeService : IDotaKnowledgeService
{
    public DotaKnowledgeService()
    {
        var serializer = CreateSerializer();
        Items = Deserialize<List<ItemDescription>>(serializer, ReadFile("items.json"));
        Heroes = Deserialize<List<HeroDescription>>(serializer, ReadFile("heroes.json"));
    }

    private static string ReadFile(string fileName)
    {
        var path = Path.Combine(Constants.ResourcesFolderPath, fileName);

        return File.ReadAllText(path);
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
    
    private static T Deserialize<T>(JsonSerializer serializer, string json)
    {
        using var textReader = new StringReader(json);
        using var jsonReader = new JsonTextReader(textReader);
        
        return serializer.Deserialize<T>(jsonReader)!;
    }

    public IEnumerable<ItemDescription> Items { get; }
    
    public IEnumerable<HeroDescription> Heroes { get; }
}