using Auth.Application.Auths.Queries.GetUser;
using MediatR;

namespace Auth.API.Endpoints.Users;

public record GetUserResponse(object MetaData, bool IsSuccess = true, string Message = "");

public class GetUser : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("/users/{id}", async (string id, ISender sender) =>
            {
                var result = await sender.Send(new GetUserQuery(id));
                var response = new GetUserResponse(result, IsSuccess: (bool)(result != null),
                    Message: result != null ? "Get User Successful" : "Get User Failure");
                return Results.Ok(response);
            })
            .WithName("GetUser")
            .Produces<GetUserResponse>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .WithSummary("GetUser")
            .WithDescription("Get User");
    }
}