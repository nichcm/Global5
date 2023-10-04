using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Diagnostics.CodeAnalysis;

namespace Global5.Api.Configuration.Cors
{
    [ExcludeFromCodeCoverage]
    public static class CorsConfiguration
    {
        public static void ConfigureCors(this IServiceCollection services, IConfiguration configuration) =>
            services.AddCors(options =>
                options.AddPolicy("CorsPolicy",
                    builder => builder
                        .AllowAnyHeader()
                        .AllowAnyMethod()
                        .AllowAnyOrigin()
            ));

    }
}