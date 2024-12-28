using System.Reflection;
using System.Text;
using Auth.Application.Auths.Commands.AuthLogin;
using Auth.Application.Services;
using BuildingBlocks.Behaviors;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.FeatureManagement;
using Microsoft.IdentityModel.Tokens;

namespace Auth.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddMediatR(config =>
        {
            config.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly());
            config.AddOpenBehavior(typeof(ValidationBehavior<,>));
            config.AddOpenBehavior(typeof(LoggingBehavior<,>));
        });


        services.AddValidatorsFromAssemblyContaining<AuthLoginCommandValidator>();
        

        

        services.Configure<JwtOptionsSetting>(options =>
        {
            options.Secret = configuration["ApiSettings:JwtOptions:Secret"]!;
            options.Audience = configuration["ApiSettings:JwtOptions:Audience"]!;
            options.Issuer = configuration["ApiSettings:JwtOptions:Issuer"]!;
        });

        services.AddStackExchangeRedisCache(options =>
        {
            options.Configuration = configuration.GetConnectionString("Redis");
        });
        services.AddFeatureManagement();
        services.AddHttpContextAccessor();
        services.AddScoped<IJwtTokenGenerator, JwtTokenGenerator>();
        services.AddScoped<IAuthRepository, AuthRepository>();
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IKeyRepository<Guid>, KeyRepository>();
        services.AddScoped<IRoleRepository, RoleRepository>();
        //services.Decorate<IAuthRepository, TokenManagementRepository>();
        services.AddAutoMapper(typeof(ServiceProfile));
        return services;
    }

    public static IServiceCollection AddApplicationAuthentication(this IServiceCollection services,
        IConfiguration configuration)
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

        var tokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(key),
            ValidateIssuer = false,
            ValidateAudience = false,
            ValidAudience = audience,
            ValidIssuer = issuer,
            RequireExpirationTime = true,
            ValidateLifetime = true,
            ClockSkew = TimeSpan.Zero,
        };
        services.AddSingleton(tokenValidationParameters);
        return services;
    }
    
    
}