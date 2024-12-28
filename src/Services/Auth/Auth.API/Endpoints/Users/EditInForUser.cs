using Auth.Application.Auths.Commands.EditInForUser;
using Mapster;
using MediatR;

namespace Auth.API.Endpoints.Users;

public record EditInForUserRequest(string UserId,string FullName, string PhoneNumber, string Avatar);

public record EditInForUserResponse(object MetaData, bool IsSuccess = true, string Message = "");

public class EditInForUser : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPatch("/users", async (EditInForUserRequest request, ISender sender) =>
            {
                var command = request.Adapt<EditInForUserCommand>();
                var result = await sender.Send(command);

                var response = new EditInForUserResponse(result, IsSuccess: !string.IsNullOrEmpty(result.UserId),
                    Message: !string.IsNullOrEmpty(result.UserId) ? "Success" : "Failure");

                return Results.Ok(response);
            })
            .WithName("EditInForUser")
            .Produces<EditInForUserResponse>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .WithSummary("EditInForUser")
            .WithDescription("Edit InFro User");
    }
}