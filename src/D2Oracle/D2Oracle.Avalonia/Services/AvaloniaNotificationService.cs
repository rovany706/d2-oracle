using Avalonia.Controls.Notifications;
using D2Oracle.Avalonia.Views;
using D2Oracle.Core.Services;

namespace D2Oracle.Avalonia.Services;

public class AvaloniaNotificationService : INotificationService
{
    private readonly MainWindow mainWindow;

    public AvaloniaNotificationService(MainWindow mainWindow)
    {
        this.mainWindow = mainWindow;
    }

    public void ShowNotification(string title, string message)
    {
        var notification = new Notification(title, message);
        this.mainWindow.WindowNotificationManager?.Show(notification);
    }
}