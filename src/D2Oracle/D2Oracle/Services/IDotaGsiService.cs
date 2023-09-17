using System;
using Dota2GSI;

namespace D2Oracle.Services;

public interface IDotaGsiService
{
    public IObservable<GameState?> GameStateObservable { get; }
}