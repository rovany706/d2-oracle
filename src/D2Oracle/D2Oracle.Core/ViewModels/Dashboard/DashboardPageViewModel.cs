using D2Oracle.Core.Services;
using ReactiveUI;

namespace D2Oracle.Core.ViewModels.Dashboard;

public class DashboardPageViewModel : ViewModelBase
{
    private bool isDotaRunning;

    public DashboardPageViewModel(IDotaProcessLocator dotaProcessLocator, CurrentStateInfoViewModel currentStateInfoViewModel)
    {
        CurrentStateInfoViewModel = currentStateInfoViewModel;

        dotaProcessLocator.IsDotaProcessRunningObservable.Subscribe(isRunning => IsDotaRunning = isRunning);
    }
    
    public CurrentStateInfoViewModel CurrentStateInfoViewModel { get; }

    public bool IsDotaRunning
    {
        get => this.isDotaRunning;
        set => this.RaiseAndSetIfChanged(ref this.isDotaRunning, value);
    }
}