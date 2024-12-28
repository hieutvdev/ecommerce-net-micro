using Catalog.API.Models;

namespace Catalog.API.Products.SearchProduct;

public record SearchProductRequest(string? KeySearch, int? PageNumber = 1, int? PageSize = 10) : IQuery<SearchProductResponse>;

public record SearchProductResponse(IEnumerable<Product> Products);

public class SearchProductEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("/products/search", async ([AsParameters] SearchProductRequest request, ISender sender) =>
        {
            

            var query = request.Adapt<SearchProductQuery>();
            var result = await sender.Send(query);

            var response = result.Adapt<SearchProductResponse>();
            var responseDto = new ResponseDto(
                MetaData: response ?? null,
                Message:"Search Product Successful");
            return Results.Ok(responseDto);
        })
            .WithName("SearchProducts")
            .Produces<SearchProductResponse>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .WithSummary("Search Products")
            .WithDescription("Search Products");
    }
}