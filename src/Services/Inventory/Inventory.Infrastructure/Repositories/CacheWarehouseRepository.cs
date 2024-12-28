using System.Text.Json;
using BuildingBlocks.Pagination;
using Inventory.Domain.Models;
using Inventory.Domain.Repositories;
using Inventory.Infrastructure.Configurations;
using Microsoft.Extensions.Caching.Distributed;
using StackExchange.Redis;

namespace Inventory.Infrastructure.Repositories;

public class CacheWarehouseRepository
(IDistributedCache cache, IWarehouseRepository warehouseRepository, IConnectionMultiplexer connectionMultiplexer) : IWarehouseRepository
{
    public async Task<IEnumerable<Warehouse>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var cachedWarehouse = await cache.GetStringAsync(CacheKey.WarehouseKey, cancellationToken);
        if (!string.IsNullOrEmpty(cachedWarehouse))
        {
            return JsonSerializer.Deserialize<IEnumerable<Warehouse>>(cachedWarehouse)!;
        }

        var warehouses = await warehouseRepository.GetAllAsync(cancellationToken);
        
        await cache.SetStringAsync(CacheKey.WarehouseKey, JsonSerializer.Serialize(warehouses),
            new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromHours(2)
            }, cancellationToken);
        return warehouses;
    }

    public async Task<bool> CreateAsync(Warehouse warehouse, CancellationToken cancellationToken = default)
    {
        var isCreated = await warehouseRepository.CreateAsync(warehouse, cancellationToken);
        if (!isCreated)
        {
            return false;
        }

        await foreach (var key in GetAllKeyFromParent(CacheKey.WarehouseKey).WithCancellation(cancellationToken))
        {
            await cache.RemoveAsync(key, cancellationToken);
        }

        return true;
    }

    public async Task<PaginatedResult<Warehouse>> GetPageAsync(PaginationRequest paginationRequest, CancellationToken cancellationToken = default)
    {
        var cacheKey = $"{CacheKey.WarehouseKey}{paginationRequest.PageIndex}::{paginationRequest.PageSize}";
        var cacheResponse = await cache.GetStringAsync(cacheKey
            , cancellationToken);
        if (!string.IsNullOrEmpty(cacheResponse))
        {
            return JsonSerializer.Deserialize<PaginatedResult<Warehouse>>(cacheResponse)!;
        }

        var warehouse = await warehouseRepository.GetPageAsync(paginationRequest, cancellationToken);

        await cache.SetStringAsync(cacheKey, JsonSerializer.Serialize(warehouse), new DistributedCacheEntryOptions
        {
            AbsoluteExpirationRelativeToNow = TimeSpan.FromHours(2)
        }, cancellationToken);
        return warehouse;


    }


    private async IAsyncEnumerable<string> GetAllKeyFromParent(string parent)
    {
        if (string.IsNullOrEmpty(parent))
            throw new ArgumentException("Value cannot be null or whitespace");

        foreach (var endPoint in connectionMultiplexer.GetEndPoints())
        {
            var server = connectionMultiplexer.GetServer(endPoint);
            foreach (var key in server.Keys(pattern: parent + '*'))
            {
                yield return key.ToString();
            }
        }
        
        
    }
}