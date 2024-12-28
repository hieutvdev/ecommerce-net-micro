using FluentValidation;
using Upload.API.UseCase.V1.Commands.UploadFileFromLocal;

namespace Upload.API.UseCase.V1.Validations.UploadFileFromLocal;

public class UploadFileFromLocalCommandValidator : AbstractValidator<UploadFileFromLocalCommand>
{
    public UploadFileFromLocalCommandValidator()
    {
        RuleFor(command => command.Context)
            .NotNull()
            .WithMessage("HttpContext cannot be null.");

        RuleFor(command => command.Context.Request.Form.Files.Count)
            .GreaterThan(0)
            .WithMessage("No files were provided in the request.");

        RuleForEach(command => command.Context.Request.Form.Files).SetValidator(new InlineValidator<IFormFile>());
        
    }
}