using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace D2Oracle.Core.Resources;

public static class JsonResourceDeserializer
{
    public static T DeserializeResource<T>(string resourceFileName)
    {
        var serializer = CreateSerializer();
        var resourceFilePath = GetResourceFilePath(resourceFileName);
        using var streamReader = File.OpenText(resourceFilePath);

        return (T)serializer.Deserialize(streamReader, typeof(T))!;
    }
    
    private static string GetResourceFilePath(string resourceFileName)
    {
        return Path.Combine(Constants.ResourcesFolderPath, Constants.JsonResourcesFolder, resourceFileName);
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
}