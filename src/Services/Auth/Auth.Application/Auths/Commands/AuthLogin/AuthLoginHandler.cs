namespace Auth.Application.Auths.Commands.AuthLogin;

public class AuthLoginHandler
(IAuthRepository repository, IMapper mapper)
: ICommandHandler<AuthLoginCommand, AuthLoginResult>
{
    public async Task<AuthLoginResult> Handle(AuthLoginCommand command, CancellationToken cancellationToken)
    {
        var request = mapper.Map<LoginRequestDto>(command);
        var response = await repository.LoginAsync(request);
        return new AuthLoginResult(response);
    }
}