using System;
using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using D2Oracle.Extensions;
using D2Oracle.Services;
using D2Oracle.ViewModels;
using D2Oracle.Views;
using Microsoft.Extensions.DependencyInjection;
using Splat;

namespace D2Oracle;

public partial class App : Application
{
    public override void Initialize()
    {
        AvaloniaXamlLoader.Load(this);
    }

    public override void OnFrameworkInitializationCompleted()
    {
        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            desktop.MainWindow = new MainWindow
            {
                DataContext = GetRequiredService<MainWindowViewModel>()
            };
        }

        base.OnFrameworkInitializationCompleted();
    }
    
    private static T GetRequiredService<T>() => Locator.Current.GetRequiredService<T>();
}