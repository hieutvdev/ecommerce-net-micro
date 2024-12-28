
using Auth.Application.Auths.Commands.AuthDeleteAccount;
using BuildingBlocks.DTOs;
using Mapster;
using MediatR;

namespace Auth.API.Endpoints.Auths;

public record DeleteAccountRequest(string UserId);

public record DeleteAccountResponse(ResponseDto ResponseDto);

public class DeleteAccount : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPatch("/auths/delete-account", async (DeleteAccountRequest request, ISender sender) =>
            {
                var command = request.Adapt<AuthDeleteAccountCommand>();
                var result = await sender.Send(command);
                var responseDto = new ResponseDto(IsSuccess: result.IsSuccess,
                    Message: result.IsSuccess ? "Delete Account Successful" : "Delete Account Failure");
                var response = new DeleteAccountResponse(responseDto);
                return Results.Ok(response);
            })
            .WithName("DeleteAccount")
            .Produces<DeleteAccountResponse>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .WithSummary("Delete Account")
            .WithDescription("Delete Account");
    }
}