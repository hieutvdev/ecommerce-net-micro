namespace Auth.Application.DTOs.Auth.Responses;

public record LoginTokenResponseDto(string AccessToken, string RefreshToken);
