using Auth.Application.Auths.Commands.AssignRoles;
using Mapster;
using MediatR;

namespace Auth.API.Endpoints.Roles;



public record AssignRolesRequest(string[] RoleNames, string Email);

public record AssignRolesResponse(object? MetaData, bool IsSuccess = true, string Message = "");

public class AssignRoles : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("/roles/assign-roles", async (AssignRolesRequest request, ISender sender) =>
            {
                var command = request.Adapt<AssignRolesCommand>();
                var result = await sender.Send(command);
                var response = new AssignRolesResponse(MetaData: null, IsSuccess: result.IsSuccess,
                    Message: result.IsSuccess ? "AssignRole Successful" : "AssignRole Failure");
                return Results.Ok(response);
            })
            .WithName("AssignRoles")
            .Produces<AssignRolesResponse>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .WithSummary("AssignRoles")
            .WithDescription("Assign Roles");
    }
}