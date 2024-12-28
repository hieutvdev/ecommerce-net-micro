using Auth.Application.DTOs.Key.Requests;

namespace Auth.Application.Auths.Commands.RefreshToken;

public class RefreshTokenHandler
(IKeyRepository<Guid> repository)
: ICommandHandler<RefreshTokenCommand, RefreshTokenResult>
{
    public async Task<RefreshTokenResult> Handle(RefreshTokenCommand command, CancellationToken cancellationToken)
    {

        var refreshTokenRequestDto = new RefreshTokenByUserRequestDto(command.Token);
        var result = await repository.RefreshTokenByUser(refreshTokenRequestDto, cancellationToken);
        return new RefreshTokenResult(result.AccessToken, result.RefreshToken);
    }
}