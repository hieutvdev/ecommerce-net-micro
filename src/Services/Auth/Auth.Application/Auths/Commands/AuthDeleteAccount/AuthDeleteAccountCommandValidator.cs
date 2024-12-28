using Auth.Application.Auths.Commands.AuthLockAccount;
using FluentValidation;

namespace Auth.Application.Auths.Commands.AuthDeleteAccount;

public class AuthDeleteAccountCommandValidator : AbstractValidator<AuthDeleteAccountCommand>
{
    public AuthDeleteAccountCommandValidator()
    {
        RuleFor(x => x.UserId).NotEmpty().WithMessage("UserId is required");
    }
}