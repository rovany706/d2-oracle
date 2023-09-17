using System;
using System.Reactive.Linq;
using D2Oracle.Services;
using ReactiveUI;

namespace D2Oracle.ViewModels;

public class MainWindowViewModel : ViewModelBase
{
    private bool isConnected;
    
    public MainWindowViewModel(IDotaGsiService dotaGsiService, CurrentStateInfoViewModel currentStateInfoViewModel)
    {
        CurrentStateInfoViewModel = currentStateInfoViewModel;
        dotaGsiService.GameStateObservable
            .Select(_ => IsConnected = true)
            .Throttle(TimeSpan.FromSeconds(5))
            .Subscribe(_ => IsConnected = false);
    }

    public CurrentStateInfoViewModel CurrentStateInfoViewModel { get; }

    public bool IsConnected
    {
        get => isConnected;
        set => this.RaiseAndSetIfChanged(ref isConnected, value);
    }
}