using Avalonia.Controls.Notifications;
using Avalonia.Interactivity;
using Avalonia.ReactiveUI;
using D2Oracle.Core.ViewModels;

namespace D2Oracle.Avalonia.Views;

public partial class MainWindow : ReactiveWindow<MainWindowViewModel>
{
    public MainWindow(MainWindowViewModel viewModel)
    {
        InitializeComponent();
        DataContext = viewModel;
    }

    public WindowNotificationManager WindowNotificationManager { get; private set; }

    private void Window_OnLoaded(object? sender, RoutedEventArgs e)
    {
        WindowNotificationManager = new WindowNotificationManager(this)
        {
            Position = NotificationPosition.TopRight,
            MaxItems = 1
        };
    }
}