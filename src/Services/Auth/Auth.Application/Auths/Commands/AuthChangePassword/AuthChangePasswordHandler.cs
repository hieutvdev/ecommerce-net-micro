namespace Auth.Application.Auths.Commands.AuthChangePassword;

public class AuthChangePasswordHandler
(IAuthRepository authRepository)
: ICommandHandler<AuthChangePasswordCommand, AuthChangePasswordResult>
{
    public async Task<AuthChangePasswordResult> Handle(AuthChangePasswordCommand command, CancellationToken cancellationToken)
    {
        var changePasswordDto = ChangePasswordToDto(command);
        var result = await authRepository.ChangePasswordAsync(changePasswordDto);
        return new AuthChangePasswordResult(result);
    }

    private static ChangePasswordRequestDto ChangePasswordToDto(AuthChangePasswordCommand command)
        => new ChangePasswordRequestDto(command.OldPassword, command.NewPassword, command.ConfirmPassword);
}