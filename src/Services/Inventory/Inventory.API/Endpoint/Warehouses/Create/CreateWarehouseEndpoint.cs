using Inventory.Application.Dtos.ResponseBase;
using Inventory.Application.Dtos.Warehouses.Requests;
using Inventory.Application.Usecase.V1.Commands.Warehouses.Create;
using MediatR;

namespace Inventory.API.Endpoint.Warehouses.Create;


public class CreateWarehouseEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("/warehouses", async (CreateWarehouseRequest res, ISender sender) =>
        {
            var result = await sender.Send(new CreateWarehouseCommand(res));

            return Results.Ok(result);
        })
        .WithName("CreateWarehouse")
        .Produces<ResponseBase>(StatusCodes.Status200OK)
        .ProducesProblem(StatusCodes.Status400BadRequest)
        .ProducesProblem(StatusCodes.Status404NotFound)
        .WithSummary("Create Warehouse")
        .WithDescription("Create Warehouse");
        
       
    }
}