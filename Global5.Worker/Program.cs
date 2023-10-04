using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.IO;
using System.Threading.Tasks;

namespace Crdc.Agro.Plataforma.Worker
{
    public class Program
    {
        public static readonly string Namespace = typeof(Program).Namespace;
        public static readonly string AppName = Namespace.Substring(Namespace.LastIndexOf('.', Namespace.LastIndexOf('.') - 1) + 1);

        [Obsolete("Use WithSensitiveDataMasking with the options argument instead")]
        public static async Task Main(string[] args)
        {
            Console.WriteLine("Iniciando Schedules");
            var configuration = GetConfiguration();

            IServiceProvider serviceProvider = Startup.RegisterServices();
            var image3DExecution = serviceProvider.GetService<Image3DExecution>();
            var image3DCameraExecution = serviceProvider.GetService<Image3DCameraExecution>();

            await image3DExecution.ExecuteAsync();
            await image3DCameraExecution.ExecuteAsync();

            Console.WriteLine("Finalizando Schedules");
        }

        private static Microsoft.Extensions.Configuration.IConfiguration GetConfiguration()
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile($"appsettings.json", optional: false, reloadOnChange: true)
                .AddEnvironmentVariables();

            return builder.Build();
        }
    }
}