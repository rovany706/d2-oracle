using System.Collections.ObjectModel;
using D2Oracle.Core.Extensions;
using D2Oracle.Core.Services;
using D2Oracle.Core.Services.NetWorth;
using Dota2GSI;
using Dota2GSI.Nodes;
using LiveChartsCore;
using LiveChartsCore.Kernel;
using LiveChartsCore.SkiaSharpView;

namespace D2Oracle.Core.ViewModels.Dashboard;

public class HeroDiagramsCardViewModel : ViewModelBase
{
    private readonly INetWorthCalculator netWorthCalculator;

    private record NetWorthSample(int ClockTime, uint NetWorth);

    private readonly ObservableCollection<NetWorthSample> netWorthValuesPerSecond = new();

    public HeroDiagramsCardViewModel(IDotaGsiService dotaGsiService, INetWorthCalculator netWorthCalculator)
    {
        this.netWorthCalculator = netWorthCalculator;
        dotaGsiService.GameStateObservable.Subscribe(OnNewGameState);

        NetWorthSeries = new ObservableCollection<ISeries>
        {
            new LineSeries<NetWorthSample>
            {
                Values = this.netWorthValuesPerSecond,
                Mapping = (sample, chartPoint) =>
                    chartPoint.Coordinate = new Coordinate(sample.ClockTime, sample.NetWorth),
                LineSmoothness = 1,
                GeometrySize = 0
            }
        };

        NetWorthXAxes = new[]
            { new Axis { Labeler = value => TimeSpan.FromSeconds(value).FormatAsDotaTime() } };

        NetWorthYAxes = new[] { new Axis { Name = Resources.Resources.NetWorth } };
    }

    private void OnNewGameState(GameState? gameState)
    {
        // Reset graph if not in game
        if (!gameState.IsInGame())
        {
            ClearGraph();

            return;
        }

        // Do not count net worth before game start
        if (gameState?.Map?.GameState != DotaGameState.DOTA_GAMERULES_STATE_GAME_IN_PROGRESS)
        {
            return;
        }

        this.netWorthValuesPerSecond.Add(new NetWorthSample(gameState.Map.ClockTime,
            this.netWorthCalculator.Calculate(gameState)));
    }

    private void ClearGraph()
    {
        if (this.netWorthValuesPerSecond.Any())
        {
            this.netWorthValuesPerSecond.Clear();
        }
    }

    public ObservableCollection<ISeries> NetWorthSeries { get; }

    public Axis[] NetWorthXAxes { get; }

    public Axis[] NetWorthYAxes { get; }
}