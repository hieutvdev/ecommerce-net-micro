using FluentValidation;

namespace Auth.Application.Auths.Commands.AssignRoles;

public class AssignRolesCommandValidator : AbstractValidator<AssignRolesCommand>
{
    public AssignRolesCommandValidator()
    {
        RuleFor(x => x.RoleNames)
            .NotEmpty().WithName("RoleNames is required")
            .Must(rolesNames => rolesNames.All(name => !string.IsNullOrWhiteSpace(name)))
            .WithName("RoleNames cannot contain empty or whitespace names")
            .Must(roleNames => roleNames.All(name => name.Length is >= 3 and <= 20))
            .WithName("Each role name must be between 3 and 20 characters long");

        RuleFor(x => x.Email)
            .NotEmpty().WithName("Email is required");
    }
}