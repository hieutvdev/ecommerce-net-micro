namespace Auth.Application.DTOs.Auth.Responses;

public record LoginResponseDto(UserDto User, LoginTokenResponseDto Token);