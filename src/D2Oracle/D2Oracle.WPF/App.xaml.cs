using System.Windows;
using D2Oracle.Core.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace D2Oracle.WPF
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private IHost? AppHost { get; set; }
        
        protected override async void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            AppHost = CreateAppHost();
            DISource.Resolver = type => AppHost.Services.GetRequiredService(type);
            await AppHost.StartAsync();
        }
        
        private static IHost CreateAppHost()
        {
            var builder = Host.CreateDefaultBuilder();
        
            builder.ConfigureServices((context, services) =>
            {
                services.ConfigureAppServices(context.Configuration);
                services.AddAppServices();
                services.AddAppViewModels();
            });
        
            return builder.Build();
        }

        protected override async void OnExit(ExitEventArgs e)
        {
            await AppHost!.StopAsync();
            base.OnExit(e);
        }
    }
}