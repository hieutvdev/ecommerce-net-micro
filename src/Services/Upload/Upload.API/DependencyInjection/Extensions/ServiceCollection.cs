using BuildingBlocks.Behaviors;
using BuildingBlocks.Exceptions.Handler;
using Carter;
using FluentValidation;
using Microsoft.FeatureManagement;
using System.Reflection;
using Amazon.Extensions.NETCore.Setup;
using Amazon.Runtime;
using Amazon.S3;
using Upload.API.Configuration;
using Upload.API.Services;
using Upload.API.Services.S3Upload;
using Upload.API.Services.UploadCloudinary;

namespace Upload.API.DependencyInjection.Extensions;

public static class ServiceCollection
{
    private const string Key = "CloudinarySetting";

    public static IServiceCollection AddServiceConfiguration(this IServiceCollection services,
        IConfiguration configuration)
    {

        services.AddCarter();
        services.AddExceptionHandler<CustomExceptionHandler>();
        services.AddSingleton<IUploadFileService, UploadFileService>();
        services.Configure<CloudinaryConfigurationSetting>(configuration.GetSection(Key));
        services.AddScoped<ICloudinaryService, CloudinaryService>();
        return services;
    }

    public static IServiceCollection AddConfigurationMediaR(this IServiceCollection services)
    {
        services.AddMediatR(config =>
        {
            config.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly());
            config.AddOpenBehavior(typeof(ValidationBehavior<,>));
            config.AddOpenBehavior(typeof(LoggingBehavior<,>));
        });

        services.AddFeatureManagement();
        services.AddExceptionHandler<CustomExceptionHandler>();
        services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

        return services;
    }

    public static IServiceCollection AddConfigurationS3(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddAWSService<IAmazonS3>(new AWSOptions
        {
            Credentials = new BasicAWSCredentials(
                configuration["S3:AccessKey"], 
                configuration["S3:SecretKey"]
            ),
            Region = Amazon.RegionEndpoint.GetBySystemName(configuration["S3:Region"])
        });

        services.AddScoped<IS3UploadService, S3UploadService>();
        return services;
    }

    public static WebApplication UseApiService(this WebApplication app)
    {
        app.MapCarter();
        app.UseExceptionHandler(config => { });
        return app;
    }
}