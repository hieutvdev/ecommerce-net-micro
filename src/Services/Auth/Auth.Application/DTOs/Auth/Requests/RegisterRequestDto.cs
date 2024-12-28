namespace Auth.Application.DTOs.Auth.Requests;

public record RegisterRequestDto(string Email, string Password, string FullName);
