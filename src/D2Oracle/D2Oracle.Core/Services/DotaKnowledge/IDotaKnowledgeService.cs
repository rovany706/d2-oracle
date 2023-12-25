namespace D2Oracle.Core.Services.DotaKnowledge;

public interface IDotaKnowledgeService
{
    /// <summary>
    /// Key - internal item name, Value - item cost
    /// </summary>
    public IReadOnlyDictionary<string, uint> Items { get; }
    
    /// <summary>
    /// Key - internal hero name, Value - localized name
    /// </summary>
    public IReadOnlyDictionary<string, string> Heroes { get; }
}