using System.Reactive.Linq;
using D2Oracle.Core.Configuration;
using Dota2GSI;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace D2Oracle.Core.Services;

public class DotaGsiService : IDotaGsiService, IDisposable
{
    public DotaGsiService(IOptions<DotaConnectionOptions> options, ILoggerFactory loggerFactory)
    {
        var port = options.Value.Port;
        GameStateListener = new GameStateListener(port, loggerFactory);
        GameStateObservable = Observable
            .FromEventPattern<GameState?>(GameStateListener, nameof(GameStateListener.NewGameState))
            .Select(x => x.EventArgs);
        GameStateListener.Start();
    }

    private GameStateListener GameStateListener { get; }
    public IObservable<GameState?> GameStateObservable { get; }
    
    public void Dispose()
    {
        GameStateListener.Dispose();
        GC.SuppressFinalize(this);
    }
}