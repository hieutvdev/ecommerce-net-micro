using System.Text;
using Catalog.API.Configurations;
using Catalog.API.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Catalog.API.Attributes;

public class CacheAttribute : Attribute, IAsyncActionFilter
{

    private readonly int _timeToLiveSeconds;

    public CacheAttribute(int timeToLiveSeconds = 1000)
    {
        _timeToLiveSeconds = timeToLiveSeconds;
    }
    
    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        var cacheConfiguration = context.HttpContext.RequestServices.GetRequiredService<RedisConfiguration>();

        if (!cacheConfiguration.Enabled)
        {
            await next();
            return;
        }

        var cacheService = context.HttpContext.RequestServices.GetRequiredService<IResponseCacheService>();
        
        var cacheKey = GenerateCacheKeyFromRequest(context.HttpContext.Request);
        
        var cacheResponse = await cacheService.GetCacheResponseAsync(cacheKey);

        if (!string.IsNullOrEmpty(cacheResponse))
        {
            var contentResult = new ContentResult
            {
                Content = cacheResponse,
                ContentType = "application/json",
                StatusCode = 200
            };
            context.Result = contentResult;
            return;
        }

        var excutedContext = await next();

        if (excutedContext.Result is OkObjectResult objectResult)
            await cacheService.SetCacheResponseAsync(cacheKey, objectResult, TimeSpan.FromSeconds(_timeToLiveSeconds*10));
        
        throw new NotImplementedException();
    }


    private static string GenerateCacheKeyFromRequest(HttpRequest request)
    {
        var keyBuilder = new StringBuilder();

        keyBuilder.Append($"{request.Path}");

        foreach (var (key, value) in request.Query.OrderBy(x => x.Key))
        {
            keyBuilder.Append($"|{key}-{value}");
        }
        

        return keyBuilder.ToString();
    }
    
}