namespace D2Oracle.Core.Services.Roshan;

public class MockRoshanTimerService : IRoshanTimerService
{
    public event EventHandler? MinRoshanRespawnTimeReached;

    public event EventHandler? MaxRoshanRespawnTimeReached;
    
    public event EventHandler? RoshanKilled;

    public TimeSpan? MinRoshanRespawnClockTime => TimeSpan.FromMinutes(8);

    public TimeSpan? MaxRoshanRespawnClockTime => TimeSpan.FromMinutes(11);
    
    public bool IsRoshanAlive { get; }
}