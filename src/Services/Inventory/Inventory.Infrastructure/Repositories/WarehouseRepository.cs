using BuildingBlocks.Pagination;
using Inventory.Domain.Models;
using Inventory.Domain.Repositories;

namespace Inventory.Infrastructure.Repositories;

public class WarehouseRepository(IRepositoryBase<Warehouse> repositoryBase) : IWarehouseRepository
{

    public async Task<IEnumerable<Warehouse>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await repositoryBase.GetAllAsync(cancellationToken);
    }

    public async Task<bool> CreateAsync(Warehouse warehouse, CancellationToken cancellationToken = default)
    {
        await repositoryBase.AddAsync(warehouse, cancellationToken);
       
        bool isSuccess = await repositoryBase.SaveChangesAsync(cancellationToken) > 0;

        return isSuccess;
    }

    public async Task<PaginatedResult<Warehouse>> GetPageAsync(PaginationRequest paginationRequest, CancellationToken cancellationToken = default)
    {
        var result = await repositoryBase.GetPageAsync(paginationRequest, cancellationToken);
        return result;
    }
}