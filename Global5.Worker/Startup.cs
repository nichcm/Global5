using Crdc.Agro.Plataforma.Infra.CrossCutting.Ioc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.IO;

namespace Crdc.Agro.Plataforma.Worker
{
    public static class Startup
    {
        public static IServiceProvider RegisterServices()
        {
            var services = new ServiceCollection();
            var path = Directory.GetCurrentDirectory();

            var configuration = GetConfiguration();

            // Services
            services.SetupIoCWorker(configuration);
            services.AddHttpClient();

            services.AddSingleton<IConfiguration>(configuration);
            services.AddSingleton<Image3DExecution>();
            services.AddSingleton<Image3DCameraExecution>();

            return services.BuildServiceProvider();
        }
        private static IConfiguration GetConfiguration()
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile($"appsettings.json", optional: false, reloadOnChange: true)
                .AddEnvironmentVariables();

            return builder.Build();
        }
        public static void DisposeServices(IServiceProvider serviceProvider)
        {
            if (serviceProvider == null)
            {
                return;
            }
            if (serviceProvider is IDisposable sp)
            {
                sp.Dispose();
            }
        }
    }
}