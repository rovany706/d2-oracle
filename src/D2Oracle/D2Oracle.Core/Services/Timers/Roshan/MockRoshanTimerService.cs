using Dota2GSI;

namespace D2Oracle.Core.Services.Timers.Roshan;

public class MockRoshanTimerService : IRoshanTimerService
{
    public event EventHandler? MinRoshanRespawnTimeReached;

    public event EventHandler? MaxRoshanRespawnTimeReached;
    
    public event EventHandler? RoshanKilled;

    public TimeSpan? MinRoshanRespawnClockTime => TimeSpan.FromMinutes(8);

    public TimeSpan? MaxRoshanRespawnClockTime => TimeSpan.FromMinutes(11);
    
    public TimeSpan? RoshanLastDeathClockTime => TimeSpan.FromMinutes(10);

    public bool IsRoshanAlive => false;
}