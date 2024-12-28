using AutoMapper;
using Inventory.Application.Dtos.ResponseBase;
using Inventory.Application.Dtos.Warehouses.Requests;
using Inventory.Domain.ValueObjects;

namespace Inventory.Application.Usecase.V1.Commands.Warehouses.FakeData;

public class FakeWarehouseHandler
(IWarehouseRepository warehouseRepository, IMapper mapper)
: ICommandHandler<FakeWarehouseCommand, ResponseBase>
{
    public async Task<ResponseBase> Handle(FakeWarehouseCommand request, CancellationToken cancellationToken)
    {
        
        var warehouse = CreateNewWarehouse(request.Request);
        
        var isSuccess =  await warehouseRepository.CreateAsync(warehouse, cancellationToken);

        return new ResponseBase(Metadata: warehouse.Id.Value, IsSuccess: isSuccess, Message: isSuccess ? "Create warehouse successful" : "Create warehouse failure");
    }

    private Warehouse CreateNewWarehouse(CreateWarehouseRequest warehouseRequest)
    {
        var address = Address.Of(warehouseRequest.Address.FirstName, warehouseRequest.Address.LastName, warehouseRequest.Address.EmailAddress, warehouseRequest.Address.AddressLine, warehouseRequest.Address.Country, warehouseRequest.Address.State, warehouseRequest.Address.ZipCode);
        var warehouse = Warehouse.Create(
            id: WarehouseId.Of(Guid.NewGuid()),
            name: warehouseRequest.WarehouseName,
            location: address,
            managerId: ManagerId.Of(warehouseRequest.ManagerId)
        );
        return warehouse;
    }
}




