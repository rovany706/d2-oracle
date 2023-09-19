using D2Oracle.ViewModels.Dashboard;

namespace D2Oracle.ViewModels;

public class MainWindowViewModel : ViewModelBase
{
    public MainWindowViewModel(DashboardPageViewModel dashboardPageViewModel, SettingsViewModel settingsViewModel)
    {
        DashboardPageViewModel = dashboardPageViewModel;
        SettingsViewModel = settingsViewModel;
    }

    public DashboardPageViewModel DashboardPageViewModel { get; }
    public SettingsViewModel SettingsViewModel { get; }
}