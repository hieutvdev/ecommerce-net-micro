namespace Auth.Application.Auths.Commands.AuthChangePassword;

public record AuthChangePasswordCommand
    (string OldPassword, string NewPassword, string ConfirmPassword) : ICommand<AuthChangePasswordResult>;

public record AuthChangePasswordResult(bool IsSuccess);