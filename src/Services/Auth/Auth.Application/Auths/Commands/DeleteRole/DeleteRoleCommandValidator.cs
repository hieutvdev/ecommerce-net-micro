using FluentValidation;

namespace Auth.Application.Auths.Commands.DeleteRole;

public class DeleteRoleCommandValidator : AbstractValidator<DeleteRoleCommand>
{
    public DeleteRoleCommandValidator()
    {
        RuleFor(r => r.RoleName)
            .NotEmpty()
            .WithName("RoleName is required");
    }
}