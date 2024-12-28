using Auth.Application.Auths.Commands.CreateRole;
using Mapster;
using MediatR;

namespace Auth.API.Endpoints.Roles;


public record CreateRoleRequest(string RoleName);

public record CreateRoleReponse(object? MetaData, bool IsSuccess = true, string Message = "");

public class CreateRole : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("/roles", async (CreateRoleRequest request, ISender sender) =>
            {
                var command = request.Adapt<CreateRoleCommand>();
                var result = await sender.Send(command);
                var response = new CreateRoleReponse(IsSuccess: result.IsSuccess,
                    MetaData: result.IsSuccess ? "Create Role Successful" : "Create Role Failure");

                return Results.Ok(response);
            })
            .RequireAuthorization()
            .WithName("CreateRole")
            .Produces<CreateRoleReponse>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .WithSummary("CreateRole")
            .WithDescription("Create Role");
    }
}