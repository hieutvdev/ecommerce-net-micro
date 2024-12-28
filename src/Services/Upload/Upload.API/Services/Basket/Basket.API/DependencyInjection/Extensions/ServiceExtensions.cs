using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

namespace Basket.API.DependencyInjection.Extensions;

public static class ServiceExtensions
{
    public static IServiceCollection AddAuthenticationService(this IServiceCollection services, IConfiguration configuration)
    {
        var jwtOptions = configuration.GetSection("ApiSettings:JwtOptions");
        var secret = jwtOptions["Secret"]!;
        var audience = jwtOptions["Audience"]!;
        var issuer = jwtOptions["Issuer"]!;
        
        // config authentication jwt
        var key = Encoding.UTF8.GetBytes(secret);

        services.AddAuthentication(x =>
        {
            x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        }).AddJwtBearer(x =>
        {
            x.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidAudience = audience,
                ValidIssuer = issuer,
                ClockSkew = TimeSpan.Zero
            };
        });


        return services;
    }
}