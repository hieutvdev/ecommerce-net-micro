using Auth.Application.Auths.Commands.UpdateRole;
using Mapster;
using MediatR;

namespace Auth.API.Endpoints.Roles;

public record UpdateRoleRequest(string RoleId, string RoleName);

public record UpdateRoleResponse(object? MetaData, bool IsSuccess = true, string Message = "");
public class UpdateRole : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPut("/roles", async (UpdateRoleRequest request, ISender sender) =>
            {
                var command = request.Adapt<UpdateRoleCommand>();
                var result = await sender.Send(command);
                var response = new UpdateRoleResponse(MetaData: null, IsSuccess: result.IsSuccess,
                    Message: result.IsSuccess ? "Update Role Successful" : "Update Role Failure");
                return Results.Ok(response);
            })
            .WithName("UpdateRole")
            .Produces<UpdateRoleResponse>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .WithSummary("UpdateRole")
            .WithDescription("Update Role");
    }
}