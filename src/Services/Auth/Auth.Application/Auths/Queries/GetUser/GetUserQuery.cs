namespace Auth.Application.Auths.Queries.GetUser;

public record GetUserQuery(string UserId) : IQuery<GetUserResult>;

public record GetUserResult(
    string UserId,
    string Email,
    string Username,
    string FullName,
    string PhoneNumber,
    string Avatar);
