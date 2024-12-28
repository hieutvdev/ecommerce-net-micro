using FluentValidation;
using Microsoft.AspNetCore.Http;

namespace Auth.Application.Auths.Commands.CreateRole;

public class CreateRoleCommandValidator : AbstractValidator<CreateRoleCommand>
{
    public CreateRoleCommandValidator()
    {
        RuleFor(x => x.RoleName)
            .NotEmpty()
            .WithMessage("Role name is required")
            .Must(x => x.Length is > 3 and <= 20)
            .WithMessage("Each role name must be between 3 and 20 characters long");
    }
}