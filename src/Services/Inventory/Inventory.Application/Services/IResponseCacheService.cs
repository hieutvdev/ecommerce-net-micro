

namespace Inventory.Application.Services;

public interface IResponseCacheService
{
    Task SetCacheResponseAsync(string cacheKey, object? response, TimeSpan timeSpan);

    Task<string?> GetCacheResponseAsync(string cacheKey);

    Task RemoveCacheResponseAsync(string partern);
}

