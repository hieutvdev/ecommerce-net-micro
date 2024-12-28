namespace Auth.Application.Auths.Commands.AuthDeleteAccount;

public record AuthDeleteAccountCommand(string UserId) : ICommand<AuthDeleteAccountResult>;

public record AuthDeleteAccountResult(bool IsSuccess);