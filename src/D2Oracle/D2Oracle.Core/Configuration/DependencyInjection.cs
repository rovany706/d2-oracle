using System.IO.Abstractions;
using D2Oracle.Core.Services;
using D2Oracle.Core.Services.Audio;
using D2Oracle.Core.Services.DotaKnowledge;
using D2Oracle.Core.Services.NetWorth;
using D2Oracle.Core.Services.Settings;
using D2Oracle.Core.Services.Timers.Roshan;
using D2Oracle.Core.Services.Timers.Runes;
using D2Oracle.Core.ViewModels;
using D2Oracle.Core.ViewModels.Dashboard;
using D2Oracle.Core.ViewModels.Dashboard.Timings;
using D2Oracle.Core.ViewModels.Settings;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

// ReSharper disable UnusedMethodReturnValue.Global

namespace D2Oracle.Core.Configuration;

public static class DependencyInjection
{
    public static IServiceCollection ConfigureAppServices(this IServiceCollection services,
        IConfiguration configuration)
    {
        services.Configure<DotaConnectionOptions>(
            configuration.GetSection(DotaConnectionOptions.DotaConnectionSectionName));

        services.Configure<SoundsOptions>(
            configuration.GetSection(SoundsOptions.SoundsOptionsSectionName));
        
        return services;
    }

    public static IServiceCollection AddAppServices(this IServiceCollection services)
    {
        services.AddSingleton<IDotaGsiService, DotaGsiService>();
        services.AddSingleton<IRoshanDeathObserverService, RoshanDeathObserverService>();
        services.AddSingleton<IRoshanTimerService, RoshanTimerService>();
        services.AddSingleton<IRoshanItemDropService, RoshanItemDropService>();
        services.AddSingleton<IDotaAudioService, DotaAudioService>();
        services.AddSingleton<IDotaKnowledgeService, DotaKnowledgeService>();
        services.AddSingleton<INetWorthCalculator, NetWorthCalculator>();
        services.AddSingleton<IWisdomRuneTimerService, WisdomRuneTimerService>();
        services.AddSingleton<IDotaConfigInstallationService, DotaConfigInstallationService>();
        services.AddSingleton<IDotaProcessLocator, DotaProcessLocator>();
        services.AddSingleton<IFileSystem>(new FileSystem());

        return services;
    }

    public static IServiceCollection AddAppViewModels(this IServiceCollection services)
    {
        services.AddTransient<MainWindowViewModel>();
        services.AddTransient<DashboardPageViewModel>();
        services.AddTransient<CurrentStateInfoViewModel>();
        services.AddTransient<SettingsViewModel>();
        services.AddTransient<TimingsCardViewModel>();
        services.AddTransient<HeroStatsCardViewModel>();
        services.AddTransient<HeroDiagramsCardViewModel>();
        services.AddTransient<RoshanTimingsViewModel>();
        services.AddTransient<WisdomRuneTimingsViewModel>();
        services.AddTransient<DotaConnectionSettingsViewModel>();
        services.AddTransient<RoshanItemsDropViewModel>();

        return services;
    }
}