using FluentValidation;

namespace Auth.Application.Auths.Commands.CreateKey;

public class CreateKeyCommandValidator : AbstractValidator<CreateKeyCommand>
{
    public CreateKeyCommandValidator()
    {
        RuleFor(x => x.Request).NotEmpty().WithMessage("KeyModel is required");
        RuleFor(x => x.Request.Token).NotEmpty().WithMessage("Token is required");
        RuleFor(x => x.Request.UserId).NotEmpty().WithMessage("Userid is required");
    }
}