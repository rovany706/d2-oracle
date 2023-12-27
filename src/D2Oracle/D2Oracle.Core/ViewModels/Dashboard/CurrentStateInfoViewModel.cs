using System.Reactive.Linq;
using D2Oracle.Core.Services;
using D2Oracle.Core.ViewModels.Dashboard.Timings;
using ReactiveUI;

namespace D2Oracle.Core.ViewModels.Dashboard;

public class CurrentStateInfoViewModel : ViewModelBase
{
    private bool isConnected;

    /// <summary>
    /// Constructor for designer
    /// </summary>
    public CurrentStateInfoViewModel()
    {
        TimingsCardViewModel = new TimingsCardViewModel();
        HeroStatsCardViewModel = new HeroStatsCardViewModel();
    }

    public CurrentStateInfoViewModel(IDotaGsiService dotaGsiService, TimingsCardViewModel timingsCardViewModel,
        HeroStatsCardViewModel heroStatsCardViewModel, HeroDiagramsCardViewModel heroDiagramsCardViewModel)
    {
        TimingsCardViewModel = timingsCardViewModel;
        HeroStatsCardViewModel = heroStatsCardViewModel;
        HeroDiagramsCardViewModel = heroDiagramsCardViewModel;
        
        dotaGsiService.GameStateObservable
            .Select(_ => IsConnected = true)
            .Throttle(TimeSpan.FromSeconds(10))
            .Subscribe(_ => IsConnected = false);
    }
    
    public bool IsConnected
    {
        get => this.isConnected;
        set => this.RaiseAndSetIfChanged(ref this.isConnected, value);
    }

    public TimingsCardViewModel TimingsCardViewModel { get; }
    
    public HeroStatsCardViewModel HeroStatsCardViewModel { get; }
    
    public HeroDiagramsCardViewModel HeroDiagramsCardViewModel { get; }
}