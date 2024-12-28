using Auth.Application.Auths.Commands.AuthLockAccount;
using Azure.Core;
using BuildingBlocks.DTOs;
using Carter;
using Mapster;
using MediatR;

namespace Auth.API.Endpoints.Auths;


public record LockAccountRequest(int Expire);

public record LockAccountResponse(ResponseDto ResponseDto);

public class LockAccount : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPatch("/auths/lock-account", async (LockAccountRequest request, ISender sender) =>
            {
                var command = request.Adapt<AuthLockAccountCommand>();
                var result = await sender.Send(command);
                var responseDto = new ResponseDto(IsSuccess: result.IsSuccess,
                    Message: result.IsSuccess ? "Locked Successful" : "Locked Failure");
                var response = new LockAccountResponse(responseDto);
                return Results.Ok(response);
            })
            .WithName("LockAccount")
            .Produces<LockAccountResponse>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .WithSummary("Lock Account")
            .WithDescription("Lock Account");
    }
}