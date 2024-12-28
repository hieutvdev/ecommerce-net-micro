using FluentValidation;

namespace Auth.Application.Auths.Commands.RefreshToken;

public class RefreshTokenCommandValidator : AbstractValidator<RefreshTokenCommand>
{
    public RefreshTokenCommandValidator()
    {
        RuleFor(r => r.Token).NotEmpty().WithName("Token cannot be null");
        
    }
}