using D2Oracle.Core.ViewModels.Dashboard.Timings;

namespace D2Oracle.Core.ViewModels.Dashboard;

public class CurrentStateInfoViewModel : ViewModelBase
{
    /// <summary>
    /// Constructor for designer
    /// </summary>
    public CurrentStateInfoViewModel()
    {
        TimingsCardViewModel = new TimingsCardViewModel();
        HeroStatsCardViewModel = new HeroStatsCardViewModel();
    }

    public CurrentStateInfoViewModel(TimingsCardViewModel timingsCardViewModel,
        HeroStatsCardViewModel heroStatsCardViewModel, HeroDiagramsCardViewModel heroDiagramsCardViewModel)
    {
        TimingsCardViewModel = timingsCardViewModel;
        HeroStatsCardViewModel = heroStatsCardViewModel;
        HeroDiagramsCardViewModel = heroDiagramsCardViewModel;
    }

    public TimingsCardViewModel TimingsCardViewModel { get; }
    
    public HeroStatsCardViewModel HeroStatsCardViewModel { get; }
    
    public HeroDiagramsCardViewModel HeroDiagramsCardViewModel { get; }
}