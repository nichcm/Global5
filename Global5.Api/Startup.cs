using Global5.Api.Configuration.ApiVersioning;
using Global5.Api.Configuration.Auth;
using Global5.Api.Configuration.Cors;
using Global5.Api.Configuration.IoC;
using Global5.Api.Configuration.Swagger;
using Global5.Application.Validation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;
using System.Diagnostics.CodeAnalysis;

namespace Global5.Api
{
    [ExcludeFromCodeCoverage]
    public class Startup
    {
        public Startup(IHostEnvironment env)
        {
            ConfigureEnvironmentVariables(env);
        }

        public IConfiguration Configuration { get; private set; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            services.ConfigureCors(Configuration);
            services.RegisterApiversion();
            services.RegisterDependencies(Configuration);
            services.ConfigureJwtAuthentication(Configuration);
            services.AddSwaggerDocumentation();
            services.AddResponseCaching();
            services.AddHttpContextAccessor();
            services.AddMemoryCache();
            services.AddHttpClient();

            services.AddMvc()
                .AddNewtonsoftJson(options =>
                {
                    options.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
                    options.SerializerSettings.Converters.Add(new StringEnumConverter());
                    options.SerializerSettings.NullValueHandling = NullValueHandling.Ignore;
                })
                .AddFluentValidation(fv => fv.RegisterValidatorsFromAssemblyContaining<FluentContainer>())
                .ConfigureApiBehaviorOptions(options =>
                {
                    options.SuppressModelStateInvalidFilter = true;
                });
        }
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IApiVersionDescriptionProvider versionProvider)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseHttpsRedirection();
            app.UseRouting();
            app.UseCors("CorsPolicy");
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseResponseCaching();
            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
            app.UseSwagger(versionProvider);
        }

        private void ConfigureEnvironmentVariables(IHostEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", true, true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", true);

            builder.AddEnvironmentVariables();
            Configuration = builder.Build();
        }
    }
}