using System;
using System.Reactive;
using Dota2GSI;

namespace D2Oracle.Services;

public interface IDotaGSIService
{
    public IObservable<GameState> GameStateObservable { get; }
}