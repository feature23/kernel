using System.ComponentModel.DataAnnotations;
using DataAnnotationsValidationResult = System.ComponentModel.DataAnnotations.ValidationResult;
using ValidationResult = F23.Kernel.Results.ValidationResult;

namespace F23.Kernel;

/// <summary>
/// A validator that uses <see cref="System.ComponentModel.DataAnnotations"/> attributes to validate an object.
/// </summary>
/// <typeparam name="T">The type of the object to be validated.</typeparam>
public class DataAnnotationsValidator<T> : IValidator<T>
    where T : notnull
{
    /// <summary>
    /// Validates the specified object.
    /// </summary>
    /// <param name="value">The object to validate.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <returns>A task that represents the asynchronous validation operation. The task result contains the validation result.</returns>
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
