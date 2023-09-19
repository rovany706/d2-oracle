using System;
using System.Reactive.Linq;
using D2Oracle.Extensions;
using D2Oracle.Services;
using D2Oracle.Services.Audio;
using D2Oracle.Services.Roshan;
using Dota2GSI;
using ReactiveUI;

namespace D2Oracle.ViewModels.Dashboard;

public class TimingsCardViewModel : ViewModelBase
{
    private readonly IRoshanTimerService roshanTimerService;
    private readonly IDotaAudioService audioService;
    
    /// <summary>
    /// Constructor for designer
    /// </summary>
    public TimingsCardViewModel()
    {
        this.time = Observable
            .Never<string>()
            .StartWith("34:30")
            .ToProperty(this, x => x.Time);
        
        this.roshanRespawnEstimatedPercent = Observable
            .Never<int>()
            .StartWith(50)
            .ToProperty(this, x => x.RoshanRespawnEstimatedPercent);
        
        this.roshanTimerService = new MockRoshanTimerService();
        this.audioService = new MockDotaAudioService();
    }
    
    public TimingsCardViewModel(IDotaGsiService dotaGsiService, IRoshanTimerService roshanTimerService, IDotaAudioService audioService)
    {
        this.roshanTimerService = roshanTimerService;
        this.audioService = audioService;

        this.time = dotaGsiService.GameStateObservable
            .Select(GetTimeFromState)
            .ToProperty(this, x => x.Time);
        
        this.roshanRespawnEstimatedPercent = dotaGsiService.GameStateObservable
            .Select(CalculateRoshanEstimatedRespawnTimePercent)
            .ToProperty(this, x => x.RoshanRespawnEstimatedPercent);
        
        Subscribe();
    }
    
    private void Subscribe()
    {
        this.roshanTimerService.MinRoshanRespawnTimeReached += OnMinRoshanRespawnTimeReached;
        this.roshanTimerService.MaxRoshanRespawnTimeReached += OnMaxRoshanRespawnTimeReached;
        this.roshanTimerService.RoshanKilled += OnRoshanKilled;
    }
    
    private void OnRoshanKilled(object? sender, EventArgs e)
    {
        this.RaisePropertyChanged(nameof(EstimatedRoshanRespawnTime));
        this.RaisePropertyChanged(nameof(IsRoshanEstimatedTimeVisible));
    }

    private void OnMaxRoshanRespawnTimeReached(object? sender, EventArgs e)
    {
        audioService.PlaySound(DotaSoundType.MaxRoshanTime);
        this.RaisePropertyChanged(nameof(IsRoshanEstimatedTimeVisible));
    }

    private void OnMinRoshanRespawnTimeReached(object? sender, EventArgs e)
    {
        audioService.PlaySound(DotaSoundType.MinRoshanTime);
    }

    private static string GetTimeFromState(GameState? gameState)
    {
        return TimeSpan.FromSeconds(gameState?.Map?.ClockTime ?? 0).FormatAsDotaTime();
    }

    private int CalculateRoshanEstimatedRespawnTimePercent(GameState? gameState)
    {
        return gameState?.Map?.ClockTime / this.roshanTimerService.MaxRoshanRespawnClockTime?.Seconds ?? 100;
    }

    private readonly ObservableAsPropertyHelper<string> time;

    public string Time => time.Value;

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
}