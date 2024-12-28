using MassTransit;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Ordering.Application.Data;
using Ordering.Infrastructure.DependencyInjection.Options;

namespace Ordering.Infrastructure.DependencyInjection.Extensions;

public static class ServiceCollection
{
    public static IServiceCollection AddInfrastructureServices
        (this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("Database");

        // Add services to the container.
        services.AddScoped<ISaveChangesInterceptor, AuditableEntityInterceptor>();
        services.AddScoped<ISaveChangesInterceptor, DispatchDomainEventsInterceptor>();

        services.AddDbContext<ApplicationDbContext>((sp, options) =>
        {
            options.AddInterceptors(sp.GetServices<ISaveChangesInterceptor>());
            options.UseSqlServer(connectionString);
        });

        services.AddScoped<IApplicationDbContext, ApplicationDbContext>();

        return services;
    }

    public static IServiceCollection AddConfigureMasstransitRabbitMQ(this IServiceCollection services,
        IConfiguration configuration)
    {
        var masstransitConfiguration = new MasstransitConfiguration();
        
        configuration.GetSection(nameof(MasstransitConfiguration)).Bind(masstransitConfiguration);

        services.AddMassTransit(mt =>
        {
            mt.UsingRabbitMq((context, bus) =>
            {
                bus.Host(new Uri(masstransitConfiguration.Host!), host =>
                {
                    host.Username(masstransitConfiguration.UserName);
                    host.Password(masstransitConfiguration.Password);
                });
            });
        });


        return services;
    }
}