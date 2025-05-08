using System.Runtime.InteropServices;
using Avalonia.Controls;
using Avalonia.Controls.Notifications;
using Avalonia.Interactivity;
using D2Oracle.Core.ViewModels;
using SukiUI.Controls;

namespace D2Oracle.Avalonia.Views;

public partial class MainWindow : SukiWindow
{
    // ReSharper disable once SuggestBaseTypeForParameterInConstructor
    public MainWindow(MainWindowViewModel viewModel)
    {
        InitializeComponent();
        DataContext = viewModel;
    }

    public WindowNotificationManager? WindowNotificationManager { get; private set; }

    private void Window_OnLoaded(object? sender, RoutedEventArgs e)
    {
        WindowNotificationManager = new WindowNotificationManager(this)
        {
            Position = NotificationPosition.TopRight,
            MaxItems = 1
        };

        if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
        {
            SystemDecorations = SystemDecorations.Full;
        }
    }
}