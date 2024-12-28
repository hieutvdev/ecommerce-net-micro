using FluentValidation;
using Upload.API.UseCase.V1.Commands.DeleteFileCloudinary;

namespace Upload.API.UseCase.V1.Validations.DeleteFileCloudinary;

public class DeleteFileCloudinaryCommandValidator : AbstractValidator<DeleteFileCloudinaryCommand>
{
    public DeleteFileCloudinaryCommandValidator()
    {
        RuleFor(x => x.PublicId)
            .NotEmpty()
            .WithMessage("PublicId is required");
    }
}