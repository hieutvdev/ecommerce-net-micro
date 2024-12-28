using Auth.Application.Auths.Commands.AuthLogin;
using Auth.Application.DTOs.Auth.Responses;
using AutoMapper;
using BuildingBlocks.DTOs;
using Carter;
using Mapster;
using MediatR;

namespace Auth.API.Endpoints.Auths;

public record AuthLoginRequest(string Email, string Password);

public record AuthLoginResponse(LoginResponseDto ResponseDto);

public class Login : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("/auths/login", async (AuthLoginRequest request, ISender sender, IMapper mapper) =>
            {
                var command = request.Adapt<AuthLoginCommand>();
                var result = await sender.Send(command);
                
                var loginResult = mapper.Map<LoginResponseDto>(result.ResponseDto);
                var response = new ResponseDto(loginResult, Message: "Login successful");

                return Results.Ok(response);

            }).WithName("Login")
            .RequireRateLimiting("AuthRate")
            .Produces<AuthLoginResponse>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .WithSummary("Login")
            .WithDescription("Login");
    }
}