

namespace Auth.Application.DTOs.Users.Responses;

public record GetUsersResponseDto(
    string Email,
    string FullName,
    string PhoneNumber,
    string Avatar,
    int Status,
    IList<string> Roles);
    
    
