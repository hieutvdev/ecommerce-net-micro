
using Inventory.Application.Dtos.ResponseBase;

namespace Inventory.Application.Usecase.V1.Queries.Warehouses.GetWarehouses;

public record GetWarehouseQuery(PaginationRequest PaginationRequest) : IQuery<ResponseBase>;