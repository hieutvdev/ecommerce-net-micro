using Auth.Application.Auths.Commands.DeleteRole;
using Mapster;
using MediatR;

namespace Auth.API.Endpoints.Roles;

public record DeleteRoleRequest(string RoleName);

public record DeleteRoleResponse(object? MetaData, bool IsSuccess = true, string Message = "");

public class DeleteRole : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapDelete("/roles/{name}", async (string name, ISender sender) =>
            {
                var request = new DeleteRoleRequest(name);
                var command = request.Adapt<DeleteRoleCommand>();
                var result = await sender.Send(command);
                var response = new DeleteRoleResponse(MetaData: null, IsSuccess: result.IsSuccess,
                    Message: result.IsSuccess ? "Delete Role Successful" : "Delete role failure");

                return Results.Ok(response);
            })
            .WithName("DeleteRole")
            .Produces<DeleteRoleResponse>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .WithSummary("DeleteRole")
            .WithDescription("Delete Role");
    }
}