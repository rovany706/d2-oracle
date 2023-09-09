using Avalonia;
using Avalonia.ReactiveUI;
using System;
using System.IO;
using D2Oracle.DI;
using Microsoft.Extensions.Configuration;
using Splat;

namespace D2Oracle;

class Program
{
    // Initialization code. Don't use any Avalonia, third-party APIs or any
    // SynchronizationContext-reliant code before AppMain is called: things aren't initialized
    // yet and stuff might break.
    [STAThread]
    public static void Main(string[] args)
    {
        var configuration = LoadConfiguration();
        RegisterDependencies(configuration);
        
        BuildAvaloniaApp()
            .StartWithClassicDesktopLifetime(args);
    }

    private static IConfigurationRoot LoadConfiguration()
    {
        var builder = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", false, false);

        var configuration = builder.Build();
        return configuration;
    }

    private static void RegisterDependencies(IConfiguration configuration)
    {
        Bootstrapper.Register(Locator.CurrentMutable, Locator.Current, configuration);
    }

    // Avalonia configuration, don't remove; also used by visual designer.
    public static AppBuilder BuildAvaloniaApp()
        => AppBuilder.Configure<App>()
            .UsePlatformDetect()
            .WithInterFont()
            .LogToTrace()
            .UseReactiveUI();
}