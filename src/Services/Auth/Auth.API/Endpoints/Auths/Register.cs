using Auth.Application.Auths.Commands.AuthRegister;
using Auth.Application.DTOs.Auth.Requests;
using Auth.Application.DTOs.Auth.Responses;
using AutoMapper;
using BuildingBlocks.DTOs;
using Carter;
using Mapster;
using MediatR;

namespace Auth.API.Endpoints.Auths;

public record RegisterRequest(string Email, string Password, string FullName);

public record RegisterResponse(LoginResponseDto ResponseDto);


public class Register : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("/auths/register", async (RegisterRequest request, ISender sender, IMapper mapper) =>
        {
            var command = request.Adapt<AuthRegisterCommand>();
            var result = await sender.Send(command);
            var registerResult = mapper.Map<AuthRegisterResult>(result);
            var response = new ResponseDto(registerResult.LoginResponseDto, Message: "Register Successful");
            return Results.Ok(response);
        })
        .WithName("Register")
        .Produces<RegisterResponse>(StatusCodes.Status201Created)
        .ProducesProblem(StatusCodes.Status400BadRequest)
        .WithSummary("Register")
        .WithDescription("Register");
    }
}