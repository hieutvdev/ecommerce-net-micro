using BuildingBlocks.Pagination;
using Inventory.Application.Dtos.Warehouses;
using Inventory.Application.Dtos.Warehouses.Requests;

namespace Inventory.Application.Services;

public interface IWarehouseService
{
    Task<bool> CreateWarehouseAsync(CreateWarehouseRequest entity, CancellationToken cancellationToken = default!);
    Task<IEnumerable<Warehouse>> GetByManagerAsync(Guid managerId, CancellationToken cancellationToken = default!);

    Task<PaginatedResult<WarehouseDto>> GetPageWarehouseAsync(PaginationRequest paginationRequest,
        CancellationToken cancellationToken = default!);

}