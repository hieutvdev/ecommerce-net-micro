using Auth.Application.Auths.Commands.AuthLockAccount;

namespace Auth.Application.Auths.Commands.AuthDeleteAccount;

public class AuthDeleteAccountHandler
(IAuthRepository authRepository)
: ICommandHandler<AuthDeleteAccountCommand, AuthDeleteAccountResult>
{
    public async Task<AuthDeleteAccountResult> Handle(AuthDeleteAccountCommand command, CancellationToken cancellationToken)
    {
        var dto = DeleteAccountCommandToDto(command);
        var result = await authRepository.DeleteUserAsync(dto);
        return new AuthDeleteAccountResult(result);
    }

    private static DeleteUserRequestDto DeleteAccountCommandToDto(AuthDeleteAccountCommand command) =>
        new DeleteUserRequestDto(command.UserId);
}