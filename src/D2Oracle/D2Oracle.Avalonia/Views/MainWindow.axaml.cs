using Avalonia.Controls.Notifications;
using Avalonia.Interactivity;
using D2Oracle.Core.ViewModels;
using SukiUI.Controls;

namespace D2Oracle.Avalonia.Views;

public partial class MainWindow : SukiWindow
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