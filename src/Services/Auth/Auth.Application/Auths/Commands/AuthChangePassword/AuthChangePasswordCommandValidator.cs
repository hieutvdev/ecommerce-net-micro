using FluentValidation;

namespace Auth.Application.Auths.Commands.AuthChangePassword;

public class AuthChangePasswordCommandValidator : AbstractValidator<AuthChangePasswordCommand>
{
    public AuthChangePasswordCommandValidator()
    {
        RuleFor(c => c.OldPassword).NotEmpty().WithMessage("OldPassword is required!");
        RuleFor(c => c.NewPassword).NotEmpty().WithMessage("NewPassword is required!");
        RuleFor(c => c.ConfirmPassword).NotEmpty().WithMessage("ConfirmPassword is required!");
        RuleFor(c => c.ConfirmPassword)
                .Equal(c => c.NewPassword)
                .WithMessage("ConfirmPassword must match NewPassword");
    }
}