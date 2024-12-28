using BuildingBlocks.CQRS;
using Inventory.Application.Dtos.ResponseBase;
using Inventory.Application.Dtos.Warehouses.Requests;
using Inventory.Application.Dtos.Warehouses.Responses;

namespace Inventory.Application.Usecase.V1.Commands.Warehouses.Create;

public record CreateWarehouseCommand(CreateWarehouseRequest Request) : ICommand<ResponseBase>;