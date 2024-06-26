using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using Avalonia.Styling;
using D2Oracle.Avalonia.Services;
using D2Oracle.Core.Configuration;
using D2Oracle.Avalonia.Views;
using D2Oracle.Core.Services;
using LazyProxy.ServiceProvider;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SukiUI;

namespace D2Oracle.Avalonia;

public partial class App : Application
{
    private IHost? AppHost { get; set; }

    public override void Initialize()
    {
        AvaloniaXamlLoader.Load(this);
    }

    public override async void OnFrameworkInitializationCompleted()
    {
        AppHost = CreateAppHost();
        await AppHost.StartAsync();
        
        SukiTheme.GetInstance().ChangeBaseTheme(ThemeVariant.Dark);
        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            desktop.ShutdownRequested += DesktopOnShutdownRequested;
            desktop.MainWindow = AppHost.Services.GetRequiredService<MainWindow>();
        }

        base.OnFrameworkInitializationCompleted();
    }

    private async void DesktopOnShutdownRequested(object? sender, ShutdownRequestedEventArgs e)
    {
        await AppHost!.StopAsync();
    }

    private static IHost CreateAppHost()
    {
        var builder = Host.CreateDefaultBuilder();
        
        builder.ConfigureServices((context, services) =>
        {
            services.AddSingleton<MainWindow>();
            services.AddSingleton<IDispatcherService, AvaloniaDispatcherService>();
            services.AddLazySingleton<INotificationService, AvaloniaNotificationService>();
            services.AddLazySingleton<IFilePickerService, AvaloniaFilePickerService>();
            services.ConfigureAppServices(context.Configuration);
            services.AddAppServices();
            services.AddAppViewModels();
        });
        
        return builder.Build();
    }
}