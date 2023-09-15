using System;
using System.Reactive.Linq;
using D2Oracle.Configuration;
using Dota2GSI;
using Microsoft.Extensions.Options;

namespace D2Oracle.Services;

public class DotaGSIService : IDotaGSIService, IDisposable
{
    public DotaGSIService(IOptions<DotaConnectionOptions> options)
    {
        var port = options.Value.Port;
        GameStateListener = new GameStateListener(port);
        GameStateObservable = Observable
            .FromEventPattern<GameState>(GameStateListener, nameof(GameStateListener.NewGameState))
            .Select(x => x.EventArgs);
        GameStateListener.Start();
    }

    private GameStateListener GameStateListener { get; }
    public IObservable<GameState> GameStateObservable { get; }

    public void Dispose()
    {
        GameStateListener.Dispose();
        GC.SuppressFinalize(this);
    }
}