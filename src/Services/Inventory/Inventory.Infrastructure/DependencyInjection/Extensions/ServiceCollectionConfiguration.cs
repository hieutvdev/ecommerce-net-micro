using Inventory.Application.Data;
using Inventory.Application.Services;
using Inventory.Domain.Repositories;
using Inventory.Infrastructure.Configurations;
using Inventory.Infrastructure.Data;
using Inventory.Infrastructure.Data.Interceptors;
using Inventory.Infrastructure.Repositories;
using Inventory.Infrastructure.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using StackExchange.Redis;

namespace Inventory.Infrastructure.DependencyInjection.Extensions;

public static class ServiceCollectionConfiguration
{
    const string CorsName = "inventory_cors";
    public static IServiceCollection AddInfrastructureService(this IServiceCollection services, IConfiguration configuration)
    {
        
        var databaseConnectionString = configuration.GetConnectionString("Database");
        var redisConfiguration = new RedisConfiguration();
        configuration.GetSection("RedisConfiguration").Bind(redisConfiguration);


        services.AddCors(builder =>
        {
            builder.AddPolicy(name: CorsName, options =>
            {
                options.WithOrigins("*")
                    .AllowAnyHeader()
                    .AllowAnyMethod();
            });
        });
        // services.AddMarten((options) =>
        // {
        //     options.Connection(databaseConnectionString!);
        //     options.AutoCreateSchemaObjects = AutoCreate.All;
        // }).UseLightweightSessions();

        services.AddScoped<ISaveChangesInterceptor, AuditableEntityInterceptor>();
        services.AddScoped<ISaveChangesInterceptor, DispatchDomainEventsInterceptor>();

        services.AddDbContext<ApplicationDbContext>((sp, options) =>
        {
            var interceptors = sp.GetServices<ISaveChangesInterceptor>().ToArray();
            options.AddInterceptors(interceptors);
            options.UseNpgsql(databaseConnectionString)
                .LogTo(Console.WriteLine, LogLevel.Information)  
                .EnableSensitiveDataLogging();
        });

        services.AddSingleton(redisConfiguration);
        
        if (!redisConfiguration.Enabled)
            return services;

        services.AddSingleton<IConnectionMultiplexer>(_ => ConnectionMultiplexer.Connect(redisConfiguration.ConnectionStrings));

        services.AddStackExchangeRedisCache(options =>
        {
            options.Configuration = redisConfiguration.ConnectionStrings;
            options.InstanceName = "Inventory_Cache";
        });
        
        return services;
    }

    public static IServiceCollection RegisterRepository(this IServiceCollection services)
    {
        services.AddScoped(typeof(IRepositoryBase<>), typeof(RepositoryBase<>));
        services.AddScoped(typeof(IMartenRepository<>), typeof(MartenRepository<>));
        services.AddScoped<IWarehouseRepository, WarehouseRepository>();
        //services.Decorate<IWarehouseRepository, CacheWarehouseRepository>();
        return services;
    }

    public static IServiceCollection RegisterService(this IServiceCollection services)
    {
        services.AddScoped<IApplicationDbContext, ApplicationDbContext>();
        services.AddSingleton<IResponseCacheService, ResponseCacheService>();

        return services;
    }
    
    

    public static WebApplication UseInfrastructureService(this WebApplication app)
    {
        app.UseCors(CorsName);
        return app;
    }
}

