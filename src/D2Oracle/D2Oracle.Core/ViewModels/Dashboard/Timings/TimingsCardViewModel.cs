using System.Reactive.Linq;
using D2Oracle.Core.Extensions;
using D2Oracle.Core.Services;
using Dota2GSI;
using ReactiveUI;

namespace D2Oracle.Core.ViewModels.Dashboard.Timings;

public class TimingsCardViewModel : ViewModelBase
{
    /// <summary>
    /// Constructor for designer
    /// </summary>
    public TimingsCardViewModel()
    {
        this.time = Observable
            .Never<string>()
            .StartWith("34:30")
            .ToProperty(this, x => x.Time);
        
        RoshanTimingsViewModel = new RoshanTimingsViewModel();
        WisdomRuneTimingsViewModel = new WisdomRuneTimingsViewModel();
    }

    public TimingsCardViewModel(IDotaGsiService dotaGsiService,
        RoshanTimingsViewModel roshanTimingsViewModel,
        WisdomRuneTimingsViewModel wisdomRuneTimingsViewModel,
        RoshanNextItemsDropViewModel roshanNextItemsDropViewModel)
    {
        RoshanTimingsViewModel = roshanTimingsViewModel;
        WisdomRuneTimingsViewModel = wisdomRuneTimingsViewModel;
        RoshanNextItemsDropViewModel = roshanNextItemsDropViewModel;

        this.time = dotaGsiService.GameStateObservable
            .Select(GetTimeFromState)
            .ToProperty(this, x => x.Time);
    }

    private static string GetTimeFromState(GameState? gameState)
    {
        return TimeSpan.FromSeconds(gameState?.Map?.ClockTime ?? 0).FormatAsDotaTime();
    }

    private readonly ObservableAsPropertyHelper<string> time;

    public string Time => this.time.Value;

    public RoshanTimingsViewModel RoshanTimingsViewModel { get; }
    public WisdomRuneTimingsViewModel WisdomRuneTimingsViewModel { get; }
    public RoshanNextItemsDropViewModel RoshanNextItemsDropViewModel { get; }
}