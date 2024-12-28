using Auth.Application.Auths.Queries.GetKeys;
using Auth.Application.DTOs.Key;
using BuildingBlocks.Pagination;
using Mapster;
using MediatR;

namespace Auth.API.Endpoints.Keys;


public record GetKeysResponse(object MetaData, bool IsSuccess = true, string Message = "");


public class GetKeys : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("/keys", async ([AsParameters] PaginationRequest request, ISender sender) =>
            {
                var result = await sender.Send(new GetKeysQuery(request));
                var response = new GetKeysResponse(MetaData: result.PaginatedResult, Message: "Get Keys Successful");
                return Results.Ok(response);
            })
            .WithName("GetKeys")
            .Produces<GetKeysResponse>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .WithSummary("GetKeys")
            .WithDescription("Get keys");
    }
}