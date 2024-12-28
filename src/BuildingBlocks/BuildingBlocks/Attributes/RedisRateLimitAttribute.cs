using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using StackExchange.Redis;

namespace BuildingBlocks.Attributes;

public class RedisRateLimitAttribute : Attribute, IAsyncActionFilter, IEndpointFilter
{
    private readonly int _limit;
    private readonly TimeSpan _period = TimeSpan.FromMinutes(1);
    private readonly string _keyPrefix = "ratelimit";
    private readonly IConnectionMultiplexer _redis;

    public RedisRateLimitAttribute(int limit)
    {
        _limit = limit;
    }
    
    
    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        var clientIp = context.HttpContext.Connection.RemoteIpAddress?.ToString();
        var redisKey = $"{_keyPrefix}:{clientIp}";

        var db = _redis.GetDatabase();
        var currentCount = await db.StringIncrementAsync(redisKey);

        if (currentCount == 1)
        {
            await db.KeyExpireAsync(redisKey, _period);
        }

        if (currentCount > _limit)
        {
            context.Result = new ContentResult
            {
                StatusCode = 429,
                Content = "Rate limit exceeded, Try again later"
            };
            return;
        }

        await next();
    }

    public ValueTask<object?> InvokeAsync(EndpointFilterInvocationContext context, EndpointFilterDelegate next)
    {
        throw new NotImplementedException();
    }
}