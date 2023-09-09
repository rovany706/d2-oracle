using System;
using System.Reactive;
using System.Reactive.Linq;
using Dota2GSI;
using Microsoft.Extensions.Configuration;

namespace D2Oracle.Services;

public class DotaGSIService : IDotaGSIService
{
    private const int DefaultPort = 3000;
    public DotaGSIService(IConfiguration configuration)
    {
        var port = GetPortFromConfiguration(configuration);
        GameStateListener = new GameStateListener(port);
        GameStateObservable = Observable.FromEventPattern<GameState>(GameStateListener, nameof(GameStateListener.NewGameState));
        GameStateListener.Start();
    }

    private static int GetPortFromConfiguration(IConfiguration configuration)
    {
        var value = configuration["Port"];
        var isSuccess = int.TryParse(value, out var port);

        return isSuccess ? port : DefaultPort;
    }

    private GameStateListener GameStateListener { get; }
    public IObservable<EventPattern<GameState>> GameStateObservable { get; }
}