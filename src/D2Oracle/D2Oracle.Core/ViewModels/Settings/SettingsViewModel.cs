namespace D2Oracle.Core.ViewModels.Settings;

public class SettingsViewModel : ViewModelBase
{
    public SettingsViewModel(DotaConnectionSettingsViewModel dotaConnectionSettingsViewModel)
    {
        DotaConnectionSettingsViewModel = dotaConnectionSettingsViewModel;
    }
    
    public DotaConnectionSettingsViewModel DotaConnectionSettingsViewModel { get; }
}