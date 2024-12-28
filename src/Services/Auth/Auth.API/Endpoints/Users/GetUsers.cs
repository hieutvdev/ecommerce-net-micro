using Auth.Application.Auths.Queries.GetUsers;
using BuildingBlocks.Pagination;
using MediatR;

namespace Auth.API.Endpoints.Users;

public record GetUsersResponse(object MetaData, bool IsSuccess = true, string Message = "");

public class GetUsers : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.Map("/users", async ([AsParameters] PaginationRequest request, ISender sender) =>
            {
                var result = await sender.Send(new GetUsersQuery(request));
                var response = new GetUsersResponse(MetaData: result.PaginatedResult, Message: "Get User");
                return Results.Ok(response);
            })
            .WithName("GetUsers")
            .Produces<GetUserResponse>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .WithSummary("GetUsers")
            .WithDescription("Get Users");
    }
}