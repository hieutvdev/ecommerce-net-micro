using BuildingBlocks.Responses.Result;

namespace BuildingBlocks.Exceptions.Validations;

public class ValidationResult : Result, IValidationResult
{
    private ValidationResult(Error[] errors) : base(false, IValidationResult.ValidationError) => Errors = errors;
    public Error[] Errors { get; }

    public static ValidationResult WithErrors(Error[] errors) => new(errors);
}