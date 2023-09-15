using System;
using System.Reactive.Linq;
using D2Oracle.Services;
using D2Oracle.Services.Roshan;
using NetCoreAudio;
using ReactiveUI;

namespace D2Oracle.ViewModels;

public class CurrentStateInfoViewModel : ViewModelBase
{
    private readonly IRoshanTimerService roshanTimerService;

    public CurrentStateInfoViewModel()
    {
        this._time = Observable
            .Never<string>()
            .StartWith("34:30")
            .ToProperty(this, x => x.Time);
        this.roshanTimerService = new MockRoshanTimerService();
    }

    public CurrentStateInfoViewModel(IDotaGSIService dotaGsiService, IRoshanTimerService roshanTimerService)
    {
        this.roshanTimerService = roshanTimerService;
        this._time = dotaGsiService.GameStateObservable
            .Select(x => TimeSpan.FromSeconds(x.Map?.ClockTime ?? -1).ToString())
            .ToProperty(this, x => x.Time);
        
        roshanTimerService.MinRoshanRespawnTimeReached += RoshanTimerServiceOnMinRoshanRespawnTimeReached;
    }

    private void RoshanTimerServiceOnMinRoshanRespawnTimeReached(object? sender, EventArgs e)
    {
        
    }

    // ReSharper disable once MemberCanBeMadeStatic.Global
    public async void PlayMinRoshanTimeSound()
    {
        var player = new Player();
        await player.Play("Resources/8min.wav");
    }

    private readonly ObservableAsPropertyHelper<string> _time;

    public string Time => _time.Value;
}