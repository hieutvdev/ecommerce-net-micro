


using BuildingBlocks.Exceptions.Handler;
using BuildingBlocks.Messaging.MassTransit;
using FluentValidation;
using Inventory.Application.Mappers;
using Microsoft.AspNetCore.Builder;
using Microsoft.FeatureManagement;

namespace Inventory.Application.DependencyInjection.Extensions;

public static class ServiceCollectionConfiguration
{
    public static IServiceCollection AddApplicationService(this IServiceCollection services, IConfiguration configuration)
    {



        services.AddAutoMapper(typeof(ServiceProfile));
        services.AddMediatR(config =>
        {
            config.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly());
            config.AddOpenBehavior(typeof(ValidationBehavior<,>));
            config.AddOpenBehavior(typeof(LoggingBehavior<,>));
            config.AddOpenBehavior(typeof(PerformancePipelineBehavior<,>));
        });

        services.AddFeatureManagement();
        services.AddExceptionHandler<CustomExceptionHandler>();
        services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
        services.AddMessageBroker(configuration, Assembly.GetExecutingAssembly());

        return services;
    }

    public static WebApplication UseApplicationService(this WebApplication app)
    {
        app.UseExceptionHandler(config => { });

        return app;
    }
}

