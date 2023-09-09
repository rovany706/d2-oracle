using System;
using System.Reactive;
using Dota2GSI;

namespace D2Oracle.Services;

public interface IDotaGSIService
{
    public IObservable<EventPattern<GameState>> GameStateObservable { get; }
}