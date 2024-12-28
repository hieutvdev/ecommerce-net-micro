using Auth.Application.DTOs.Users.Requests;
using Auth.Application.DTOs.Users.Responses;

namespace Auth.Application.Extensions;

public static class AuthExtensions
{
    public static GetUserResponseDto ApplicationUserToUserInFo(ApplicationUser user)
        => new GetUserResponseDto(user.Id, user.Email ?? "", user.UserName ?? "", user.FullName, user.PhoneNumber ?? "", user.Avatar);
    
    
}