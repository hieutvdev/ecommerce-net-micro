namespace Auth.Application.DTOs.Key.Responses;

public record RefreshTokenByUserResponseDto(string AccessToken, string RefreshToken);
