using System;
using System.Reactive.Linq;
using D2Oracle.Services;
using D2Oracle.Services.Audio;
using D2Oracle.Services.Roshan;
using ReactiveUI;

namespace D2Oracle.ViewModels;

public class CurrentStateInfoViewModel : ViewModelBase
{
    private readonly IRoshanTimerService roshanTimerService;
    private readonly IDotaAudioService audioService;

    public CurrentStateInfoViewModel()
    {
        this._time = Observable
            .Never<string>()
            .StartWith("34:30")
            .ToProperty(this, x => x.Time);
        this.roshanTimerService = new MockRoshanTimerService();
    }

    public CurrentStateInfoViewModel(IDotaGSIService dotaGsiService, IRoshanTimerService roshanTimerService, IDotaAudioService audioService)
    {
        this.roshanTimerService = roshanTimerService;
        this.audioService = audioService;
        this._time = dotaGsiService.GameStateObservable
            .Select(x => TimeSpan.FromSeconds(x.Map?.ClockTime ?? -1).ToString())
            .ToProperty(this, x => x.Time);
        
        roshanTimerService.MinRoshanRespawnTimeReached += RoshanTimerServiceOnMinRoshanRespawnTimeReached;
        this.roshanTimerService.MaxRoshanRespawnTimeReached += RoshanTimerServiceOnMaxRoshanRespawnTimeReached;
    }

    private void RoshanTimerServiceOnMaxRoshanRespawnTimeReached(object? sender, EventArgs e)
    {
        audioService.PlaySound(DotaSoundType.MaxRoshanTime);
    }

    private void RoshanTimerServiceOnMinRoshanRespawnTimeReached(object? sender, EventArgs e)
    {
        audioService.PlaySound(DotaSoundType.MinRoshanTime);
    }
    
    private readonly ObservableAsPropertyHelper<string> _time;

    public string Time => _time.Value;
}