using D2Oracle.Services;
using D2Oracle.Services.Audio;
using D2Oracle.Services.Roshan;
using D2Oracle.ViewModels;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace D2Oracle.Configuration;

public static class DependencyInjection
{
    public static IServiceCollection ConfigureAppServices(this IServiceCollection services,
        IConfiguration configuration)
    {
        services.Configure<DotaConnectionOptions>(
            configuration.GetSection(DotaConnectionOptions.DotaConnectionSectionName));

        return services;
    }

    public static IServiceCollection AddAppServices(this IServiceCollection services)
    {
        services.AddSingleton<IDotaGsiService, DotaGsiService>();
        services.AddSingleton<IRoshanTimerService, RoshanTimerService>();
        services.AddSingleton<IDotaAudioService, DotaAudioService>();

        return services;
    }

    public static IServiceCollection AddAppViewModels(this IServiceCollection services)
    {
        services.AddTransient<MainWindowViewModel>();
        services.AddTransient<CurrentStateInfoViewModel>();

        return services;
    }
}