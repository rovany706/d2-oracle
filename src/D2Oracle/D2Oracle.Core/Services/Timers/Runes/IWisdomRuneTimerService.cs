namespace D2Oracle.Core.Services.Timers.Runes;

public interface IWisdomRuneTimerService
{
    public int WisdomRuneSpawnTimeMultiplierInMinutes { get; }

    event EventHandler WisdomRuneSpawnsSoon;
}