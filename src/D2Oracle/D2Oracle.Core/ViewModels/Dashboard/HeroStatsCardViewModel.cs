﻿using System.Diagnostics;
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
    private readonly INetWorthCalculator netWorthCalculator;

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

        this.netWorth = Observable
            .Never<uint>()
            .StartWith((uint)555)
            .ToProperty(this, x => x.NetWorth);
    }

    public HeroStatsCardViewModel(IDotaGsiService dotaGsiService, IDotaKnowledgeService dotaKnowledgeService,
        INetWorthCalculator netWorthCalculator)
    {
        this.dotaKnowledgeService = dotaKnowledgeService;
        this.netWorthCalculator = netWorthCalculator;

        this.heroName = dotaGsiService.GameStateObservable
            .Select(GetHeroName)
            .ToProperty(this, x => x.HeroName);

        this.gpm = dotaGsiService.GameStateObservable
            .Select(x => x?.Player?.Gpm ?? 0)
            .ToProperty(this, x => x.Gpm);

        this.xpm = dotaGsiService.GameStateObservable
            .Select(x => x?.Player?.Xpm ?? 0)
            .ToProperty(this, x => x.Xpm);

        this.netWorth = dotaGsiService.GameStateObservable
            .Select(this.netWorthCalculator.Calculate)
            .ToProperty(this, x => x.NetWorth);
    }
    private string GetHeroName(GameState? gameState)
    {
        return this.dotaKnowledgeService.Heroes.SingleOrDefault(hero => hero.Name.Equals(gameState?.Hero?.Name))?.LocalizedName
               ?? string.Empty;
    }

    private readonly ObservableAsPropertyHelper<string> heroName;

    public string HeroName => heroName.Value;

    private readonly ObservableAsPropertyHelper<uint> gpm;

    public uint Gpm => gpm.Value;

    private readonly ObservableAsPropertyHelper<uint> xpm;

    public uint Xpm => xpm.Value;

    private readonly ObservableAsPropertyHelper<uint> netWorth;

    public uint NetWorth => netWorth.Value;

    public void GoToDotabuff()
    {
        var dotaBuffHeroName = HeroName.ToLower().Replace(' ', '-');
        var url = string.Format(DotabuffHeroUrlTemplate, dotaBuffHeroName);
        
        GoToUrl(url);
    }

    public void GoToDota2ProTracker()
    {
        var url = string.Format(Dota2ProTrackerHeroUrlTemplate, HeroName);
        
        GoToUrl(url);
    }

    private static void GoToUrl(string url)
    {
        Process.Start(new ProcessStartInfo(url) { UseShellExecute = true });
    }
}