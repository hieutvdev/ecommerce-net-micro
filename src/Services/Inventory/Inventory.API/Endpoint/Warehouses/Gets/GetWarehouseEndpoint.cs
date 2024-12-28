using BuildingBlocks.Pagination;
using Inventory.Application.Dtos.ResponseBase;
using Inventory.Application.Usecase.V1.Queries.Warehouses.GetWarehouses;
using MediatR;

namespace Inventory.API.Endpoint.Warehouses.Gets;

public class GetWarehouseEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("/warehouses", async ([AsParameters] PaginationRequest paginatedRequest, ISender sender) =>
            {
                var result = await sender.Send(new GetWarehouseQuery(paginatedRequest));

                return Results.Ok(result);
            })
            .WithName("GetWarehouse")
            .Produces<ResponseBase>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .WithSummary("Get Warehouse")
            .WithDescription("Get Warehouse");
        
       
    }
}