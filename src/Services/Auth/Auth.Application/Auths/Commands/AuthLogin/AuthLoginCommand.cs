namespace Auth.Application.Auths.Commands.AuthLogin;

public record AuthLoginCommand(string Email, string Password) : ICommand<AuthLoginResult>;

public record AuthLoginResult(LoginResponseDto ResponseDto);
