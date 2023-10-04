using Global5.Application.AutoMapper;
using Global5.Application.Interfaces;
using Global5.Application.Services;
using Global5.Domain.Entities.Translations;
using AutoMapper;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using Global5.Domain.Interfaces.Repository;
using Global5.Infra.Data.Repository;

namespace Global5.Infra.CrossCutting.Ioc
{
    public static class Bootstrapper
    {
        public static void SetupIoC(this IServiceCollection services, IConfiguration configuration)
        {
            services.ConfigureAutoMapper();
            services.RegisterConfigSingletonServiceLanguage();
            services.RegisterRepositories(configuration);
            services.RegisterBackGroundService();
            services.RegisterServices();
        }
        private static void RegisterBackGroundService(this IServiceCollection services)
        {
            //services.AddHostedService<LiquidateBackgroundService>();
        }
        public static void SetupIoCWorker(this IServiceCollection services, IConfiguration configuration)
        {
            services.ConfigureAutoMapper();
            services.RegisterConfigSingletonServiceLanguage();
        }

        private static void ConfigureAutoMapper(this IServiceCollection services)
        {
            services.AddAutoMapper(typeof(DomainToViewModelMappingProfile), typeof(ViewModelToDomainMappingProfile));
        }

        private static void RegisterServices(this IServiceCollection services)
        {
            services.AddTransient<ILogService, LogService>();
            services.AddTransient<ITranslateService, TranslateService>();
            services.AddTransient<IZipService, ZipService>();
            services.AddTransient<IBlobStorageService, BlobStorageService>();
            services.AddTransient<IUsersService, UsersService>();
            services.AddTransient<ITokenService, TokenService>();
            services.AddTransient<IVehicleBrandService, VehicleBrandService>();
            services.AddTransient<ILogService, LogService>();

        }

        private static void RegisterRepositories(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddTransient<IVehicleBrandRepository>(service =>
            {
                var connectionString = configuration.GetConnectionString("DefaultConnection");
                return new VehicleBrandRepository(connectionString);
            }); 
            
            services.AddTransient<IUsersRepository>(service =>
            {
                var connectionString = configuration.GetConnectionString("DefaultConnection");
                return new UsersRepository(connectionString);
            });

            services.AddTransient<ILogRegistrationRepository>(service =>
            {
                var connectionString = configuration.GetConnectionString("DefaultConnection");
                return new LogRegistrationRepository(connectionString);
            });

            services.AddTransient<IFunctionalityRepository>(service =>
            {
                var connectionString = configuration.GetConnectionString("DefaultConnection");
                return new FunctionalityRepository(connectionString);
            });
        }

        private static void RegisterConfigSingletonServiceLanguage(this IServiceCollection services)
        {
            var list = new List<Language>();

            string relativePath = $"Translations";

            string directoryPath = Path.GetFullPath(relativePath);
            if (Directory.Exists(directoryPath))
            {
                var files = Directory.GetFiles(directoryPath);

                foreach (var file in files)
                {
                    using var streamReader = new StreamReader(file);

                    var fileInfo = new FileInfo(file);

                    string json = streamReader.ReadToEnd();
                    var deserializedObject = JsonConvert.DeserializeObject<Language>(json);

                    list.Add(deserializedObject);
                }
            }
            services.AddSingleton<IEnumerable<Language>>(list);
        }
    }
}