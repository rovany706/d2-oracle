namespace D2Oracle.Core.Services.DotaKnowledge;

public interface IDotaKnowledgeService
{
    public IEnumerable<ItemDescription> Items { get; }
    public IEnumerable<HeroDescription> Heroes { get; }
}