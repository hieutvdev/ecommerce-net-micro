
using Inventory.Application.Dtos.ResponseBase;

namespace Inventory.Application.Usecase.V1.Queries.Warehouses.GetWarehouses;

public class GetWarehousesHandler(IWarehouseRepository warehouseRepository)
: IQueryHandler<GetWarehouseQuery, ResponseBase>
{
    public async Task<ResponseBase> Handle(GetWarehouseQuery request, CancellationToken cancellationToken)
    {
        var warehouses = await warehouseRepository.GetPageAsync(request.PaginationRequest, cancellationToken);
        var warehouseDto = warehouses.Data.Select(x => new WarehouseDto(
            x.Id.Value, x.WarehouseName, x.ManagerId.Value, x.WarehouseStatus, WarehouseExtensions.AddressToDto(x.Location)));
        var result =  new PaginatedResult<WarehouseDto>(warehouses.PageIndex, warehouses.PageSize, warehouses.Count, warehouseDto);
        return new ResponseBase(Metadata: result, Message: warehouses.Count > 0 ? "S" : "E",
            IsSuccess: warehouses.Count > 0);
    }
    
}