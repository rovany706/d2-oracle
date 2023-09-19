using D2Oracle.Services;
using D2Oracle.Services.Audio;
using D2Oracle.Services.DotaKnowledge;
using D2Oracle.Services.Roshan;
using D2Oracle.ViewModels;
using D2Oracle.ViewModels.Dashboard;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

// ReSharper disable UnusedMethodReturnValue.Global

namespace D2Oracle.Configuration;

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
        services.AddSingleton<IRoshanTimerService, RoshanTimerService>();
        services.AddSingleton<IDotaAudioService, DotaAudioService>();
        services.AddSingleton<IDotaKnowledgeService, DotaKnowledgeService>();
        services.AddSingleton<NetWorthCalculator>();

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

        return services;
    }
}