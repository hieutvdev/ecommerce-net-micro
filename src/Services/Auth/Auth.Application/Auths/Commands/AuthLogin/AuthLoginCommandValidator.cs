using FluentValidation;

namespace Auth.Application.Auths.Commands.AuthLogin;

public class AuthLoginCommandValidator : AbstractValidator<AuthLoginCommand>
{
    public AuthLoginCommandValidator()
    {
        RuleFor(r => r.Email).NotEmpty().WithMessage("Email is required");
        RuleFor(r => r.Password).NotEmpty().WithMessage("Password is required");
    }
}