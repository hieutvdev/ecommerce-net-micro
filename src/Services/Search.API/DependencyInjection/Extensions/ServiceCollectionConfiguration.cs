using Search.API.Configurations;

namespace Search.API.DependencyInjection.Extensions;

public static class ServiceCollectionConfiguration
{
    public static IServiceCollection AddApiService(this IServiceCollection services, IConfiguration configuration)
    {
        var els = new ElasticsearchConfiguration();
        
        configuration.GetSection("ElasticSearch").Bind(els);

        return services;
    }
}