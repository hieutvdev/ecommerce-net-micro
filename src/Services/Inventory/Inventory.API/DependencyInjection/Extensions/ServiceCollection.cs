

using Microsoft.Extensions.DependencyInjection;

namespace Inventory.API.DependencyInjection.Extensions;

public static class ServiceCollection
{
    public static IServiceCollection AddApiService(this IServiceCollection service, IConfiguration configuration)
    {

      

        service.AddCarter();
       

      
        
        
        
        service.AddHealthChecks()
            .AddRedis(configuration.GetSection("RedisConfiguration:ConnectionStrings").Value!)
            .AddNpgSql(configuration.GetConnectionString("Database")!);
           

        return service;
    }

    public static IServiceCollection AddSerilogConfiguration(this IServiceCollection services, IConfiguration configuration)
    {
        Log.Logger = new LoggerConfiguration()
            .ReadFrom.Configuration(configuration)
            .CreateLogger();
        services.AddSerilog();

        return services;
    }


    public static WebApplication UseApiService(this WebApplication app)
    {
        app.MapCarter();
        app.UseHealthChecks("/health",
            new HealthCheckOptions
            {
                ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
            });
        
        return app;
    }
}