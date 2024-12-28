using Auth.Application.Auths.Commands.RefreshToken;
using Auth.Application.DTOs.Key.Responses;
using AutoMapper;
using Azure.Core;
using BuildingBlocks.DTOs;
using Carter;
using Mapster;
using MediatR;

namespace Auth.API.Endpoints.Keys;

public record RefreshTokenRequest(string Token);

public record RefreshTokenResponse(string AccessToken, string RefreshToken);


public class RefreshToken : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("/keys/refreshToken", async (RefreshTokenRequest request, ISender sender, IMapper mapper) =>
            {
                var command = request.Adapt<RefreshTokenCommand>();
                var result = await sender.Send(command);
                var response = new ResponseDto(result, Message: "Refresh Token Successful");
                return Results.Ok(response);
            })
            .WithName("RefreshToken")
            .Produces<RefreshTokenResponse>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status401Unauthorized)
            .WithSummary("RefreshToken")
            .WithDescription("Refresh Token");
    }
}