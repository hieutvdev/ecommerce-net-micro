namespace Auth.Application.Auths.Commands.AuthLockAccount;

public class AuthLockAccountHandler
(IAuthRepository authRepository)
: ICommandHandler<AuthLockAccountCommand, AuthLockAccountResult>
{
    
    public async Task<AuthLockAccountResult> Handle(AuthLockAccountCommand command, CancellationToken cancellationToken)
    {
        var lockUserRequestDto = LockCommandToDto(command);
        var result = await authRepository.LockUserAsync(lockUserRequestDto);
        return new AuthLockAccountResult(result);
    }

    private static LockUserRequestDto LockCommandToDto(AuthLockAccountCommand command) =>
        new LockUserRequestDto(command.Expire);
}