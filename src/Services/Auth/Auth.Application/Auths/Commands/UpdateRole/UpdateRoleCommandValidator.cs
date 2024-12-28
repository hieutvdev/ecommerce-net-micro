using FluentValidation;

namespace Auth.Application.Auths.Commands.UpdateRole;

public class UpdateRoleCommandValidator : AbstractValidator<UpdateRoleCommand>
{
    public UpdateRoleCommandValidator()
    {
        RuleFor(x => x.RoleId)
            .NotEmpty()
            .WithMessage("RoleId is required");
        RuleFor(x => x.RoleName)
            .NotEmpty()
            .WithMessage("RoleName is required")
            .Must(x => x.Length is > 3 and <= 20)
            .WithName("Each role name must be between 3 and 20 characters long");
    }
}

