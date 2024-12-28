namespace Auth.Application.DTOs.Users.Requests;

public record EditInForUserRequestDto(string UserId,
    string FullName,
    string PhoneNumber,
    string Avatar);