
using BuildingBlocks.Attributes;
using Catalog.API.Attributes;
using Catalog.API.Models;

namespace Catalog.API.Products.GetProducts;

public record GetProductsRequest(int? PageNumber = 1, int? PageSize = 10);
public record GetProductsResponse(IEnumerable<Product> Products);

public class GetProductsEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("/products", async ([AsParameters] GetProductsRequest request, ISender sender) =>
            {
                var query = request.Adapt<GetProductsQuery>();

                var result = await sender.Send(query);

                var response = result.Adapt<GetProductsResponse>();
                var metadata = new
                {
                    Count = response!.Products.LongCount(),
                    Data = response ?? null

                };
                var responseDto = new ResponseDto(
                    MetaData: metadata,
                    Message: "Get list products successful"
                );
                return Results.Ok(responseDto);
            })
            .WithName("GetProducts")
            .Produces<GetProductsResponse>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .WithSummary("Get Products")
            .WithDescription("Get Products")
            .WithMetadata(new CacheAttribute());
    }
}