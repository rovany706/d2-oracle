namespace D2Oracle.Core.Services.DotaKnowledge;

public interface IRoshanItemDropService
{
    IObservable<IEnumerable<string>> CurrentItems { get; }
    
    IObservable<IEnumerable<string>> LastItems { get; }
}