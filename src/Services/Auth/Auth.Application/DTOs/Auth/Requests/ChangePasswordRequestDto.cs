namespace Auth.Application.DTOs.Auth.Requests;

public record ChangePasswordRequestDto(string OldPassword, string NewPassword, string ConfirmPassword);