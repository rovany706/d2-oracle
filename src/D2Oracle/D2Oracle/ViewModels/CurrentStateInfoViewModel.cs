﻿using System;
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

    /// <summary>
    /// Constructor for designer
    /// </summary>
    public CurrentStateInfoViewModel()
    {
        this.time = Observable
            .Never<string>()
            .StartWith("34:30")
            .ToProperty(this, x => x.Time);
        
        this.heroName = Observable
            .Never<string>()
            .StartWith("Pudge")
            .ToProperty(this, x => x.HeroName);
        
        this.gpm = Observable
            .Never<uint>()
            .StartWith((uint) 500)
            .ToProperty(this, x => x.Gpm);
        
        this.xpm = Observable
            .Never<uint>()
            .StartWith((uint) 600)
            .ToProperty(this, x => x.Xpm);
        
        this.roshanTimerService = new MockRoshanTimerService();
        this.audioService = new MockDotaAudioService();
    }

    public CurrentStateInfoViewModel(IDotaGsiService dotaGsiService, IRoshanTimerService roshanTimerService,
        IDotaAudioService audioService)
    {
        this.roshanTimerService = roshanTimerService;
        this.audioService = audioService;
        
        InitializeProperties(dotaGsiService);
        Subscribe();
    }

    private void InitializeProperties(IDotaGsiService dotaGsiService)
    {
        this.time = dotaGsiService.GameStateObservable
            .Select(x => FormatDotaTime(TimeSpan.FromSeconds(x?.Map?.ClockTime ?? 0)))
            .ToProperty(this, x => x.Time);

        this.heroName = dotaGsiService.GameStateObservable
            .Select(x => x?.Hero?.Name ?? string.Empty)
            .ToProperty(this, x => x.HeroName);

        this.gpm = dotaGsiService.GameStateObservable
            .Select(x => x?.Player?.Gpm ?? 0)
            .ToProperty(this, x => x.Gpm);

        this.xpm = dotaGsiService.GameStateObservable
            .Select(x => x?.Player?.Xpm ?? 0)
            .ToProperty(this, x => x.Xpm);
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
        this.RaisePropertyChanged(nameof(IsRoshanEstTimeVisible));
    }

    private void OnMaxRoshanRespawnTimeReached(object? sender, EventArgs e)
    {
        audioService.PlaySound(DotaSoundType.MaxRoshanTime);
        this.RaisePropertyChanged(nameof(IsRoshanEstTimeVisible));
    }

    private void OnMinRoshanRespawnTimeReached(object? sender, EventArgs e)
    {
        audioService.PlaySound(DotaSoundType.MinRoshanTime);
    }

    private ObservableAsPropertyHelper<string> time;

    public string Time => time.Value;

    public string EstimatedRoshanRespawnTime
    {
        get
        {
            var minTime = FormatDotaTime(roshanTimerService.MinRoshanRespawnClockTime);
            var maxTime = FormatDotaTime(roshanTimerService.MaxRoshanRespawnClockTime);

            return $"{minTime} - {maxTime}";
        }
    }

    public bool IsRoshanEstTimeVisible => !roshanTimerService.IsRoshanAlive;

    private ObservableAsPropertyHelper<string> heroName;

    public string HeroName => heroName.Value;
    
    private ObservableAsPropertyHelper<uint> gpm;

    public uint Gpm => gpm.Value;

    private ObservableAsPropertyHelper<uint> xpm;

    public uint Xpm => xpm.Value;
    
    private static string FormatDotaTime(TimeSpan? time)
    {
        if (time is null) return string.Empty;

        const string dotaTimeFormat = @"mm\:ss";
        var sign = time < TimeSpan.Zero ? "-" : "";

        return $"{sign}{time.Value.ToString(dotaTimeFormat)}";
    }
}