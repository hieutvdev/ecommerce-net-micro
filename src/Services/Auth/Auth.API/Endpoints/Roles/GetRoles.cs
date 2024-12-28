using Auth.Application.Auths.Queries.GetRoles;
using MediatR;

namespace Auth.API.Endpoints.Roles;


public record GetRolesResponse(object? MetaData, bool IsSuccess = true, string Message = "");

public class GetRoles : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("/roles", async (ISender sender) =>
            {
                var result = await sender.Send(new GetRolesQuery());
                var response = new GetRolesResponse(MetaData: result.Response, Message: "Get Roles Successful");
                return Results.Ok(response);
            })
            .WithName("GetRoles")
            .Produces<GetRolesResponse>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .WithSummary("GetRoles")
            .WithDescription("Get Roles");
    }
}