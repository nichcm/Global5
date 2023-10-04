using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System.Diagnostics.CodeAnalysis;
using System.Text;

namespace Global5.Api.Configuration.Auth
{
    [ExcludeFromCodeCoverage]
    public static class AuthenticationConfiguration
    {
        public static void ConfigureJwtAuthentication(this IServiceCollection services, IConfiguration configuration)
        {
            var secret = configuration.GetSection("JwtSettings").GetSection("secret").Value;
            var key = Encoding.ASCII.GetBytes(secret);
            var audience = configuration.GetSection("JwtSettings").GetSection("audience").Value;
            var issuer = configuration.GetSection("JwtSettings").GetSection("issuer").Value;

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;

            })
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false,

                };
                options.SaveToken = true;
            });
        }

    }
}