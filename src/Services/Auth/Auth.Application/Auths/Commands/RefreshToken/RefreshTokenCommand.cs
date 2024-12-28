using Auth.Application.DTOs.Key.Requests;
using Auth.Application.DTOs.Key.Responses;

namespace Auth.Application.Auths.Commands.RefreshToken;

public record RefreshTokenCommand(string Token) : ICommand<RefreshTokenResult>;
public record RefreshTokenResult(string AccessToken, string RefreshToken);