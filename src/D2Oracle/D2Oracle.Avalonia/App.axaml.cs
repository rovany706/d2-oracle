using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using D2Oracle.Core.Configuration;
using D2Oracle.Avalonia.Views;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

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
        DISource.Resolver = type => AppHost.Services.GetRequiredService(type);
        await AppHost.StartAsync();
        
        SukiUI.ColorTheme.LoadDarkTheme(Current);
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
            services.ConfigureAppServices(context.Configuration);
            services.AddAppServices();
            services.AddAppViewModels();
        });
        
        return builder.Build();
    }
}