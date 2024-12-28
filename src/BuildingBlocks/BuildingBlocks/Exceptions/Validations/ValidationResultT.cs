using BuildingBlocks.Responses.Result;

namespace BuildingBlocks.Exceptions.Validations;

public class ValidationResultT<TValue> : ResultT<TValue>, IValidationResult
{
    private ValidationResultT(Error[] errors) : base(default, false, IValidationResult.ValidationError) => Errors = errors;
    public Error[] Errors { get; }

    public static ValidationResultT<TValue> WithErrors(Error[] errors) => new(errors);
}