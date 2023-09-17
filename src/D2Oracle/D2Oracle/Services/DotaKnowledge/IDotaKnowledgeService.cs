using System.Collections.Generic;

namespace D2Oracle.Services.DotaKnowledge;

public interface IDotaKnowledgeService
{
    public IEnumerable<ItemDescription> Items { get; }
    public IEnumerable<HeroDescription> Heroes { get; }
}