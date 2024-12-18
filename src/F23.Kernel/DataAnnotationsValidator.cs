using System.ComponentModel.DataAnnotations;
using DataAnnotationsValidationResult = System.ComponentModel.DataAnnotations.ValidationResult;
using ValidationResult = F23.Kernel.Results.ValidationResult;

namespace F23.Kernel;

public class DataAnnotationsValidator<T> : IValidator<T>
    where T : notnull
{
    public Task<ValidationResult> Validate(T value, CancellationToken cancellationToken = default)
    {
        var context = new ValidationContext(value);
        var results = new List<DataAnnotationsValidationResult>();

        return Validator.TryValidateObject(value, context, results, true)
            ? Task.FromResult(ValidationResult.Passed())
            : Task.FromResult(ValidationResult.Failed(results
                .SelectMany(i => i.MemberNames.Select(n => new { i.ErrorMessage, MemberName = n }))
                .Select(i => new ValidationError(i.MemberName, i.ErrorMessage ?? "Invalid value"))
                .ToList()));
    }
}