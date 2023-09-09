using D2Oracle.Extensions;
using D2Oracle.Services;
using D2Oracle.ViewModels;
using Microsoft.Extensions.Configuration;
using Splat;

namespace D2Oracle.DI;

public static class Bootstrapper
{
    public static void Register(IMutableDependencyResolver services, IReadonlyDependencyResolver resolver,
        IConfiguration configuration)
    {
        services.RegisterLazySingleton<IDotaGSIService>(() => new DotaGSIService(configuration));
        
        services.Register(() => new MainWindowViewModel(
            resolver.GetRequiredService<IDotaGSIService>(),
            resolver.GetRequiredService<CurrentStateInfoViewModel>()));
        
        services.Register(() => new CurrentStateInfoViewModel(resolver.GetRequiredService<IDotaGSIService>()));
    }
}