using Global5.Infra.CrossCutting.Ioc;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Diagnostics.CodeAnalysis;
using System.Security.Claims;

namespace Global5.Api.Configuration.IoC
{
    [ExcludeFromCodeCoverage]
    public static class IocConfiguration
    {
        public static void RegisterDependencies(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddTransient<ClaimsPrincipal>(s => s.GetService<IHttpContextAccessor>().HttpContext.User);
            services.SetupIoC(configuration);
        }
    }
}