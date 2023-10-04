using jca.BankSlip.Infra.CrossCutting.Ioc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace jca.BankSlip.Worker
{
    public class ProgramHost
    {
        public static IConfiguration Configuration { get; private set; }

        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureServices((hostContext, services) =>
                {
                    // Configurations
                    IConfiguration configuration = hostContext.Configuration;
                    services.SetupIoCWorker(configuration);
                    services.AddHttpClient();
                    RegisterConfiguration(args);
                    services.AddHostedService<CRDocumentExecution>();
                });

        public static void RegisterConfiguration(string[] args) =>
            Configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddEnvironmentVariables()
                .AddCommandLine(args)
                .Build();
    }
}
