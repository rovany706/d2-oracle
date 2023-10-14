namespace D2Oracle.Core.Services.Timers.Runes;

public class MockWisdomRuneTimerService : IWisdomRuneTimerService
{
    public int WisdomRuneSpawnTimeMultiplierInMinutes => 7;
    
    public event EventHandler? WisdomRuneSpawnsSoon;
}