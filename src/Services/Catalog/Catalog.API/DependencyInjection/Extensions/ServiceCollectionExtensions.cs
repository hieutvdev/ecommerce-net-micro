using Catalog.API.Configurations;
using Catalog.API.Mapper;
using Catalog.API.Services;
using Microsoft.AspNetCore.ResponseCompression;
using StackExchange.Redis;

namespace Catalog.API.DependencyInjection.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddConfigurationAutoMapper(this IServiceCollection service)
        => service.AddAutoMapper(typeof(ServiceProfile));

    public static IServiceCollection AddRedisService(this IServiceCollection services, IConfiguration configuration)
    {
        var redisConfiguration = new RedisConfiguration();
        
        
        configuration.GetSection("RedisConfiguration").Bind(redisConfiguration);
        
            
        services.AddSingleton(redisConfiguration);

        if (!redisConfiguration.Enabled)
            return services;

        services.AddSingleton<IConnectionMultiplexer>(_ =>
            ConnectionMultiplexer.Connect(redisConfiguration.ConnectionStrings));
        
        services.AddStackExchangeRedisCache(options => options.Configuration = redisConfiguration.ConnectionStrings);

        services.AddSingleton<IResponseCacheService, ResponseCacheService>();

        services.AddResponseCompression(options =>
        {
            options.EnableForHttps = true;
            options.MimeTypes = ResponseCompressionDefaults.MimeTypes.Concat(new[] { "application/json",
                "multipart/form-data",
                "application/pdf",
                "image/jpeg",
                "image/png",
                "application/zip",
                "application/octet-stream" });
        });
        return services;
    }


    public static WebApplication UseApiService(this WebApplication app)
    {

        app.UseResponseCompression();
        return app;
    }
}