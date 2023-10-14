using System.Reactive.Linq;
using D2Oracle.Core.Extensions;
using D2Oracle.Core.Services;
using D2Oracle.Core.Services.Audio;
using D2Oracle.Core.Services.Timers.Runes;
using Dota2GSI;
using Dota2GSI.Nodes;
using ReactiveUI;

namespace D2Oracle.Core.ViewModels.Dashboard.Timings;

public class WisdomRuneTimingsViewModel : ViewModelBase
{
    private readonly IWisdomRuneTimerService wisdomRuneTimerService;
    private readonly IDotaAudioService audioService;

    public WisdomRuneTimingsViewModel()
    {
        this.wisdomRuneTimerService = new MockWisdomRuneTimerService();
        this.audioService = new MockDotaAudioService();
        this.nextWisdomRuneTime = Observable
            .Never<string>()
            .StartWith("7:00")
            .ToProperty(this, x => x.NextWisdomRuneTime);
        this.nextWisdomRunePercent = Observable
            .Never<int>()
            .StartWith(75)
            .ToProperty(this, x => x.NextWisdomRunePercent);
    }

    public WisdomRuneTimingsViewModel(IDotaGsiService dotaGsiService, IWisdomRuneTimerService wisdomRuneTimerService,
        IDotaAudioService audioService)
    {
        this.wisdomRuneTimerService = wisdomRuneTimerService;
        this.audioService = audioService;

        this.nextWisdomRunePercent = dotaGsiService.GameStateObservable
            .Select(CalculateRoshanEstimatedRespawnTimePercent)
            .ToProperty(this, x => x.NextWisdomRunePercent);

        this.nextWisdomRuneTime = dotaGsiService.GameStateObservable
            .Select(GetWisdomRuneNextSpawnTime)
            .ToProperty(this, x => x.NextWisdomRuneTime);

        Subscribe();
    }

    private readonly ObservableAsPropertyHelper<int> nextWisdomRunePercent;

    public int NextWisdomRunePercent => nextWisdomRunePercent.Value;


    private readonly ObservableAsPropertyHelper<string> nextWisdomRuneTime;

    public string NextWisdomRuneTime => this.nextWisdomRuneTime.Value;

    private void Subscribe()
    {
        this.wisdomRuneTimerService.WisdomRuneSpawnsSoon += OnWisdomRuneSpawnsSoon;
    }

    private async void OnWisdomRuneSpawnsSoon(object? sender, EventArgs e)
    {
        await audioService.PlaySound(DotaSoundType.WisdomRuneSoon);
    }

    private int CalculateRoshanEstimatedRespawnTimePercent(GameState? gameState)
    {
        if (gameState?.Map is null || gameState?.Map?.GameState != DotaGameState.DOTA_GAMERULES_STATE_GAME_IN_PROGRESS)
        {
            return 100;
        }

        var clockTime = TimeSpan.FromSeconds(gameState.Map.ClockTime);
        var previousSpawnTime =
            TimeSpan.FromMinutes(
                clockTime.TotalMinutes.ClosestMultipleFloor(this.wisdomRuneTimerService
                    .WisdomRuneSpawnTimeMultiplierInMinutes));
        var clockTimeDeltaRelativeToPreviousSpawn = clockTime - previousSpawnTime;
        var nextSpawnRelativeToPreviousSpawn = TimeSpan
            .FromMinutes(this.wisdomRuneTimerService.WisdomRuneSpawnTimeMultiplierInMinutes).TotalSeconds;

        return (int?)(clockTimeDeltaRelativeToPreviousSpawn.TotalSeconds / nextSpawnRelativeToPreviousSpawn * 100) ??
               100;
    }

    private string GetWisdomRuneNextSpawnTime(GameState? gameState)
    {
        if (gameState?.Map is null)
        {
            return TimeSpan.Zero.FormatAsDotaTime();
        }

        var clockTime = TimeSpan.FromSeconds(gameState.Map.ClockTime);

        return TimeSpan.FromMinutes(
            clockTime.TotalMinutes.ClosestMultipleCeil(
                this.wisdomRuneTimerService.WisdomRuneSpawnTimeMultiplierInMinutes
            )).FormatAsDotaTime();
    }
}