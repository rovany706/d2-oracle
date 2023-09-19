using System;
using System.Collections.ObjectModel;
using System.Linq;
using D2Oracle.Extensions;
using D2Oracle.Services;
using Dota2GSI;
using LiveChartsCore;
using LiveChartsCore.Kernel;
using LiveChartsCore.SkiaSharpView;

namespace D2Oracle.ViewModels.Dashboard;

public class HeroDiagramsCardViewModel : ViewModelBase
{
    private readonly NetWorthCalculator netWorthCalculator;

    private record NetWorthSample(int ClockTime, uint NetWorth);

    private readonly ObservableCollection<NetWorthSample> netWorthValuesPerSecond = new();

    public HeroDiagramsCardViewModel(IDotaGsiService dotaGsiService, NetWorthCalculator netWorthCalculator)
    {
        this.netWorthCalculator = netWorthCalculator;
        dotaGsiService.GameStateObservable.Subscribe(OnNewGameState);

        NetWorthSeries = new ObservableCollection<ISeries>
        {
            new LineSeries<NetWorthSample>
            {
                Values = netWorthValuesPerSecond,
                Mapping = (sample, chartPoint) =>
                    chartPoint.Coordinate = new Coordinate(sample.ClockTime, sample.NetWorth),
                LineSmoothness = 1,
                GeometrySize = 0
            }
        };

        NetWorthXAxes = new[]
            { new Axis { Labeler = value => TimeSpan.FromSeconds(value).FormatAsDotaTime() } };
        
        NetWorthYAxes = new[] { new Axis { Name = Resources.NetWorth } };
    }

    private void OnNewGameState(GameState? gameState)
    {
        // Reset graph if not in game
        if (!gameState.IsInGame() && netWorthValuesPerSecond.Any())
        {
            netWorthValuesPerSecond.Clear();
        }

        netWorthValuesPerSecond.Add(new NetWorthSample(gameState?.Map?.ClockTime ?? 0,
            netWorthCalculator.Calculate(gameState)));
    }

    public ObservableCollection<ISeries> NetWorthSeries { get; }

    public Axis[] NetWorthXAxes { get; }

    public Axis[] NetWorthYAxes { get; }
}