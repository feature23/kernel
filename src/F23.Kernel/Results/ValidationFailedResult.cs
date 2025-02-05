namespace F23.Kernel.Results;

/// <summary>
/// Represents a validation failure result containing validation errors.
/// </summary>
public class ValidationFailedResult(IReadOnlyCollection<ValidationError> errors) : ValidationResult(false)
{
    /// <summary>
    /// Gets a message describing the outcome of the operation.
    /// </summary>
    public override string Message => "The operation failed due to validation errors.";

    /// <summary>
    /// Gets a collection of <see cref="ValidationError"/> instances associated with the validation failure.
    /// </summary>
    /// <remarks>
    /// This property provides detailed information about the errors that occurred during validation,
    /// including the key associated with each error and its corresponding message.
    /// </remarks>
    public IReadOnlyCollection<ValidationError> Errors => errors;
}

/// <summary>
/// Represents a validation failure result that includes validation errors.
/// </summary>
/// <typeparam name="T">The type of the result's associated value, if the operation had succeeded.</typeparam>
public class ValidationFailedResult<T>(IReadOnlyCollection<ValidationError> errors) : ValidationResult<T>(false)
{
    /// <summary>
    /// Gets a message describing the outcome of the operation.
    /// </summary>
    public override string Message => "The operation failed due to validation errors.";

    /// <summary>
    /// Gets a collection of <see cref="ValidationError"/> instances associated with the validation failure.
    /// </summary>
    /// <remarks>
    /// This property provides detailed information about the errors that occurred during validation,
    /// including the key associated with each error and its corresponding message.
    /// </remarks>
    public IReadOnlyCollection<ValidationError> Errors => errors;
}
