using Dota2GSI;

namespace D2Oracle.Core.GSI;

public class GSIListener : IDisposable
{
    public GameStateListener GameStateListener { get; } = new(3000);

    public void Dispose()
    {
        GameStateListener.Dispose();
    }
}