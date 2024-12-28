namespace Catalog.API.Products.DeleteProduct;


public record DeleteProductResponse(bool IsSuccess);


public class DeleteProductEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapDelete("/products/{id}", async (Guid id, ISender sender) =>
        {
            var result = await sender.Send(new DeleteProductCommand(id));
            var response = result.Adapt<DeleteProductResponse>();
            var responseDto = new ResponseDto(Message: $"Delete Product With ID : {id} Successful", IsSuccess: response.IsSuccess);
            return Results.Ok(
                responseDto);
            
        })
            .WithName("Delete Product")            
            .Produces<DeleteProductResponse>()
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .WithSummary("Delete Product")
            .WithDescription("Delete Product");
    }
}