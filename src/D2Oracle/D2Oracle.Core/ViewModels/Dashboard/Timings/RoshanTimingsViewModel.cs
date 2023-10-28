using System.Reactive.Linq;
using D2Oracle.Core.Extensions;
using D2Oracle.Core.Services;
using D2Oracle.Core.Services.Audio;
using D2Oracle.Core.Services.Timers.Roshan;
using Dota2GSI;
using ReactiveUI;

namespace D2Oracle.Core.ViewModels.Dashboard.Timings;

public class RoshanTimingsViewModel : ViewModelBase
{
    private readonly IRoshanTimerService roshanTimerService;
    private readonly IDotaAudioService audioService;

    public RoshanTimingsViewModel()
    {
        this.roshanRespawnEstimatedPercent = Observable
            .Never<int>()
            .StartWith(50)
            .ToProperty(this, x => x.RoshanRespawnEstimatedPercent);
        
        this.roshanTimerService = new MockRoshanTimerService();
        this.audioService = new MockDotaAudioService();
    }
    
    public RoshanTimingsViewModel(IDotaGsiService dotaGsiService, IRoshanTimerService roshanTimerService,
        IDotaAudioService audioService)
    {
        this.roshanTimerService = roshanTimerService;
        this.audioService = audioService;

        this.roshanRespawnEstimatedPercent = dotaGsiService.GameStateObservable
            .Select(CalculateRoshanEstimatedRespawnTimePercent)
            .ToProperty(this, x => x.RoshanRespawnEstimatedPercent);
        
        Subscribe();
    }

    public string EstimatedRoshanRespawnTime
    {
        get
        {
            var minTime = roshanTimerService.MinRoshanRespawnClockTime.FormatAsDotaTime();
            var maxTime = roshanTimerService.MaxRoshanRespawnClockTime.FormatAsDotaTime();

            return $"{minTime} - {maxTime}";
        }
    }

    public bool IsRoshanEstimatedTimeVisible => !roshanTimerService.IsRoshanAlive;

    private readonly ObservableAsPropertyHelper<int> roshanRespawnEstimatedPercent;

    public int RoshanRespawnEstimatedPercent => roshanRespawnEstimatedPercent.Value;
    
    private void Subscribe()
    {
        this.roshanTimerService.MinRoshanRespawnTimeReached += OnMinRoshanRespawnTimeReached;
        this.roshanTimerService.MaxRoshanRespawnTimeReached += OnMaxRoshanRespawnTimeReached;
        this.roshanTimerService.RoshanLastDeathClockTimeChanged += OnRoshanLastDeathClockTimeChanged;
    }

    private int CalculateRoshanEstimatedRespawnTimePercent(GameState? gameState)
    {
        var clockTimeDeltaRelativeToDeath =
            gameState?.Map?.ClockTime - this.roshanTimerService.RoshanLastDeathClockTime?.TotalSeconds;
        var respawnTimeRelativeToDeath = this.roshanTimerService.MaxRoshanRespawnClockTime?.TotalSeconds -
                                         roshanTimerService.RoshanLastDeathClockTime?.TotalSeconds;

        return (int?)(clockTimeDeltaRelativeToDeath / respawnTimeRelativeToDeath * 100) ?? 100;
    }

    private void OnRoshanLastDeathClockTimeChanged(object? sender, EventArgs e)
    {
        this.RaisePropertyChanged(nameof(EstimatedRoshanRespawnTime));
        this.RaisePropertyChanged(nameof(IsRoshanEstimatedTimeVisible));
    }

    private async void OnMaxRoshanRespawnTimeReached(object? sender, EventArgs e)
    {
        await audioService.PlaySoundAsync(DotaSoundType.MaxRoshanTime);
        this.RaisePropertyChanged(nameof(IsRoshanEstimatedTimeVisible));
    }

    private async void OnMinRoshanRespawnTimeReached(object? sender, EventArgs e)
    {
        await audioService.PlaySoundAsync(DotaSoundType.MinRoshanTime);
    }
}