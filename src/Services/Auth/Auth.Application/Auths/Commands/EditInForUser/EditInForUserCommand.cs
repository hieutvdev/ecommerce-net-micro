namespace Auth.Application.Auths.Commands.EditInForUser;

public record EditInForUserCommand(string UserId,string FullName, string PhoneNumber, string Avatar) : ICommand<EditInForUserResult>;

public record EditInForUserResult(string UserId,
    string Email,
    string Username,
    string FullName,
    string PhoneNumber,
    string Avatar);