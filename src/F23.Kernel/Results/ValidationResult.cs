namespace F23.Kernel.Results;

/// <summary>
/// Represents the result of a validation operation.
/// </summary>
public abstract class ValidationResult(bool isSuccess) : Result(isSuccess)
{
    /// <summary>
    /// Creates a validation result that represents a failure with a single validation error.
    /// </summary>
    /// <param name="error">The validation error to include in the result.</param>
    /// <returns>A <see cref="ValidationResult"/> indicating failure with the provided error.</returns>
    public static ValidationResult Failed(ValidationError error) => new ValidationFailedResult([error]);

    /// <summary>
    /// Creates a failed validation result with a key and message describing the validation error.
    /// </summary>
    /// <param name="key">The key associated with the validation error.</param>
    /// <param name="message">The validation error message.</param>
    /// <returns>A <see cref="ValidationResult"/> representing the failure with the specified validation error details.</returns>
    public static ValidationResult Failed(string key, string message) => new ValidationFailedResult([new ValidationError(key, message)]);

    /// <summary>
    /// Creates a validation result with the specified collection of validation errors.
    /// </summary>
    /// <param name="errors">A collection of <see cref="ValidationError"/> that caused the validation failure.</param>
    /// <returns>A <see cref="ValidationResult"/> indicating failure with the provided errors.</returns>
    public static ValidationResult Failed(IReadOnlyCollection<ValidationError> errors) => new ValidationFailedResult(errors);

    /// <summary>
    /// Creates a <see cref="ValidationResult"/> that represents a successful validation outcome.
    /// </summary>
    /// <returns>A <see cref="ValidationResult"/> indicating that validation passed.</returns>
    public static ValidationResult Passed() => new ValidationPassedResult();
}

/// <summary>
/// Represents the result of a validation operation.
/// </summary>
/// <typeparam name="T">The type of the associated value.</typeparam>
public abstract class ValidationResult<T>(bool isSuccess) : Result<T>(isSuccess)
{
    /// <summary>
    /// Creates a validation result with the specified collection of validation errors.
    /// </summary>
    /// <param name="errors">A collection of <see cref="ValidationError"/> that caused the validation failure.</param>
    /// <returns>A <see cref="ValidationResult"/> indicating failure with the provided errors.</returns>
    public static ValidationResult<T> Failed(IReadOnlyCollection<ValidationError> errors) => new ValidationFailedResult<T>(errors);

    /// <summary>
    /// Creates a <see cref="ValidationResult"/> that represents a successful validation outcome.
    /// </summary>
    /// <returns>A <see cref="ValidationResult"/> indicating that validation passed.</returns>
    public static ValidationResult<T> Passed() => new ValidationPassedResult<T>();
}
