namespace Catalog.API.Products.DataPake;

public class DataFakeEndpoint: ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("/products/fakes",
                async ( ISender sender) =>
                {
                    var result = await sender.Send(new DataFakeCommand());

                    var response = result.Adapt<DataFakeResult>();

                    return Results.Ok(response);

                })
            .WithName("DataFake")
            .Produces<DataFakeResult>(StatusCodes.Status201Created)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .WithSummary("DataFake")
            .WithDescription("DataFake");
    }
}

