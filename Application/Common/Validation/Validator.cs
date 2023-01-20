using FluentValidation;
using FluentValidation.Results;

namespace Application.Common.Validation;

public abstract class Validator<T> : AbstractValidator<T>
{
    public static string Default = "Bad request";

    public ValidationResult StdValidate(ValidationContext<T> context)
    {
        return Validate(context);
    }

    public Task<ValidationResult> StdValidateAsync(ValidationContext<T> context,
        CancellationToken cancellation = default)
    {
        return ValidateAsync(context, cancellation)
            .ContinueWith(
                x => x.Result, cancellation
            );
    }


    public ValidationResult StdValidate(T instance)
    {
        return ValidationsTools.FromFluentValidationResult(Validate(instance));
    }

    public Task<ValidationResult> StdValidateAsync(T instance, CancellationToken cancellation = default)
    {
        return ValidateAsync(instance, cancellation)
            .ContinueWith(x =>
                    ValidationsTools.FromFluentValidationResult(x.Result), cancellation
            );
    }
}