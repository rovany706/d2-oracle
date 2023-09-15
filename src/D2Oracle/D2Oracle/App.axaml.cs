using System.IO;
using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using D2Oracle.Configuration;
using D2Oracle.Views;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace D2Oracle;

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
        
        builder.ConfigureAppConfiguration(configurationBuilder =>
        {
            configurationBuilder.SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", false, false);
        });

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