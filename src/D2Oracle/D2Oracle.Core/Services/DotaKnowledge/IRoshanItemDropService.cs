namespace D2Oracle.Core.Services.DotaKnowledge;

public interface IRoshanItemDropService
{
    IObservable<IEnumerable<string>> Items { get; }

    IEnumerable<string> GetCurrentItems();
}