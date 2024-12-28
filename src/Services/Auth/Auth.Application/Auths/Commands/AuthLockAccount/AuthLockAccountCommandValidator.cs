using FluentValidation;

namespace Auth.Application.Auths.Commands.AuthLockAccount;

public class AuthLockAccountCommandValidator : AbstractValidator<AuthLockAccountCommand>
{
    public AuthLockAccountCommandValidator()
    {
        RuleFor(x => x.Expire).NotEmpty().WithMessage("Expire is required");
        RuleFor(x => x.Expire).InclusiveBetween(0, 7).WithMessage("Expire must be greater than 0 and less than 7 days");
    }
}