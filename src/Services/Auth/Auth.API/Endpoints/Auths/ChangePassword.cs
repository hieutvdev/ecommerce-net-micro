using Auth.Application.Auths.Commands.AuthChangePassword;
using BuildingBlocks.DTOs;
using Carter;
using Mapster;
using MediatR;

namespace Auth.API.Endpoints.Auths;

public record ChangePasswordRequest(string OldPassword, string NewPassword, string ConfirmPassword);

public record ChangePasswordResponse(ResponseDto ResponseDto);

public class ChangePassword : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPatch("/auths/change-password", async (ChangePasswordRequest request, ISender sender) =>
            {
                var command = request.Adapt<AuthChangePasswordCommand>();
                var result = await sender.Send(command);
                var responseDto = new ResponseDto(IsSuccess: result.IsSuccess,
                    Message: result.IsSuccess ? "ChangePassword successful" : "ChangePassword Failure");

                var response = new ChangePasswordResponse(responseDto);
                return Results.Ok(response);
                ;
            })
       
            .WithName("ChangePassword")
            .Produces<ChangePasswordResponse>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .WithSummary("Change Password")
            .WithDescription("Change password");
    }
}