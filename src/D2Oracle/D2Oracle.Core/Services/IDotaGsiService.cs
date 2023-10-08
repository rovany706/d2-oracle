using Dota2GSI;

namespace D2Oracle.Core.Services;

public interface IDotaGsiService
{
    public IObservable<GameState?> GameStateObservable { get; }
}