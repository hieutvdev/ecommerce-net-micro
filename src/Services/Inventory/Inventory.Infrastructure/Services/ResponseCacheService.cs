





using Inventory.Application.Services;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using StackExchange.Redis;

namespace Inventory.Infrastructure.Services;

public class ResponseCacheService : IResponseCacheService
{
    private readonly IDistributedCache _distributedCache;
    private readonly IConnectionMultiplexer _connectionMultiplexer;
    
    public ResponseCacheService(IDistributedCache distributedCache, IConnectionMultiplexer connectionMultiplexer)
    {
        _distributedCache = distributedCache;
        _connectionMultiplexer = connectionMultiplexer;
    }
    public async Task<string?> GetCacheResponseAsync(string cacheKey)
    {
        string? cacheResponse = await _distributedCache.GetStringAsync(cacheKey)!;
        return string.IsNullOrEmpty(cacheResponse) ? null : cacheResponse;
    }


    public async Task RemoveCacheResponseAsync(string partern)
    {
        if (string.IsNullOrEmpty(partern))
        {
            throw new ArgumentException("Value cannot be null or empty.");
        }

        await foreach(var key in GetKeyAsync(partern))
        {
            await _distributedCache.RemoveAsync(key);
        }

    }


    private async IAsyncEnumerable<string> GetKeyAsync(string partern)
    {
        if (string.IsNullOrEmpty(partern))
        {
            throw new ArgumentException("Value cannot be null or empty.");
        }

        foreach(var endpoint in _connectionMultiplexer.GetEndPoints())
        {
            var server = _connectionMultiplexer.GetServer(endpoint);
            foreach(var key in server.Keys(pattern: partern + "*"))
            {
                yield return key.ToString();
            }
        }
    }

    public async Task SetCacheResponseAsync(string cacheKey, object? response, TimeSpan timeSpan)
    {
        if (response == null)
            return;

        var serializedResponse = JsonConvert.SerializeObject(response, new JsonSerializerSettings
        {
            ContractResolver = new CamelCasePropertyNamesContractResolver()
        });

        await _distributedCache.SetStringAsync(cacheKey, serializedResponse, new DistributedCacheEntryOptions
        {
            AbsoluteExpirationRelativeToNow = timeSpan
        });

    }
}

