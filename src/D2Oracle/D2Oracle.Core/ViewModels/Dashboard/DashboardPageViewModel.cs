using System.Reactive.Linq;
using D2Oracle.Core.Services;
using ReactiveUI;

namespace D2Oracle.Core.ViewModels.Dashboard;

public class DashboardPageViewModel : ViewModelBase
{
    private bool isConnected;
    
    public DashboardPageViewModel(IDotaGsiService dotaGsiService, CurrentStateInfoViewModel currentStateInfoViewModel)
    {
        CurrentStateInfoViewModel = currentStateInfoViewModel;
        
        dotaGsiService.GameStateObservable
            .Select(_ => IsConnected = true)
            .Throttle(TimeSpan.FromSeconds(10))
            .Subscribe(_ => IsConnected = false);
    }
    
    public CurrentStateInfoViewModel CurrentStateInfoViewModel { get; }

    public bool IsConnected
    {
        get => isConnected;
        set => this.RaiseAndSetIfChanged(ref isConnected, value);
    }
}