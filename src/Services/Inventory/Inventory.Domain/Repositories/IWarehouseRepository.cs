using BuildingBlocks.Pagination;
using Inventory.Domain.Models;

namespace Inventory.Domain.Repositories;

public interface IWarehouseRepository
{
    Task<IEnumerable<Warehouse>> GetAllAsync(CancellationToken cancellationToken = default!);
    Task<bool> CreateAsync(Warehouse warehouse, CancellationToken cancellationToken = default!);

    Task<PaginatedResult<Warehouse>> GetPageAsync(PaginationRequest paginationRequest,
        CancellationToken cancellationToken = default!);

}