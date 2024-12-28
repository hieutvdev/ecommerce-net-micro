using Auth.Application.DTOs.Auth.Requests;
using Auth.Application.DTOs.Auth.Responses;

namespace Auth.Application.Services.IServices;

public interface IAuthRepository
{
    Task<LoginResponseDto> LoginAsync(LoginRequestDto dto);
    Task<LoginResponseDto> RegisterAsync(RegisterRequestDto dto);
    Task<bool> ChangePasswordAsync(ChangePasswordRequestDto dto);
    Task<bool> LockUserAsync(LockUserRequestDto dto);
    Task<bool> DeleteUserAsync(DeleteUserRequestDto dto);
}