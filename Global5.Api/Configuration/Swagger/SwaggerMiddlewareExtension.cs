using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using System.Diagnostics.CodeAnalysis;

namespace Global5.Api.Configuration.Swagger
{
    [ExcludeFromCodeCoverage]
    public static class SwaggerMiddlewareExtension
    {
        public static void UseSwagger(this IApplicationBuilder app, IApiVersionDescriptionProvider versionProvider)
        {
            app.UseSwagger(options => options.RouteTemplate = "docs/{documentName}/swagger.json");

            app.UseSwaggerUI(options =>
            {
                foreach (var description in versionProvider.ApiVersionDescriptions)
                {
                    options.RoutePrefix = "swagger";
                    options.SwaggerEndpoint($"../docs/{description.GroupName}/swagger.json", description.GroupName);
                }
            });
        }
    }
}