using System.Reactive.Linq;
using D2Oracle.Core.Extensions;
using D2Oracle.Core.Services;
using D2Oracle.Core.Services.Audio;
using D2Oracle.Core.Services.DotaKnowledge;
using D2Oracle.Core.Services.Timers.Roshan;
using Dota2GSI;
using ReactiveUI;

namespace D2Oracle.Core.ViewModels.Dashboard.Timings;

public class RoshanTimingsViewModel : ViewModelBase
{
    private readonly IRoshanTimerService roshanTimerService;
    private readonly IRoshanDeathObserverService roshanDeathObserverService;
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
        IRoshanDeathObserverService roshanDeathObserverService, IDotaAudioService audioService)
    {
        this.roshanTimerService = roshanTimerService;
        this.roshanDeathObserverService = roshanDeathObserverService;
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
            var minTime = this.roshanTimerService.MinRoshanRespawnClockTime.FormatAsDotaTime();
            var maxTime = this.roshanTimerService.MaxRoshanRespawnClockTime.FormatAsDotaTime();

            return $"{minTime} - {maxTime}";
        }
    }

    public bool IsRoshanEstimatedTimeVisible => !this.roshanDeathObserverService.IsRoshanAlive;

    private readonly ObservableAsPropertyHelper<int> roshanRespawnEstimatedPercent;

    public int RoshanRespawnEstimatedPercent => this.roshanRespawnEstimatedPercent.Value;

    public int RoshanDeathCount => this.roshanDeathObserverService.RoshanDeathCount + 1;

    private void Subscribe()
    {
        this.roshanTimerService.MinRoshanRespawnTimeReached += OnMinRoshanRespawnTimeReached;
        this.roshanTimerService.MaxRoshanRespawnTimeReached += OnMaxRoshanRespawnTimeReached;
        this.roshanDeathObserverService.IsRoshanAliveChanged += OnIsRoshanAliveChanged;
    }

    private int CalculateRoshanEstimatedRespawnTimePercent(GameState? gameState)
    {
        if (IsRoshanEstimatedTimeVisible == false)
        {
            return 100;
        }
        
        var clockTimeDeltaRelativeToDeath =
            gameState?.Map?.ClockTime - this.roshanDeathObserverService.RoshanLastDeathClockTime?.TotalSeconds;
        var respawnTimeRelativeToDeath = this.roshanTimerService.MaxRoshanRespawnClockTime?.TotalSeconds -
                                         this.roshanDeathObserverService.RoshanLastDeathClockTime?.TotalSeconds;

        return (int?)(clockTimeDeltaRelativeToDeath / respawnTimeRelativeToDeath * 100) ?? 100;
    }

    private void UpdateTimerUI()
    {
        this.RaisePropertyChanged(nameof(EstimatedRoshanRespawnTime));
        this.RaisePropertyChanged(nameof(IsRoshanEstimatedTimeVisible));
        this.RaisePropertyChanged(nameof(RoshanDeathCount));
    }

    private void OnIsRoshanAliveChanged(object? sender, EventArgs e)
    {
        UpdateTimerUI();
    }
    
    private async void OnMaxRoshanRespawnTimeReached(object? sender, EventArgs e)
    {
        await this.audioService.PlaySoundAsync(DotaSoundType.MaxRoshanTime);
        this.RaisePropertyChanged(nameof(IsRoshanEstimatedTimeVisible));
    }

    private async void OnMinRoshanRespawnTimeReached(object? sender, EventArgs e)
    {
        await this.audioService.PlaySoundAsync(DotaSoundType.MinRoshanTime);
    }
}