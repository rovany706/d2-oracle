using System.Diagnostics;
using System.Reactive;
using System.Reactive.Linq;
using D2Oracle.Core.Services;
using D2Oracle.Core.Services.DotaKnowledge;
using D2Oracle.Core.Services.NetWorth;
using Dota2GSI;
using ReactiveUI;

namespace D2Oracle.Core.ViewModels.Dashboard;

public class HeroStatsCardViewModel : ViewModelBase
{
    private const string DotabuffHeroUrlTemplate = "https://www.dotabuff.com/heroes/{0}";
    private const string Dota2ProTrackerHeroUrlTemplate = "https://dota2protracker.com/hero/{0}";

    private readonly IDotaKnowledgeService dotaKnowledgeService;

    /// <summary>
    /// Constructor for designer
    /// </summary>
    public HeroStatsCardViewModel()
    {
        this.heroName = Observable
            .Never<string>()
            .StartWith("Pudge")
            .ToProperty(this, x => x.HeroName);

        this.gpm = Observable
            .Never<uint>()
            .StartWith((uint)500)
            .ToProperty(this, x => x.Gpm);

        this.xpm = Observable
            .Never<uint>()
            .StartWith((uint)600)
            .ToProperty(this, x => x.Xpm);
    }

    public HeroStatsCardViewModel(IDotaGsiService dotaGsiService, IDotaKnowledgeService dotaKnowledgeService)
    {
        this.dotaKnowledgeService = dotaKnowledgeService;

        this.heroName = dotaGsiService.GameStateObservable
            .Select(GetHeroName)
            .ToProperty(this, x => x.HeroName);

        this.gpm = dotaGsiService.GameStateObservable
            .Select(x => x?.Player?.Gpm ?? 0)
            .ToProperty(this, x => x.Gpm);

        this.xpm = dotaGsiService.GameStateObservable
            .Select(x => x?.Player?.Xpm ?? 0)
            .ToProperty(this, x => x.Xpm);
        
        var isHeroPicked = this
            .WhenAnyValue(x => x.HeroName)
            .Select(name => !string.IsNullOrEmpty(name))
            .ObserveOn(RxApp.MainThreadScheduler);

        GoToDotabuff = ReactiveCommand.Create(OpenDotabuff, isHeroPicked);
        GoToDota2ProTracker = ReactiveCommand.Create(OpenDota2ProTracker, isHeroPicked);
    }

    private string GetHeroName(GameState? gameState)
    {
        var isSuccess =
            this.dotaKnowledgeService.Heroes.TryGetValue(gameState?.Hero?.Name ?? "", out var localizedName);

        return isSuccess ? localizedName! : string.Empty;
    }

    private readonly ObservableAsPropertyHelper<string> heroName;

    public string HeroName => this.heroName.Value;

    private readonly ObservableAsPropertyHelper<uint> gpm;

    public uint Gpm => this.gpm.Value;

    private readonly ObservableAsPropertyHelper<uint> xpm;

    public uint Xpm => this.xpm.Value;

    public ReactiveCommand<Unit, Unit> GoToDotabuff { get; }
    public ReactiveCommand<Unit, Unit> GoToDota2ProTracker { get; }

    private void OpenDotabuff()
    {
        var dotaBuffHeroName = HeroName.ToLower().Replace(' ', '-');
        var url = string.Format(DotabuffHeroUrlTemplate, dotaBuffHeroName);

        GoToUrl(url);
    }

    private void OpenDota2ProTracker()
    {
        var url = string.Format(Dota2ProTrackerHeroUrlTemplate, HeroName);

        GoToUrl(url);
    }

    private static void GoToUrl(string url)
    {
        Process.Start(new ProcessStartInfo(url) { UseShellExecute = true });
    }
}