
namespace Auth.Application.Auths.Commands.AuthRegister;
public class AuthRegisterHandler
(IAuthRepository authRepository, IMapper mapper)
: ICommandHandler<AuthRegisterCommand, AuthRegisterResult>
{
    public async Task<AuthRegisterResult> Handle(AuthRegisterCommand command, CancellationToken cancellationToken)
    {
        var user = mapper.Map<RegisterRequestDto>(command);
        var response = await authRepository.RegisterAsync(user);
        return new AuthRegisterResult(response);
    }
}