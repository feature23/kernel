using F23.Kernel.Results;

namespace F23.Kernel;

/// <summary>
/// A validator that validates an object.
/// </summary>
/// <typeparam name="T">The type of the object to be validated.</typeparam>
public interface IValidator<in T>
{
    /// <summary>
    /// Validates the specified object.
    /// </summary>
    /// <param name="value">The object to validate.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <returns>A task that represents the asynchronous validation operation. The task result contains the validation result.</returns>
    Task<ValidationResult> Validate(T value, CancellationToken cancellationToken = default);
}
