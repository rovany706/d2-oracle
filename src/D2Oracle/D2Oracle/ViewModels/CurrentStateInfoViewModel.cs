using System;
using System.Reactive.Linq;
using D2Oracle.Services;
using ReactiveUI;

namespace D2Oracle.ViewModels;

public class CurrentStateInfoViewModel : ViewModelBase
{
    public CurrentStateInfoViewModel(IDotaGSIService dotaGsiService)
    {
        _time = dotaGsiService.GameStateObservable
            .Select(x => TimeSpan.FromSeconds(x.EventArgs.Map.ClockTime).ToString())
            .ToProperty(this, x => x.Time);
    }
    
    private readonly ObservableAsPropertyHelper<string> _time;
    public string Time => _time.Value;
}