namespace D2Oracle.Core.Services.Timers.Roshan;

public class MockRoshanTimerService : IRoshanTimerService
{
    public event EventHandler? MinRoshanRespawnTimeReached;

    public event EventHandler? MaxRoshanRespawnTimeReached;
    
    public TimeSpan? MinRoshanRespawnClockTime => TimeSpan.FromMinutes(8);

    public TimeSpan? MaxRoshanRespawnClockTime => TimeSpan.FromMinutes(11);

    public event EventHandler? CurrentMatchIdChanged;
}