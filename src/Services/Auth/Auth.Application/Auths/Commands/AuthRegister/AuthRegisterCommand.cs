
using FluentValidation;

namespace Auth.Application.Auths.Commands.AuthRegister;

public record AuthRegisterCommand(string Email, string Password, string FullName) : ICommand<AuthRegisterResult>;
public record AuthRegisterResult(LoginResponseDto LoginResponseDto);
public class AuthRegisterCommandValidator : AbstractValidator<AuthRegisterCommand>
{
    public AuthRegisterCommandValidator()
    {
        RuleFor(x => x.FullName)
            .NotEmpty()
            .NotNull()
            .WithMessage("Fullname is required");

        RuleFor(x => x.Email)
            .NotEmpty()
            .WithMessage("Email is required");

        RuleFor(x => x.Password)
            .NotEmpty()
            .WithMessage("Password is required");
    }
}
