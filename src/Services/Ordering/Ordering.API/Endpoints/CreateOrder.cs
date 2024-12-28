using Ordering.Application.Orders.Commands.CreateOrder;
using Ordering.Domain.Models;

namespace Ordering.API.Endpoints;


public record CreateOrderRequest(OrderDto Order);
public record CreateOrderResponse(Guid Id);


public class CreateOrder : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("/orders", async (CreateOrderRequest request, ISender sender) =>
            {
                var command = request.Adapt<CreateOrderCommand>();

                var result = await sender.Send(command);

                var response = result.Adapt<CreateOrderResponse>();


                return Results.Created($"/orders/{response.Id}", response);
            })
           
            .WithName("CreateOrder")
            .Produces<CreateOrderResponse>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .WithSummary("Create Order")
            .WithDescription("Create Order");
    }
}