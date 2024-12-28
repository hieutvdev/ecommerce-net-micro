using FluentValidation;

namespace Auth.Application.Auths.Commands.EditInForUser;

public class EditInForUserCommandValidator : AbstractValidator<EditInForUserCommand>
{
    public EditInForUserCommandValidator()
    {
        RuleFor(x => x.UserId).NotEmpty().WithMessage("UserId is required");
        RuleFor(x => x.FullName).NotEmpty().WithMessage("FullName is required");
    }
}